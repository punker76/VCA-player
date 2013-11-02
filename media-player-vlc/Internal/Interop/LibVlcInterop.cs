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
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Security;
using System.Text;
using DZ.MediaPlayer.Vlc.Exceptions;
using Common.Logging;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.Interop {
    /// <summary>
    /// Thunks to original library.
    /// </summary>
    internal static class LibVlcInterop {
        #region Diagnostics

        /// <summary>
        /// Set true to cause a trace message to be generated for each libVLC call
        /// </summary>
        internal static bool callTraceEnabled = true;
		private static readonly ILog logger = LogManager.GetLogger(typeof(LibVlcInterop));
		
        private static void traceCall(string msg) {
			if (logger.IsTraceEnabled) {
            	logger.Trace("native call: " + msg);
			}
        }

        #endregion

        #region Volume

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_audio_get_volume")]
        [SuppressUnmanagedCodeSecurity]
        private static extern int libvlc_audio_get_volume_unmanaged(IntPtr libvlc_media_player_t);

        public static int libvlc_audio_get_volume(IntPtr libvlc_media_player_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_audio_get_volume");
            }
            return libvlc_audio_get_volume_unmanaged(libvlc_media_player_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_audio_set_volume")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 libvlc_audio_set_volume_unmanaged(IntPtr libvlc_media_player_t, Int32 volume);

        public static Int32 libvlc_audio_set_volume(IntPtr libvlc_media_player_t, Int32 volume) {
            if (callTraceEnabled) {
                traceCall("libvlc_audio_set_volume");
            }
            return libvlc_audio_set_volume_unmanaged(libvlc_media_player_t, volume);
        }

        #endregion

        #region Errors

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_errmsg")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_errmsg_unmanaged();

        /// <summary>
        /// Return a human-readable error message for the last LibVLC error in the calling thread.
        /// </summary>
        /// <returns>A human-readable error message for the last LibVLC error in the calling thread. 
        /// Or <code>null</code> if there is no any error. 
        /// </returns>
        public static string libvlc_errmsg() {
            if (callTraceEnabled) {
                traceCall("libvlc_errmsg");
            }
            IntPtr result = libvlc_errmsg_unmanaged();
			if ( result != IntPtr.Zero ) {
				int size = 0;
				int max = 1024 * 4;
				byte[] message = null;
	            while (size < max) {
					byte b = Marshal.ReadByte(result, size);
					if ( b == 0x0 ) {
						message = new byte[size];
						break;
					}
					size++;
				}
				if ( message != null ) {
					Marshal.Copy(result, message, 0, size);
					return Encoding.UTF8.GetString(message);
				}
				throw new VlcInternalException("Message exceeds maximum limit, probably libvlc_errmsg returned bad pointer.");
			} else {
				return null;
			}
		}

        /// <summary>
        /// Clears the LibVLC error status for the current thread.
        /// </summary>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_clearerr")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_clearerr_unmanaged();

        public static void libvlc_clearerr() {
            if (callTraceEnabled) {
                traceCall("libvlc_clearerr");
            }
            libvlc_clearerr_unmanaged();
        }

        #endregion

        #region Core objects

        /// <summary>
        /// Find a named object and increment its refcount. Don't forget to call __vlc_object_release()
        /// after using found object.
        /// </summary>
        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "vlc_object_find_name")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr vlc_object_find_name_unmanaged(IntPtr vlc_object_t, IntPtr name, int flags);

        public static IntPtr vlc_object_find_name(IntPtr vlc_object_t, string name, int flags) {
            if (callTraceEnabled) {
                traceCall("vlc_object_find_name");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                return vlc_object_find_name_unmanaged(vlc_object_t, namePtr, flags);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        /// <summary>
        /// Find a typed object and increment its refcount. Don't forget to call __vlc_object_release()
        /// after using found object.
        /// </summary>
        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "vlc_object_find")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr vlc_object_find_unmanaged(IntPtr vlc_object_t, int i_type, int i_mode);

        public static IntPtr vlc_object_find(IntPtr vlc_object_t, int i_type, int i_mode) {
            if (callTraceEnabled) {
                traceCall("vlc_object_find");
            }
            return vlc_object_find_unmanaged(vlc_object_t, i_type, i_mode);
        }

        /// <summary>
        /// Decrement an object refcount
        /// * And destroy the object if its refcount reach zero.
        /// </summary>
        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "vlc_object_release")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void vlc_object_release_unmanaged(IntPtr vlc_object_t);

        public static void vlc_object_release(IntPtr vlc_object_t) {
            if (callTraceEnabled) {
                traceCall("vlc_object_release");
            }
            vlc_object_release_unmanaged(vlc_object_t);
        }

        /// <summary>
        /// Gets the list of children of an objects, and increment their reference count.
        /// </summary>
        /// <param name="p_object">Parent object</param>
        /// <returns>A list (possibly empty) or NULL in case of error.</returns>
        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "vlc_list_children")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr vlc_list_children_unmanaged(IntPtr p_object);

        public static libvlc_list_t vlc_list_children(IntPtr p_object) {
            if (callTraceEnabled) {
                traceCall("vlc_list_children");
            }
            IntPtr ptrRet = vlc_list_children_unmanaged(p_object);
            return (libvlc_list_t)(Marshal.PtrToStructure(ptrRet, typeof(libvlc_list_t)));
        }

        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "var_Set")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void var_Set_unmanaged(IntPtr p_object, IntPtr psz_name, vlc_value_t value);

        public static void var_Set(IntPtr p_object, string name, vlc_value_t value) {
            if (callTraceEnabled) {
                traceCall("var_Set");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                var_Set_unmanaged(p_object, namePtr, value);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        [DllImport("libvlccore", ExactSpelling = true, EntryPoint = "var_Get")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void var_Get_unmanaged(IntPtr p_object, IntPtr psz_name, ref vlc_value_t value);

        public static void var_Get(IntPtr p_object, string name, ref vlc_value_t value) {
            if (callTraceEnabled) {
                traceCall("var_Get");
            }
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
            try {
                var_Get_unmanaged(p_object, namePtr, ref value);
            } finally {
                Marshal.FreeHGlobal(namePtr);
            }
        }

        #endregion

        #region Core

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_new")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new_unmanaged(int argc, IntPtr argv);

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_new")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new_unmanaged(int argc, [MarshalAs(UnmanagedType.LPArray)] String[] argv);
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_new")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_new_unmanaged(int argc, IntPtr[] argv);
		
		/*
		public static IntPtr libvlc_new2(string[] parameters) {
            //
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null) {
                throw new InvalidOperationException("Can't get entry assembly. TODO.");
            }
            string assemblyLocation = assembly.Location;
            //
            Encoding encoding = Encoding.UTF8;
            int locationOffset = encoding.GetByteCount(assemblyLocation + "\0");
            int bufferSize = locationOffset;
            IntPtr ptrAllStrings = IntPtr.Zero;
            //
            try {
                IntPtr[] arrayOfPointers = null;
                //
                if (parameters != null && parameters.Length > 0) {
                    arrayOfPointers = new IntPtr[parameters.Length + 1];
                    //
                    for (int i = 0; i < parameters.Length; i++) {
                        bufferSize += encoding.GetByteCount(parameters[i] + "\0");
                    }
                    //
                    ptrAllStrings = Marshal.AllocHGlobal(bufferSize);
                    //
                    string zeroTerminated = assemblyLocation + "\0";
                    byte[] zeroTerminatedArray = encoding.GetBytes(zeroTerminated);
                    //
                    Marshal.Copy(zeroTerminatedArray, 0, ptrAllStrings, zeroTerminatedArray.Length);
                    arrayOfPointers[0] = ptrAllStrings;
                    IntPtr ptr = new IntPtr(ptrAllStrings.ToInt64() + locationOffset);
                    //
                    for (int i = 0; i < parameters.Length; i++) {
                        zeroTerminated = parameters[i] + "\0";
                        zeroTerminatedArray = encoding.GetBytes(zeroTerminated);
                        //
                        Marshal.Copy(zeroTerminatedArray, 0, ptr, zeroTerminatedArray.Length);
                        arrayOfPointers[i + 1] = ptr;
                        //
                        int offset = encoding.GetByteCount(zeroTerminated);
                        ptr = new IntPtr(ptr.ToInt64() + offset);
                    }
                    //
                } else {
                    arrayOfPointers = new IntPtr[1];
                    ptrAllStrings = Marshal.AllocHGlobal(bufferSize);
                    //
                    string zeroTerminated = assemblyLocation + "\0";
                    byte[] zeroTerminatedArray = encoding.GetBytes(zeroTerminated);
                    //
                    Marshal.Copy(zeroTerminatedArray, 0, ptrAllStrings, zeroTerminatedArray.Length);
                    arrayOfPointers[0] = ptrAllStrings;
                }
                IntPtr argvPtr = IntPtr.Zero;

                //
                IntPtr descriptor = libvlc_new_unmanaged(arrayOfPointers.Length - 1, arrayOfPointers);
                if (descriptor == IntPtr.Zero) {
                    throw new VlcInternalException("Cannot create instance of libvlc.");
                }
                return descriptor;
            } finally {
                if (ptrAllStrings != IntPtr.Zero) {
                    Marshal.FreeHGlobal(ptrAllStrings);
                }
            }
        }*/

        public static IntPtr libvlc_new(string[] parameters) {
            if (callTraceEnabled) {
                traceCall("libvlc_new");
            }
            //
            // Check the libvlc version before continuing
            string libvlcVersion = libvlc_get_version();
#if DEBUG
            string expectedPrefix = "1.1.";
            System.Diagnostics.Debug.Assert(libvlcVersion.StartsWith(expectedPrefix),
                String.Format("libvlc {0}x is required, but \"{1}\" is present", expectedPrefix, libvlcVersion));
#endif
            System.Diagnostics.Trace.WriteLine(String.Format("using libvlc version \"{0}\"", libvlcVersion));
            //
            IntPtr descriptor = libvlc_new_unmanaged(parameters.Length, parameters);
            if (IntPtr.Zero == descriptor) {
				string message = LibVlcInterop.libvlc_errmsg();
				if ( string.IsNullOrEmpty(message) ) {
					message = "Cannot create instance of libvlc.";
				}
                throw new VlcInternalException(message);
            }
            return descriptor;
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_get_version")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_get_version_unmanaged();

        public static String libvlc_get_version() {
            if (callTraceEnabled) {
                traceCall("libvlc_get_version");
            }
            IntPtr version = libvlc_get_version_unmanaged();
            return Marshal.PtrToStringAnsi(version);
        }

        /// <summary>
        /// Decrement the reference count of a libvlc instance, and destroy it if it reaches zero
        /// </summary>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_release")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_release_unmanaged(IntPtr libvlc_instance_t);

        public static void libvlc_release(IntPtr libvlc_instance_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_release");
            }
            libvlc_release_unmanaged(libvlc_instance_t);
        }

        #endregion

        #region Events

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VlcEventHandlerDelegate(IntPtr libvlc_event, IntPtr userData);

        #endregion

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_event_attach")]
        [SuppressUnmanagedCodeSecurity]
        private static extern int libvlc_event_attach_unmanaged(IntPtr p_event_manager, libvlc_event_type_e i_event_type, IntPtr f_callback, IntPtr user_data);

        public static int libvlc_event_attach(IntPtr p_event_manager, libvlc_event_type_e i_event_type, IntPtr f_callback, IntPtr user_data) {
            if (callTraceEnabled) {
                traceCall("libvlc_event_attach");
            }
            return libvlc_event_attach_unmanaged(p_event_manager, i_event_type, f_callback, user_data);
        }
        
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_event_detach")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_event_detach_unmanaged(IntPtr p_event_manager, libvlc_event_type_e i_event_type, IntPtr f_callback, IntPtr user_data);

        public static void libvlc_event_detach(IntPtr p_event_manager, libvlc_event_type_e i_event_type, IntPtr f_callback, IntPtr user_data) {
            if (callTraceEnabled) {
                traceCall("libvlc_event_detach");
            }
            libvlc_event_detach_unmanaged(p_event_manager, i_event_type, f_callback, user_data);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_event_type_name")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_event_type_name_unmanaged(libvlc_event_type_e event_type);

        public static string libvlc_event_type_name(libvlc_event_type_e event_type) {
            if (callTraceEnabled) {
                traceCall("libvlc_event_type_name");
            }
            IntPtr strPtr = libvlc_event_type_name_unmanaged(event_type);
            return Marshal.PtrToStringAnsi(strPtr);
        }

        #endregion

        #region Media

        /// <summary>
        /// Create a media with the given MRL
        /// </summary>
        /// <param name="p_instance">libvlc instance </param>
        /// <param name="psz_mrl">MRL</param>
        /// <returns>Media descriptor</returns>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_new_path")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_new_path_unmanaged(IntPtr p_instance, IntPtr psz_mrl);

        /// <summary>
        /// Create a media with the given MRL
        /// Helper to the original function, <paramref name="mrl"/> string will be
        /// converted to the UTF-8 encoding automatically and marshalled to the libvlc.
        /// </summary>
        /// <param name="p_instance">libvlc instance </param>
        /// <param name="mrl">MRL</param>
        public static IntPtr libvlc_media_new_path(IntPtr p_instance, string mrl) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_new_path");
            }
            IntPtr pMrl = IntPtr.Zero;
            try {
                byte[] bytes = Encoding.UTF8.GetBytes(mrl);
                //
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                //
                return (libvlc_media_new_path_unmanaged(p_instance, pMrl));
            } finally {
                if (pMrl != IntPtr.Zero) {
                    Marshal.FreeHGlobal(pMrl);
                }
            }
        }
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_parse")]
        [SuppressUnmanagedCodeSecurity]
		private static extern void libvlc_media_parse_unmanaged(IntPtr media);
		
		/// <summary>
		/// Parse a media.
		/// </summary>
		/// <param name="media">
		/// A <see cref="IntPtr"/> pointer to libvlc_media_t.
		/// </param>
		public static void libvlc_media_parse(IntPtr media) {
			if (callTraceEnabled) {
                traceCall("libvlc_media_parse");
            }
			libvlc_media_parse_unmanaged(media);
		}
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_parse_async")]
        [SuppressUnmanagedCodeSecurity]
		private static extern void libvlc_media_parse_async_unmanaged(IntPtr media);
		
		/// <summary>
		/// Parse a media asynchronously.
		/// </summary>
		/// <param name="media">
		/// A <see cref="IntPtr"/> pointer to libvlc_media_t.
		/// </param>
		public static void libvlc_media_parse_async(IntPtr media) {
			if (callTraceEnabled) {
                traceCall("libvlc_media_parse_async");
            }
			libvlc_media_parse_unmanaged(media);
		}
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_is_parsed")]
        [SuppressUnmanagedCodeSecurity]
		private static extern int libvlc_media_is_parsed_unmanaged(IntPtr media);
		
		/// <summary>
		/// Get parsed status for media descriptor object.
		/// </summary>
		/// <param name="media">
		/// A <see cref="IntPtr"/> pointer to libvlc_media_t.
		/// </param>
		/// <returns>
		/// A <see cref="bool"/> value will be <code>true</code> if media 
		/// is parsed and <code>false</code> if not.
		/// </returns>
		public static bool libvlc_media_is_parsed(IntPtr media) {
			if (callTraceEnabled) {
                traceCall("libvlc_media_is_parsed");
            }
			return libvlc_media_is_parsed_unmanaged(media) != 0;
		}
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_get_duration")]
        [SuppressUnmanagedCodeSecurity]
		private static extern Int64 libvlc_media_get_duration_unmanaged(IntPtr media);
		
		public static Int64 libvlc_media_get_duration(IntPtr media) {
			if (callTraceEnabled) {
                traceCall("libvlc_media_is_parsed");
            }
			return libvlc_media_get_duration_unmanaged(media);
		}
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_get_tracks_info")]
        [SuppressUnmanagedCodeSecurity]
		private static extern int libvlc_media_get_tracks_info_unmanaged(IntPtr media, IntPtr tracks);
		
		[DllImport("libvlc", ExactSpelling = true, EntryPoint = "free")]
        [SuppressUnmanagedCodeSecurity]
		public static extern int libvlc_free(IntPtr ptr);
		
		public static string vlc_fourcc_GetDescription(int i_cat, uint i_fourcc) {
			if (callTraceEnabled) {
                traceCall("vlc_fourcc_GetDescription");
            }
            IntPtr strPtr = vlc_fourcc_GetDescription_unmanaged(i_cat, i_fourcc);
            return Marshal.PtrToStringAnsi(strPtr);
		}
		
		[DllImport("libvlccore", ExactSpelling = true, EntryPoint = "vlc_fourcc_GetDescription")]
        [SuppressUnmanagedCodeSecurity]
		private static extern IntPtr vlc_fourcc_GetDescription_unmanaged(int i_cat, uint i_fourcc);

		public static libvlc_media_track_info_t[] libvlc_media_get_tracks_info(IntPtr media) {
			if (callTraceEnabled) {
                traceCall("libvlc_media_get_tracks_info");
            }
			//
			int size = Marshal.SizeOf(typeof(libvlc_media_track_info_t));
			//
			int pointerSize = IntPtr.Size;
			IntPtr pointer = Marshal.AllocHGlobal(pointerSize);
			//
			for (int i = 0; i < pointerSize; i++ ) 
				Marshal.WriteByte(new IntPtr(pointer.ToInt64() + i), 0x0);
			//
			try {
				//
				int count = libvlc_media_get_tracks_info_unmanaged(media, pointer);
				//
				if (logger.IsDebugEnabled) {
					logger.Debug(string.Format(CultureInfo.InvariantCulture, 
						"There are at least {0} tracks in the media. Retrieving...", count));
				}
				if ( count > 0 ) {
					List<libvlc_media_track_info_t> list = new List<libvlc_media_track_info_t>(count);
					//
					IntPtr arrPtr = Marshal.ReadIntPtr(pointer);
					if ( arrPtr != IntPtr.Zero ) {
						try {
							for ( int i = 0; i < count; i++ ) {
								//
								libvlc_media_track_info_t structure = new libvlc_media_track_info_t();
								Marshal.PtrToStructure(arrPtr, structure);
								list.Add(structure);
								//
								arrPtr = new IntPtr(arrPtr.ToInt64() + size);
							}
							return list.ToArray();
						} finally {
							// TODO: libvlc tells to free memory. but how?
							try {
								//LibVlcInterop.libvlc_free(arrPtr);
							} catch(Exception exc) {
								if (logger.IsErrorEnabled) {
									logger.Error("Can't free memory allocated inside libvlc.", exc);
								}
							}
						}
					}
				}
				//
			} catch(Exception exc) {
				if (logger.IsErrorEnabled) {
					logger.Error("Can't get media tracks info.", exc);
				}
				throw;
			} finally {
				Marshal.FreeHGlobal(pointer);
			}
			return new libvlc_media_track_info_t[] {};
		}
		
		
        /// <summary>
        /// Add an option to the media
        /// </summary>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_add_option")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_add_option_unmanaged(IntPtr libvlc_media_inst, IntPtr ppsz_options);

        /// <summary>
        /// Adds an options in MRL's option format
        /// </summary>
        /// <param name="libvlc_media_inst">Media instance</param>
        /// <param name="options">Options string ("::sout=#transcode{vcodec=DIV3,vb=1024,scale=1}:duplicate{dst=display,dst=std{access=file,mux=ps,dst=\"C:\\temp.mpeg\"}}" for example)</param>
        public static void libvlc_media_add_option(IntPtr libvlc_media_inst, string options) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_add_option");
            }
            IntPtr pOptions = IntPtr.Zero;
            try {
                pOptions = Marshal.StringToHGlobalAnsi(options);

                libvlc_media_add_option_unmanaged(libvlc_media_inst, pOptions);
            } finally {
                if (pOptions != IntPtr.Zero) {
                    Marshal.FreeHGlobal(pOptions);
                }
            }
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_get_mrl")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_get_mrl_unmanaged(IntPtr libvlc_media_inst);

        public static string libvlc_media_get_mrl(IntPtr libvlc_media_inst) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_get_mrl");
            }
            IntPtr result = libvlc_media_get_mrl_unmanaged(libvlc_media_inst);
            if (result == IntPtr.Zero) {
                throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
            }
            return Marshal.PtrToStringAnsi(result);
        }

        /// <summary>
        /// Decrement the reference count of a media descriptor object
        /// </summary>
        /// <param name="libvlc_media_inst">Media descriptor</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_release")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_release_unmanaged(IntPtr libvlc_media_inst);

        public static void libvlc_media_release(IntPtr libvlc_media_inst) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_release");
            }
            libvlc_media_release_unmanaged(libvlc_media_inst);
        }

        /// <summary>
        /// Increment the reference count of a media descriptor object
        /// </summary>
        /// <param name="libvlc_media_inst">Media descriptor</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_retain")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_retain_unmanaged(IntPtr libvlc_media_inst);

        public static IntPtr libvlc_media_retain(IntPtr libvlc_media_inst) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_retain");
            }
            return libvlc_media_retain_unmanaged(libvlc_media_inst);
        }

        /// <summary>
        /// Get current state of media descriptor object
        /// </summary>
        /// <param name="libvlc_media_inst">Media descriptor</param>
        /// <returns>libvlc_state_t enum</returns>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_get_state")]
        [SuppressUnmanagedCodeSecurity]
        private static extern libvlc_state_t libvlc_media_get_state_unmanaged(IntPtr libvlc_media_inst);

        public static libvlc_state_t libvlc_media_get_state(IntPtr libvlc_media_inst) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_get_state");
            }
            return libvlc_media_get_state_unmanaged(libvlc_media_inst);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_event_manager")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_event_manager_unmanaged(IntPtr libvlc_media_t);

        public static IntPtr libvlc_media_event_manager(IntPtr libvlc_media_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_event_manager");
            }
            return libvlc_media_event_manager_unmanaged(libvlc_media_t);
        }

        #endregion

        #region Mediaplayer

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_new")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_player_new_unmanaged(IntPtr libvlc_instance);

        /// <summary>
        /// Create an empty Media Player object
        /// </summary>
        /// <param name="libvlc_instance">Pointer to instance</param>
        /// <returns>Media player descriptor</returns>
        public static IntPtr libvlc_media_player_new(IntPtr libvlc_instance) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_new");
            }
            return libvlc_media_player_new_unmanaged(libvlc_instance);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_new_from_media")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_player_new_from_media_unmanaged(IntPtr libvlc_media);

        /// <summary>
        /// Create a Media Player object from a Media
        /// </summary>
        /// <param name="libvlc_media">Media descriptor</param>
        /// <returns>Media player descriptor</returns>
        public static IntPtr libvlc_media_player_new_from_media(IntPtr libvlc_media) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_new_from_media");
            }
            return libvlc_media_player_new_from_media_unmanaged(libvlc_media);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_get_media")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_player_get_media_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get the media used by the media_player
        /// </summary>
        /// <param name="libvlc_mediaplayer">Mediaplayer descriptor</param>
        /// <returns>Media descriptor</returns>
        public static IntPtr libvlc_media_player_get_media(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_get_media");
            }
            return libvlc_media_player_get_media_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_media")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_media_unmanaged(IntPtr libvlc_media_player_t, IntPtr libvlc_media_t);

        public static void libvlc_media_player_set_media(IntPtr libvlc_media_player_t, IntPtr libvlc_media_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_media");
            }
            libvlc_media_player_set_media_unmanaged(libvlc_media_player_t, libvlc_media_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_play")]
        [SuppressUnmanagedCodeSecurity]
        private static extern int libvlc_media_player_play_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Play
        /// </summary>
        public static int libvlc_media_player_play(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_play");
            }
            return libvlc_media_player_play_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_pause")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_pause_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Pause
        /// </summary>
        public static void libvlc_media_player_pause(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_pause");
            }
            libvlc_media_player_pause_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_stop")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_stop_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Stop
        /// </summary>
        public static void libvlc_media_player_stop(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_stop");
            }
            libvlc_media_player_stop_unmanaged(libvlc_mediaplayer);
        }

        /// <summary>
        /// Set the drawable where the media player should render its video output
        /// </summary>
        /// <param name="libvlc_mediaplayer">The Media Player</param>
        /// <param name="libvlc_drawable">The libvlc_drawable_t where the media player should render its video</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_hwnd")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_hwnd_unmanaged(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable);

        public static void libvlc_media_player_set_hwnd(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_hwnd");
            }
            libvlc_media_player_set_hwnd_unmanaged(libvlc_mediaplayer, libvlc_drawable);
        }
		
		/// <summary>
        /// Set the drawable where the media player should render its video output
        /// </summary>
        /// <param name="libvlc_mediaplayer">The Media Player</param>
        /// <param name="libvlc_drawable">The libvlc_drawable_t where the media player should render its video</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_xwindow")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_xwindow_unmanaged(IntPtr libvlc_mediaplayer, Int32 libvlc_drawable);

        public static void libvlc_media_player_set_xwindow(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_xwindow");
            }
            libvlc_media_player_set_xwindow_unmanaged(libvlc_mediaplayer, libvlc_drawable.ToInt32());
        }
		
		/// <summary>
        /// Set the drawable where the media player should render its video output
        /// </summary>
        /// <param name="libvlc_mediaplayer">The Media Player</param>
        /// <param name="libvlc_drawable">The libvlc_drawable_t where the media player should render its video</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_agl")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_agl_unmanaged(IntPtr libvlc_mediaplayer, Int32 libvlc_drawable);

        public static void libvlc_media_player_set_agl(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_agl");
            }
            libvlc_media_player_set_agl_unmanaged(libvlc_mediaplayer, libvlc_drawable.ToInt32());
        }
		
		/// <summary>
        /// Set the drawable where the media player should render its video output
        /// </summary>
        /// <param name="libvlc_mediaplayer">The Media Player</param>
        /// <param name="libvlc_drawable">The libvlc_drawable_t where the media player should render its video</param>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_nsobject")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_nsobject_unmanaged(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable);

        public static void libvlc_media_player_set_nsobject(IntPtr libvlc_mediaplayer, IntPtr libvlc_drawable) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_nsobject");
            }
            libvlc_media_player_set_nsobject_unmanaged(libvlc_mediaplayer, libvlc_drawable);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_get_length")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int64 libvlc_media_player_get_length_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get the current movie length (in ms)
        /// </summary>
        public static Int64 libvlc_media_player_get_length(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_get_length");
            }
            return libvlc_media_player_get_length_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_get_time")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int64 libvlc_media_player_get_time_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get the current movie time (in ms)
        /// </summary>
        public static Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_get_time");
            }
            return libvlc_media_player_get_time_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_time")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_time_unmanaged(IntPtr libvlc_mediaplayer, Int64 time);

        /// <summary>
        /// Set the movie time (in ms)
        /// </summary>
        public static void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_time");
            }
            libvlc_media_player_set_time_unmanaged(libvlc_mediaplayer, time);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_get_position")]
        [SuppressUnmanagedCodeSecurity]
        private static extern float libvlc_media_player_get_position_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get current position (float part)
        /// </summary>
        public static float libvlc_media_player_get_position(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_get_position");
            }
            return libvlc_media_player_get_position_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_set_position")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_set_position_unmanaged(IntPtr libvlc_mediaplayer, float position);

        /// <summary>
        /// Set current position (float part)
        /// </summary>
        public static void libvlc_media_player_set_position(IntPtr libvlc_mediaplayer, float position) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_set_position");
            }
            libvlc_media_player_set_position_unmanaged(libvlc_mediaplayer, position);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_release")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_media_player_release_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Release a media_player after use Decrement the reference count of a media player object
        /// </summary>
        public static void libvlc_media_player_release(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_release");
            }
            libvlc_media_player_release_unmanaged(libvlc_mediaplayer);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_event_manager")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_media_player_event_manager_unmanaged(IntPtr libvlc_media_player_t);

        /// <summary>
        /// Get the Event MediaLibraryFactory from which the media player send event
        /// </summary>
        public static IntPtr libvlc_media_player_event_manager(IntPtr libvlc_media_player_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_event_manager");
            }
            return libvlc_media_player_event_manager_unmanaged(libvlc_media_player_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_media_player_get_fps")]
        [SuppressUnmanagedCodeSecurity]
        private static extern float libvlc_media_player_get_fps_unmanaged(IntPtr libvlc_media_player_t);

        public static float libvlc_media_player_get_fps(IntPtr libvlc_media_player_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_media_player_get_fps");
            }
            return libvlc_media_player_get_fps_unmanaged(libvlc_media_player_t);
        }

        #endregion

        #region Core->MediaPlayer->Video

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_video_get_spu")]
        [SuppressUnmanagedCodeSecurity]
        private static extern int libvlc_video_get_spu_unmanaged(IntPtr libvlc_mediaplayer);

        /// <summary>
        /// Get current video subtitle
        /// </summary>
        public static int libvlc_video_get_spu(IntPtr libvlc_mediaplayer) {
            if (callTraceEnabled) {
                traceCall("libvlc_video_get_spu");
            }
            return libvlc_video_get_spu_unmanaged(libvlc_mediaplayer);
        }

        /// <summary>
        /// Set new video subtitle
        /// </summary>
        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_video_set_spu")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 libvlc_video_set_spu_unmanaged(IntPtr libvlc_mediaplayer, int spu);

        public static Int32 libvlc_video_set_spu(IntPtr libvlc_mediaplayer, int spu) {
            if (callTraceEnabled) {
                traceCall("libvlc_video_set_spu");
            }
            return libvlc_video_set_spu_unmanaged(libvlc_mediaplayer, spu);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_video_take_snapshot")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 libvlc_video_take_snapshot_unmanaged(
            IntPtr libvlc_media_player_t, UInt32 num, IntPtr filePath, UInt32 i_width, UInt32 i_height);

        /// <summary>
        /// Take a snapshot of the current video window
        /// If i_width AND i_height is 0, original size is used. If i_width XOR i_height is 0, original aspect-ratio is preserved
        /// </summary>
        public static Int32 libvlc_video_take_snapshot(
                IntPtr libvlc_media_player_t, UInt32 num, IntPtr filePath, UInt32 i_width, UInt32 i_height)
        {
            if (callTraceEnabled) {
                traceCall("libvlc_video_take_snapshot");
            }
            return libvlc_video_take_snapshot_unmanaged(libvlc_media_player_t, num, filePath, i_width, i_height);
        }

        #endregion

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_set_fullscreen")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_set_fullscreen_unmanaged(IntPtr libvlc_media_player_t, int b_fullscreen);

        /// <summary>
        /// Enable or disable fullscreen on a video output.
        /// </summary>
        public static void libvlc_set_fullscreen(IntPtr libvlc_media_player_t, int b_fullscreen) {
            if (callTraceEnabled) {
                traceCall("libvlc_set_fullscreen");
            }
            libvlc_set_fullscreen_unmanaged(libvlc_media_player_t, b_fullscreen);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_get_fullscreen")]
        [SuppressUnmanagedCodeSecurity]
        private static extern int libvlc_get_fullscreen_unmanaged(IntPtr libvlc_media_player_t);

        /// <summary>
        /// Enable or disable fullscreen on a video output.
        /// </summary>
        public static int libvlc_get_fullscreen(IntPtr libvlc_media_player_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_get_fullscreen");
            }
            return libvlc_get_fullscreen_unmanaged(libvlc_media_player_t);
        }

        #region Logging

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_get_log_verbosity")]
        [SuppressUnmanagedCodeSecurity]
        private static extern UInt32 libvlc_get_log_verbosity_unmanaged(IntPtr libvlc_instance_t);

        public static UInt32 libvlc_get_log_verbosity(IntPtr libvlc_instance_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_get_log_verbosity");
            }
            return libvlc_get_log_verbosity_unmanaged(libvlc_instance_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_set_log_verbosity")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_set_log_verbosity_unmanaged(IntPtr libvlc_instance_t, UInt32 level);

        public static void libvlc_set_log_verbosity(IntPtr libvlc_instance_t, UInt32 level) {
            if (callTraceEnabled) {
                traceCall("libvlc_set_log_verbosity");
            }
            libvlc_set_log_verbosity_unmanaged(libvlc_instance_t, level);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_open")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_log_open_unmanaged(IntPtr libvlc_instance_t);

        public static IntPtr libvlc_log_open(IntPtr libvlc_instance_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_open");
            }
            return libvlc_log_open_unmanaged(libvlc_instance_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_close")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_log_close_unmanaged(IntPtr libvlc_log_t);

        public static void libvlc_log_close(IntPtr libvlc_log_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_close");
            }
            libvlc_log_close_unmanaged(libvlc_log_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_count")]
        [SuppressUnmanagedCodeSecurity]
        private static extern UInt32 libvlc_log_count_unmanaged(IntPtr libvlc_log_t);

        public static UInt32 libvlc_log_count(IntPtr libvlc_log_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_count");
            }
            return libvlc_log_count_unmanaged(libvlc_log_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_clear")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_log_clear_unmanaged(IntPtr libvlc_log_t);

        public static void libvlc_log_clear(IntPtr libvlc_log_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_clear");
            }
            libvlc_log_clear_unmanaged(libvlc_log_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_get_iterator")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_log_get_iterator_unmanaged(IntPtr libvlc_log_t);

        public static IntPtr libvlc_log_get_iterator(IntPtr libvlc_log_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_get_iterator");
            }
            return libvlc_log_get_iterator_unmanaged(libvlc_log_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_iterator_free")]
        [SuppressUnmanagedCodeSecurity]
        private static extern void libvlc_log_iterator_free_unmanaged(IntPtr libvlc_log_iterator_t);

        public static void libvlc_log_iterator_free(IntPtr libvlc_log_iterator_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_iterator_free");
            }
            libvlc_log_iterator_free_unmanaged(libvlc_log_iterator_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_iterator_has_next")]
        [SuppressUnmanagedCodeSecurity]
        private static extern Int32 libvlc_log_iterator_has_next_unmanaged(IntPtr libvlc_log_iterator_t);

        public static Int32 libvlc_log_iterator_has_next(IntPtr libvlc_log_iterator_t) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_iterator_has_next");
            }
            return libvlc_log_iterator_has_next_unmanaged(libvlc_log_iterator_t);
        }

        [DllImport("libvlc", ExactSpelling = true, EntryPoint = "libvlc_log_iterator_next")]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr libvlc_log_iterator_next_unmanaged(IntPtr libvlc_log_iterator_t, ref libvlc_log_message_t p_buffer);

        public static IntPtr libvlc_log_iterator_next(IntPtr libvlc_log_iterator_t, ref libvlc_log_message_t p_buffer) {
            if (callTraceEnabled) {
                traceCall("libvlc_log_iterator_next");
            }
            return libvlc_log_iterator_next_unmanaged(libvlc_log_iterator_t, ref p_buffer);
        }

        #endregion
    }
}