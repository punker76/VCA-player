// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DZ.MediaPlayer.Vlc.Exceptions;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

#endregion

namespace DZ.MediaPlayer.Vlc.Deployment {

	/// <summary>
	/// Defines deployment routines. This class can
	/// help to deploy vlc library automatically and
	/// control files.
	/// </summary>
	public sealed partial class VlcDeployment {
		private static global::Common.Logging.ILog logger = 
			global::Common.Logging.LogManager.GetLogger(typeof(VlcDeployment));
		private static VlcDeployment defaultDeployment;
		private static DeterminedOSType osType;
		private static bool osSpecificTasksRun;

		private readonly Dictionary<string, string> deploymentContent;
		private readonly string deploymentLocation;
		private readonly HashAlgorithm hashReceiver;
		private readonly string packageHash;
		private readonly string packageLocation;
		private readonly string vlcVersion;

		private VlcDeploymentFailReason deploymentFailReason;

		/// <summary>
		/// Fail reason of last checks.
		/// </summary>
		public VlcDeploymentFailReason FailReason {
			get {
				return (deploymentFailReason);
			}
		}

		/// <summary>
		/// Instantiates with specific properties.
		/// </summary>
		/// <param name="version">Version of required library.</param>
		/// <param name="deploymentLocation">Where to deploy.</param>
		/// <param name="packageLocation">Location of zipped vlc library.</param>
		/// <param name="packageHash">Hash of package.</param>
		/// <param name="deploymentContent">Dictionary of files contained in package. Key is name of file, and value is hash.</param>
		/// <param name="hashReceiver">How to compute hash.</param>
		/// <exception cref="ArgumentException">Some of parameter is invalid.</exception>
		public VlcDeployment (string version, string deploymentLocation, string packageLocation, string packageHash, IDictionary<string, string> deploymentContent, HashAlgorithm hashReceiver) {
			//
			if (version == null) {
				throw new ArgumentNullException ("version");
			}
			if (version.Length == 0) {
				throw new ArgumentException ("Version is empty.", "version");
			}
			if (deploymentLocation == null) {
				throw new ArgumentNullException ("deploymentLocation");
			}
			if (deploymentLocation.Length == 0) {
				throw new ArgumentException ("Deployment location is empty.", "deploymentLocation");
			}
			if (deploymentLocation.IndexOfAny (Path.GetInvalidPathChars ()) > 0) {
				throw new ArgumentException ("Deployment location contains invalid path characters.");
			}
			if (packageLocation == null) {
				throw new ArgumentNullException ("packageLocation");
			}
			if (packageLocation.Length == 0) {
				throw new ArgumentException ("Package location is empty.", "packageLocation");
			}
			if (packageLocation.IndexOfAny (Path.GetInvalidPathChars ()) > 0) {
				throw new ArgumentException ("Package location contains invalid path characters.", "packageLocation");
			}
			if (packageHash == null) {
				throw new ArgumentNullException ("packageHash");
			}
			if (packageHash.Length == 0) {
				throw new ArgumentException ("Hash is empty.", "packageHash");
			}
			if (deploymentContent == null) {
				throw new ArgumentNullException ("deploymentContent");
			}
			foreach (KeyValuePair<string, string> pair in deploymentContent) {
				string filePath = pair.Key;
				string fileHash = pair.Value;
				//
				if (string.IsNullOrEmpty (filePath) || string.IsNullOrEmpty (fileHash)) {
					throw new ArgumentNullException ("deploymentContent", "Empty file path or hash found in dictionary.");
				}
				if (filePath.IndexOfAny (Path.GetInvalidPathChars ()) >= 0 && Path.IsPathRooted (filePath)) {
					throw new ArgumentNullException ("deploymentContent", "Invalid path characters found or path is not relative.");
				}
			}
			if (hashReceiver == null) {
				throw new ArgumentNullException ("hashReceiver");
			}
			hashReceiver.Initialize ();
			//
			vlcVersion = version;
			this.deploymentLocation = deploymentLocation;
			this.packageLocation = packageLocation;
			this.packageHash = packageHash;
			this.deploymentContent = new Dictionary<string, string> (deploymentContent);
			this.hashReceiver = hashReceiver;
		}

