/* This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.
*/

#region Usings

using System;
using System.Threading;
using Common.Logging;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal;
using DZ.MediaPlayer.Vlc.Internal.Interfaces;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;
using DZ.MediaPlayer.Vlc.Internal.Interop;
using DZ.MediaPlayer.Vlc.Io;

#endregion

namespace DZ.MediaPlayer.Vlc
{
    /// <summary>
    /// Implements <see cref="MediaLibraryFactory"/> and manages vlc specific objects internally.
    /// </summary>
    public sealed class VlcMediaLibraryFactory : MediaLibraryFactory, ILogVerbosityManager
    {
        /// <summary>
        /// Associated logger. Used primarily to log vlclib logging messages.
        /// </summary>
        private readonly ILog logger;

        /// <summary>
        /// Default name of vlc logger.
        /// </summary>
        public const string DEFAULT_LOGGER_CLASS_NAME = "VlcManagerLogger";

        /// <summary>
        /// Descriptor of libvlc instance retrieved from libvlc module.
        /// </summary>
        private readonly IntPtr descriptor;
		
		private InternalObjectsFactory internalFactory;

        /// <summary>
        /// Instantiates vlc library using specific parameters.
        /// </summary>
        /// <param name="parameters">Parameters for vlc core.</param>
        public VlcMediaLibraryFactory(string[] parameters)
            : this(parameters, DEFAULT_LOGGER_CLASS_NAME, true) {
        }

        /// <summary>
        /// Instantiates vlc library using specific parameters and logger class name.
        /// </summary>
        /// <param name="parameters">Parameters for vlc core.</param>
        /// <param name="loggerClassName">Name of logger to use for vlc logs.</param>
        public VlcMediaLibraryFactory(string[] parameters, string loggerClassName)
			: this(parameters, DEFAULT_LOGGER_CLASS_NAME, true) {
        }
		
		/// <summary>
        /// Instantiates vlc library using specific parameters and logger class name. Also allows you to
        /// customize logging thread spawning.
        /// </summary>
        /// <param name="parameters">Parameters for vlc core.</param>
        /// <param name="loggerClassName">Name of logger to use for vlc logs.</param>
        /// <param name="spawnLoggingThread">User should pass <code>false</code> here if he doesn't want to 
        /// spawn logging thread which will integrate vlc logging into Common.Logging.</param>
        public VlcMediaLibraryFactory(string[] parameters, string loggerClassName, bool spawnLoggingThread) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            if (loggerClassName == null) {
                throw new ArgumentNullException("loggerClassName");
            }
            if (loggerClassName.Length == 0) {
                throw new ArgumentException("String is empty.", "loggerClassName");
            }
			//
			Deployment.VlcDeployment.RunOSDependentTasks();
			//
            try {
                descriptor = createInstance(parameters);
				internalFactory = new InternalObjectsFactory(descriptor);
                //
                logger = LogManager.GetLogger(loggerClassName);
                vlcLog = internalFactory.CreateVlcLog(logger, this);
				if ( spawnLoggingThread ) {
                	initializeAndStartLoggingThread();
				}
            } catch (DllNotFoundException exc) {
                throw new VlcDeploymentException("Vlc library is not deployed. Use VlcDeployment class to deploy vlc libraries.", exc);
            }
        }

        private static string[] addFilterRequestToParameters(string[] parameters) {
            string[] parametersWithAddedFilter = new string[parameters.Length + 2];
            parameters.CopyTo(parametersWithAddedFilter, 0);
            parametersWithAddedFilter[parametersWithAddedFilter.Length - 2] = "--video-filter";
            parametersWithAddedFilter[parametersWithAddedFilter.Length - 1] = "adjust@elwood_adjust";
            //
            return (parametersWithAddedFilter);
        }