		private static void determineOSType () {
			//
			if (osType == VlcDeployment.DeterminedOSType.Unknown) {
				OperatingSystem system = Environment.OSVersion;
				if (system.Platform == PlatformID.Win32NT || system.Platform == PlatformID.Win32S || system.Platform == PlatformID.Win32Windows || system.Platform == PlatformID.WinCE) {
					//
					osType = VlcDeployment.DeterminedOSType.Windows;
					//
				}
				if ( system.Platform == PlatformID.MacOSX || system.Platform == PlatformID.Unix ) {
					// TODO: determine Mac OS X
					if (Directory.Exists("/System")) {
						osType = VlcDeployment.DeterminedOSType.MacOS;
					}
				}
			}
		}

		/// <summary>
		/// Deployment location used with <see cref="VlcDeployment"/>.
		/// </summary>
		public string DeploymentLocation {
			get {
				return deploymentLocation;
			}
		}

		/// <summary>
		/// Get's default vlc deployment.
		/// </summary>
		public static VlcDeployment Default {
			get {
				if (defaultDeployment == null) {
					defaultDeployment = createVlcDeployment ();
				}
				return (defaultDeployment);
			}
		}
		
		// create instance which suits for current OS
		private static VlcDeployment createVlcDeployment () {
			determineOSType ();
			if (osType == VlcDeployment.DeterminedOSType.MacOS) {
				return new VlcDeployment (DefVlcVersion, GetMacOSDeploymentLocation(), GetDefaultPackagePath(GetMacOSPackageFileName()), 
				                          GetMacOSPackageHash(), GetMacOSPackageContents (), GetDefaultHashAlgorithm ());
			} else if (osType == VlcDeployment.DeterminedOSType.Windows) {
				return new VlcDeployment (DefVlcVersion, GetWindowsOSDeploymentLocation (), GetDefaultPackagePath (GetWindowsOSPackageFileName()), 
				                          GetWindowsOSPackageHash (), GetWindowsOSPackageContents (), GetDefaultHashAlgorithm ());
			} else {
				throw new NotSupportedException ("Operating system is not supported.");
			}
		}

		/// <summary>
		/// Checks for existence of library.
		/// </summary>
		/// <param name="checkHashes"><code>True</code> to compare hashes.</param>
		/// <param name="tryLoad"><code>True</code> to try load of library.</param>
		/// <returns><code>False</code> if library does not exists.</returns>
		public bool CheckVlcLibraryExistence (bool checkHashes, bool tryLoad) {
			//
			VlcDeploymentFailReason failReason = deploymentFailReason;
			try {
				deploymentFailReason = 0;
				DirectoryInfo info = new DirectoryInfo (deploymentLocation);
				if (!info.Exists) {
					deploymentFailReason = VlcDeploymentFailReason.EmptyDeployment;
					return (false);
				} else {
					//
					FileInfo[] files = info.GetFiles ();
					//
					List<string> fileNames = new List<string> ();
					//
					foreach (FileInfo file in files) {
						fileNames.Add (file.Name);
					}
					//
					foreach (KeyValuePair<string, string> pair in deploymentContent) {
						string filePath = pair.Key.Replace ('\\', Path.DirectorySeparatorChar);
						if (filePath.StartsWith (Path.DirectorySeparatorChar.ToString ())) {
							filePath = filePath.Substring (1);
						}
						string fileHash = pair.Value;
						//
						string fullFilePath = Path.GetFullPath (Path.Combine (deploymentLocation, filePath));
						if (filePath.LastIndexOf (Path.DirectorySeparatorChar) > 0) {
							string directoryName = Path.GetDirectoryName (filePath);
							string directoryPath = Path.Combine (deploymentLocation, directoryName);
							if (!Directory.Exists (directoryPath)) {
								deploymentFailReason = FailReason | VlcDeploymentFailReason.NotAllFilesDeployed;
								return (false);
							} else {
								if (!File.Exists (fullFilePath)) {
									deploymentFailReason = FailReason | VlcDeploymentFailReason.NotAllFilesDeployed;
									return (false);
								}
							}
						} else {
							// this is file in root
							if (!fileNames.Contains (filePath)) {
								deploymentFailReason = FailReason | VlcDeploymentFailReason.NotAllFilesDeployed;
								return (false);
							}
						}
						if (checkHashes) {
							using (Stream stream = File.Open (fullFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
								byte[] hash = hashReceiver.ComputeHash (stream);
								string hashBase64 = Convert.ToBase64String (hash);
								if (string.Compare (fileHash, hashBase64) != 0) {
									deploymentFailReason = FailReason | VlcDeploymentFailReason.InvalidHashOfFile;
									return (false);
								}
							}
						}
					}
					//
				}
				//
				if (tryLoad) {
					try {
						List<string> parameters = new List<string>();
						if (osType == VlcDeployment.DeterminedOSType.MacOS) {
							string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "plugins");
							parameters.Add("--plugin-path");
							parameters.Add(path);
						}
						using (VlcMediaLibraryFactory factory = new VlcMediaLibraryFactory (parameters.ToArray())) {
							string version = factory.Version;
							if (string.Compare (version, vlcVersion, StringComparison.Ordinal) != 0) {
								deploymentFailReason = FailReason | VlcDeploymentFailReason.LibraryVersionDiffers;
							}
							return (true);
						}
					} catch (VlcInternalException exc) {
						deploymentFailReason = FailReason | VlcDeploymentFailReason.LibraryCannotBeLoaded;
						if (logger.IsWarnEnabled) {
							logger.Warn("Cannot check vlc version.", exc);
						}
					}
				}
				return (FailReason == 0);
			} catch (Exception) {
				deploymentFailReason = failReason;
				throw;
			}
		}
		