        private static IntPtr createInstance(string[] parameters) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }
            //
            IntPtr res = LibVlcInterop.libvlc_new(parameters);
            if (IntPtr.Zero == res) {
                throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
            }
            //
            return (res);
        }

        private VlcDoubleWindowFactory doubleWindowFactory;

        /// <summary>
        /// Factory to create vlc specific windows.
        /// </summary>
        public VlcDoubleWindowFactory DoubleWindowFactory {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (doubleWindowFactory);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                doubleWindowFactory = value;
            }
        }

        private string version;

        /// <summary>
        /// Version of vlc library.
        /// </summary>
        public string Version {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (version == null) {
                    version = LibVlcInterop.libvlc_get_version();
                }
                //
                return (version);
            }
        }

        #region ILogVerbosityManager members

        /// <summary>
        /// Global vlclib log verbosity level.
        /// </summary>
        public int Verbosity {
            get {
                VerifyObjectIsNotDisposed();
                //
                uint res = LibVlcInterop.libvlc_get_log_verbosity(descriptor);
                //
                return Convert.ToInt32(res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                LibVlcInterop.libvlc_set_log_verbosity(descriptor, Convert.ToUInt32(value));
            }
        }

        #endregion

        /// <summary>
        /// Disposes all unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">If this parameters is <code>True</code> then Dispose is called explicitly, else Dispose called from finalizer.</param>
        protected override void Dispose(bool isDisposing) {
            try {
                if (isDisposing) {
                    if (thread != null) {
                        try {
                            if (thread.IsAlive) {
                                stopLoggingThread();
                            }
                        } catch (Exception exc) {
                            if (logger.IsErrorEnabled) {
                                logger.Error("Error while trying to interrupt a logging thread.", exc);
                            }
                            throw;
                        }
                    }
                    if (vlcLog != null) {
                        vlcLog.Dispose();
                    }
                }
                //
                if (descriptor != IntPtr.Zero) {
                    LibVlcInterop.libvlc_release(descriptor);
                }
            } finally {
                base.Dispose(isDisposing);
            }
        }

        /// <summary>
        /// Indicates which player implementation will be used.
        /// </summary>
        public bool CreateSinglePlayers {
            get {
            	return (createSinglePlayers);
            }
            set {
            	createSinglePlayers = value;
            }
        }

        /// <summary>
        /// Creates player which can be used to control media playing.
        /// </summary>
        /// <returns>Player instance.</returns>
        public override Player CreatePlayer(PlayerOutput playerOutput) {
            VerifyObjectIsNotDisposed();
            //
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            //
            if (CreateSinglePlayers) {
                return new VlcSinglePlayer(playerOutput, internalFactory);
            } else {
                return new VlcPlayer(playerOutput, internalFactory);
            }
        }

        /// <summary>
        /// Creates window where player renders video. User can control position of window and it's size.
        /// </summary>
        /// <returns>Window for rendering video.</returns>
        public override MediaWindow CreateWindow() {
            VerifyObjectIsNotDisposed();
            //
            if (doubleWindowFactory == null) {
                throw new InvalidOperationException("Factory for windows creation is not initialized.");
            }
            //
            return DoubleWindowFactory.CreateWindow();
        }

        #region Logging routine

        private readonly VlcLog vlcLog;
        private Thread thread;
        private readonly EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        private bool threadStopSignalSent;
    	private bool createSinglePlayers = true;

    	private void initializeAndStartLoggingThread() {
            thread = new Thread(loggingThreadFunc);
            thread.IsBackground = true;
            //
            thread.Start();
        }

        private void stopLoggingThread() {
            threadStopSignalSent = true;
            waitHandle.Set();
            //
            thread.Join();
        }

        private void loggingThreadFunc() {
            while (!threadStopSignalSent) {
                try {
                    vlcLog.UpdateMessages();
                    vlcLog.Clear();
                } catch (ObjectDisposedException) {
                    if (logger.IsWarnEnabled) {
                        logger.Warn("Logging thread : vlc logger instance has been disposed.");
                    }
                }
                //
                waitHandle.WaitOne(new TimeSpan(0, 0, 5), true);
            }
        }

        #endregion
    }
}