		/// <summary>
		/// Operating system detected during library initialization.
		/// </summary>
	    public static DeterminedOSType OSType {
	        get {
	            determineOSType();
	            return osType;
	        }
	    }
		
		/// <summary>
		/// Executes additional tasks specific to OS. 
		/// </summary>
		public static void RunOSDependentTasks() {
			if (!osSpecificTasksRun) {
				//
				determineOSType();
				//
				osSpecificTasksRun = true;
			}
		}

		/// <summary>
		/// Installs library.
		/// </summary>
		/// <param name="checkForPackageHash"><code>True</code> to check for package (zip) hash.</param>
		public void Install (bool checkForPackageHash) {
			Install (checkForPackageHash, false, false, false);
		}

		/// <summary>
		/// Installs library.
		/// </summary>
		/// <param name="checkForPackageHash"><code>True</code> to check for package (zip) hash.</param>
		/// <param name="checkForExistanceAfterInstall"><code>True</code> to additionally validate existance of vlc library after deployment.</param>
		/// <param name="checkExtractedHashes"><code>True</code> to check deployed file hashes.</param>
		/// <param name="tryLoad"><code>True</code> to try loading library.</param>
		public void Install (bool checkForPackageHash, bool checkForExistanceAfterInstall, bool checkExtractedHashes, bool tryLoad) {
			if (!File.Exists (packageLocation)) {
				throw new FileNotFoundException ("Package file not found.", packageLocation);
			}
			if (!Directory.Exists (deploymentLocation)) {
				Directory.CreateDirectory (deploymentLocation);
			}
			if (checkForPackageHash) {
				using (Stream stream = File.Open (packageLocation, FileMode.Open, FileAccess.Read, FileShare.None)) {
					byte[] hash = hashReceiver.ComputeHash (stream);
					string hashBase64 = Convert.ToBase64String (hash);
					//
					if (string.Compare (packageHash, hashBase64) != 0) {
						throw new VlcDeploymentException ("Package hash differs from original.");
					}
					//
					stream.Position = 0;
					stream.Close ();
				}
			}
			//
			try {
				byte[] buffer = new byte[1024];
				using (ZipFile file = new ZipFile(packageLocation)) {
					foreach(ZipEntry entry in file) {
						//
						if ( entry.IsDirectory ) {
							string dir = Path.Combine(deploymentLocation, entry.Name);
							if (!Directory.Exists(dir)) {
								Directory.CreateDirectory(dir);
							}
						} else if (entry.IsFile) {
							string fileName = Path.GetFileName(entry.Name);
							string dir = Path.Combine(deploymentLocation, Path.GetDirectoryName(entry.Name));
							//
							if (!Directory.Exists(dir)) {
								Directory.CreateDirectory(dir);
							}
							string filePath = Path.Combine(dir, fileName);
							if (File.Exists(filePath)) {
								File.Delete(filePath);
							}
							using (FileStream stream = File.Create(filePath)) {
								StreamUtils.Copy(file.GetInputStream(entry), stream, buffer);
							}
						}
						//
					}
				}
			} catch (Exception exc) {
				throw new VlcDeploymentException ("Cannot open package or/and extract package.", exc);
			}
			//
			if (checkForExistanceAfterInstall) {
				if (!CheckVlcLibraryExistence (checkExtractedHashes, tryLoad)) {
					throw new VlcDeploymentException ("Library cannot be loaded after install. Fatal error. See fail reason.");
				}
			}
		}

		/// <summary>
		/// Get's dictionary of file names from directory where key is relative path
		/// of file and value is hash of file.
		/// </summary>
		/// <param name="directoryPath">Path to directory where files are located.</param>
		/// <param name="hashReceiver">Hot to retrieve hashes.</param>
		/// <returns>Dictionary of file hashes.</returns>
		public static Dictionary<string, string> GetDirectoryStructureHashes (string directoryPath, HashAlgorithm hashReceiver) {
			if (directoryPath == null) {
				throw new ArgumentNullException ("directoryPath");
			}
			if (directoryPath.Length == 0) {
				throw new ArgumentException ("Directory path is empty.", "directoryPath");
			}
			if (directoryPath.IndexOfAny (Path.GetInvalidPathChars ()) >= 0) {
				throw new ArgumentException ("Directory path contains invalid path characters.", "directoryPath");
			}
			if (hashReceiver == null) {
				throw new ArgumentNullException ("hashReceiver");
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> ();
			RecursivelyMoveThroughDirectory (directoryPath, true, true, dictionary, delegate(string path, string relativePath, bool file, Dictionary<string, string> dic) {
				if (file) {
					if (!relativePath.StartsWith (".")) {
						dic.Add (relativePath, GetFileHash (path, hashReceiver));
					}
				}
				return (true);
			});
			//
			return (dictionary);
		}
		
		/// <summary>
		/// Get base64 string of hash calculated for specific file using specified <see cref="HashAlgorithm"/>. 
		/// </summary>
		/// <param name="filePath">
		/// A <see cref="System.String"/> which defines path to existing file.
		/// </param>
		/// <param name="hashReceiver">
		/// A <see cref="HashAlgorithm"/> algorithm to use for hash computing
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> - a base64 string of the hash.
		/// </returns>
		public static string GetFileHash (string filePath, HashAlgorithm hashReceiver) {
			if (hashReceiver == null) {
				throw new ArgumentNullException ("hashReceiver");
			}
			using (Stream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				byte[] hash = hashReceiver.ComputeHash (stream);
				string hashBase64 = Convert.ToBase64String (hash);
				return hashBase64;
			}
		}

		/// <summary>
		/// Returns an string which represents initialization of dictionary of file hashes.
		/// </summary>
		/// <param name="variableName">Name of dictionary variable.</param>
		/// <param name="dictionary">Dictionary which should be instantiated.</param>
		/// <returns>CSharp code string.</returns>
		public static string GetCSharpHashDictionaryConstructor (string variableName, Dictionary<string, string> dictionary) {
			if (string.IsNullOrEmpty (variableName)) {
				throw new ArgumentException ("Variable name cannot be null or empty.", "variableName");
			}
			if (dictionary == null) {
				return ("Dictionary<string, string> " + variableName + " = null;");
			} else {
				if (dictionary.Count == 0) {
					return ("Dictionary<string, string> " + variableName + " = new Dictionary<string, string>();");
				} else {
					StringBuilder begin = new StringBuilder ("Dictionary<string, string> " + variableName + " = new Dictionary<string, string>();" + Environment.NewLine);
					foreach (KeyValuePair<string, string> pair in dictionary) {
						begin.Append (variableName + ".Add(");
						begin.Append (convertDirectorySeparator (pair.Key));
						begin.Append ("\", ");
						begin.Append ("@\"");
						begin.Append (pair.Value);
						begin.Append ("\");");
						begin.Append (Environment.NewLine);
					}
					return (begin.ToString ());
				}
			}
		}
		
		/// <summary>
		/// This method can be used to create zip file with libvlc libraries and plugins. 
		/// </summary>
		/// <param name="directoryPath">
		/// Path to existing directory which will be zipped.
		/// </param>
		/// <param name="packageFilePath">
		/// Path to zip file to create.
		/// </param>
		public static void CreateDeploymentPackage(string directoryPath, string packageFilePath) {
			if (directoryPath == null) {
				throw new ArgumentNullException("directoryPath");
			}
			if (directoryPath == string.Empty) {
				throw new ArgumentException("Directory path is empty string.");
			}
			if (directoryPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0) {
				throw new ArgumentException("Directory path contains invalid characters.");
			}
			if (!Directory.Exists(directoryPath)) {
				throw new ArgumentException("Directory does not exists.");
			}
			FastZip zip = new FastZip();
			zip.CreateZip(packageFilePath, directoryPath, true, "^[^.].*", "^[^.].*");
		}

		private static string convertDirectorySeparator (string path) {
			string separator = Path.DirectorySeparatorChar.ToString ();
			if (path.StartsWith (separator)) {
				path = "Path.DirectorySeparatorChar + \"" + path.Substring (separator.Length);
			} else {
				path = "\"" + path;
			}
			path = path.Replace (separator, "\" + Path.DirectorySeparatorChar + \"");
			if (path.EndsWith ("\"\"")) {
				path = path.Remove (path.Length - 2);
			}
			return path;
		}

		private static void RecursivelyMoveThroughDirectory<TDataType> (string directoryPath, bool moveSubDirs, bool moveFiles, TDataType callbackData, RecursiveDirectoryMoveHandler<TDataType> callback) {
			DirectoryInfo info = new DirectoryInfo (directoryPath);
			if (info.Exists) {
				if (moveFiles) {
					FileInfo[] files = info.GetFiles ();
					foreach (FileInfo file in files) {
						if (!callback (file.FullName, file.Name, true, callbackData)) {
							return;
						}
					}
				}
				if (moveSubDirs) {
					DirectoryInfo[] directories = info.GetDirectories ();
					foreach (DirectoryInfo directory in directories) {
						if (!callback (directory.FullName, directory.Name, false, callbackData)) {
							return;
						}
					}
					directories = info.GetDirectories ();
					foreach (DirectoryInfo directory in directories) {
						RecursivelyMoveThroughDirectoryInternal (string.Empty, directory, true, moveFiles, callbackData, callback);
					}
				}
			}
		}

		private static void RecursivelyMoveThroughDirectoryInternal<TDataType> (string parentName, DirectoryInfo current, bool moveSubDirs, bool moveFiles, TDataType callbackData, RecursiveDirectoryMoveHandler<TDataType> callback) {
			//
			DirectoryInfo info = current;
			if (info.Exists) {
				string currentDir = parentName + Path.DirectorySeparatorChar + info.Name + Path.DirectorySeparatorChar;
				if (moveFiles) {
					FileInfo[] files = info.GetFiles ();
					foreach (FileInfo file in files) {
						if (!callback (file.FullName, currentDir + file.Name, true, callbackData)) {
							return;
						}
					}
				}
				if (moveSubDirs) {
					DirectoryInfo[] directories = info.GetDirectories ();
					foreach (DirectoryInfo directory in directories) {
						if (!callback (directory.FullName, currentDir + directory.Name, false, callbackData)) {
							return;
						}
					}
					directories = info.GetDirectories ();
					foreach (DirectoryInfo directory in directories) {
						RecursivelyMoveThroughDirectoryInternal (parentName + Path.DirectorySeparatorChar + info.Name, directory, true, moveFiles, callbackData, callback);
					}
				}
			}
		}

		#region Nested type: DeterminedOSType
		
		/// <summary>
		/// Defines OS type determined by deployment module. 
		/// </summary>
		public enum DeterminedOSType {
			/// <summary>
			/// OS type isn't determined yet and it's currently unknown. 
			/// </summary>
			Unknown,
			/// <summary>
			/// OS type is Microsoft Windows. 
			/// </summary>
			Windows,
			/// <summary>
			/// OS type is Mac OS. 
			/// </summary>
			MacOS
		}

		#endregion

		#region Nested type: RecursiveDirectoryMoveHandler

		private delegate bool RecursiveDirectoryMoveHandler<TDataType> (string fullPath, string relativePath, bool isFile, TDataType dataType);
		
		#endregion
	}
}
