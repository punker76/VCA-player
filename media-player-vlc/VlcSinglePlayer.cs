using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Common.Logging;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal;
using DZ.MediaPlayer.Vlc.Internal.Interfaces;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;
using DZ.MediaPlayer.Vlc.Internal.Interop;
using DZ.MediaPlayer.Vlc.Io;

namespace DZ.MediaPlayer.Vlc {
	
	#pragma warning disable 0419
	
    /// <summary>
    /// Represents <see cref="Player"/> abstract class implementation using libvlc and
    /// only one of libvlc_player object. 
    /// </summary>
    /// <remarks>
    /// Alternative to previous <see cref="VlcPlayer"/> implementation. 
    /// This is simpler implementation of <see cref="Player"/> which is better approach to use library.
    /// </remarks>
    public sealed class VlcSinglePlayer : VlcPlayerBase {
        private static readonly ILog logger = LogManager.GetLogger(typeof (VlcSinglePlayer));
        private readonly List<PlayerEventsReceiver> eventsReceivers = new List<PlayerEventsReceiver>();

        // Necessary to create internal objects
        private readonly IInternalObjectsFactory internalObjectsFactory;
        private readonly PlayerOutput playerOutput;
        private readonly EventWaitHandle stateChangeEventHandle; // TODO: dispose
        private MediaInput currentMedia;
		
		// this will be set from Play/PlayNext methods. this is actually playing media
        private VlcMediaInternal currentMediaInternal;
		// we do not control VlcMediaInternal in case if it was preparsed.
		private bool disposeCurrentMedia;
		
		// these will be set from SetMediaInput methods: 
		private VlcMediaInternal currentMediaInternalPreprepared;
		private VlcMediaInternal nextMediaInternalPreprepared;
		private bool disposeCurrentMediaPreprepared;
		private bool disposeNextMediaPreprepared;
		
        private readonly VlcMediaPlayerInternal internalPlayer;
        private bool isFullScreen;
        private MediaInput nextMedia;
        private TimeSpan waitingRequiredStateTimeout = TimeSpan.FromSeconds(20);

        internal VlcSinglePlayer(PlayerOutput playerOutput, IInternalObjectsFactory internalObjectsFactory) 
			:base(internalObjectsFactory) {
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
			if (playerOutput.Window != null && !(playerOutput.Window is DoubleWindowBase)) {
				//NOTE: it is not realy neccessary for VlcSinglePlayer - should we create simpler Window class?
				throw new ArgumentException("Window property of PlayerOutput should be of class DoubleWindowBase.", "playerOutput");
			}
            if (internalObjectsFactory == null) {
                throw new ArgumentNullException("internalObjectsFactory");
            }
            //
            this.playerOutput = playerOutput;
            this.internalObjectsFactory = internalObjectsFactory;
            //
            stateChangeEventHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            //
            internalPlayer = internalObjectsFactory.CreateVlcMediaPlayerInternal();
            initializeEvents();
        }


        /// <summary>
        /// Timeout to wait a required state.
        /// </summary>
        public TimeSpan WaitingRequiredStateTimeout {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (waitingRequiredStateTimeout);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                waitingRequiredStateTimeout = value;
            }
        }
	
		/// <summary>
        /// Position of the currently playing media (0.0f - 1.0f).
        /// </summary>
        public override float Position {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                return (internalPlayer.Position);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value < 0f) || (value > 1.0f)) {
                    throw new ArgumentOutOfRangeException("value", "must be (0.0 - 1.0)");
                }
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                internalPlayer.Position = value;
            }
        }
		
		/// <summary>
		/// Indicates player fullscreen state. If that property is <value>true</value> then
		/// player output window is in fullscreen mode.
		/// </summary>
        public override bool IsFullScreen {
            get {
                return (isFullScreen);
            }
            set {
                if (isFullScreen != value) {
                    isFullScreen = value;
                    makeFullScreen(value);
                }
            }
        }
		
		/// <summary>
        /// Time of the currently playing media.
        /// </summary>
        public override TimeSpan Time {
            get {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                return (internalPlayer.Time);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if (currentMediaInternal == null) {
                    throw new MediaPlayerException("Current media is not loaded.");
                }
                //
                internalPlayer.Time = value;
            }
        }
		
		/// <summary>
        /// Audio volume (0 - 200).
        /// </summary>
        public override int Volume {
            get {
                VerifyObjectIsNotDisposed();
                //
                int res = LibVlcInterop.libvlc_audio_get_volume(this.internalPlayer.Descriptor);
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                if ((value > 200) || (value < 0)) {
                    throw new ArgumentException("Must be between 0 and 200.", "value");
                }
                //
                int res = LibVlcInterop.libvlc_audio_set_volume(this.internalPlayer.Descriptor, value);
                if (res != 0) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
            }
        }

        /// <summary>
        /// List of subscribers to the player events.
        /// </summary>
        public override IList<PlayerEventsReceiver> EventsReceivers {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (eventsReceivers);
            }
        }

        private void initializeEvents() {
            VlcMediaPlayerInternal player = internalPlayer;
            //
            IntPtr pFirstMediaPlayerInternalEventManager =
                LibVlcInterop.libvlc_media_player_event_manager(player.Descriptor);

            // Attaching to player
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_TimeChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerTimeChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_Stopped),
                          libvlc_event_type_e.libvlc_MediaPlayerStopped);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_PositionChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerPositionChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_EncounteredError),
                          libvlc_event_type_e.libvlc_MediaPlayerEncounteredError);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_EndReached),
                          libvlc_event_type_e.libvlc_MediaPlayerEndReached);

            // StateChanged events
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerOpening);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerBuffering);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerPlaying);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerPaused);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerSeekableChanged);
            attachToEvent(pFirstMediaPlayerInternalEventManager,
                          new LibVlcInterop.VlcEventHandlerDelegate(playerInternal_StateChanged),
                          libvlc_event_type_e.libvlc_MediaPlayerPausableChanged);
        }

        // TODO: is cleaning all internals after timeout good idea? timeout means strange - so maybe reinitialize?
        // TODO: handle timeout events and properly maintain internal state
        private void waitForStateChangeOrFail(VlcMediaState state) {
            while ((VlcMediaState) getVlcState() != state) {
				if (logger.IsTraceEnabled) {
					logger.Trace(string.Format(CultureInfo.InvariantCulture, "Waiting for state. Current state is {0}, but expected {1}.",
					             getVlcState(), state));
				}
                if (stateChangeEventHandle.WaitOne(waitingRequiredStateTimeout, false)) {
                    if ((VlcMediaState) getVlcState() == state)
                        return;
                } else {
					if (logger.IsErrorEnabled) {
						logger.Error(string.Format(CultureInfo.InvariantCulture, "Failing with invalid state. Current state is {0}, but expected {1}.",
					             getVlcState(), state));
					}
                    if ((VlcMediaState) getVlcState() != state) {
                        throw new VlcTimeoutException("Timeout waiting required state.");
                    }
                }
            }
        }

        private void onAnyEvent() {
            bool screen = internalPlayer.IsFullScreen;
            this.isFullScreen = screen;
        }

        private void playerInternal_StateChanged(IntPtr libvlc_event, IntPtr userdata) {
            onAnyEvent();
            if (stateChangeEventHandle != null) {
                stateChangeEventHandle.Set();
            }
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnStateChanged();
            }
        }

        private void playerInternal_EndReached(IntPtr libvlc_event, IntPtr userdata) {
			playerState = PlayerState.Stopped;
            onAnyEvent();
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnEndReached();
            }
        }

        private void playerInternal_EncounteredError(IntPtr libvlc_event, IntPtr userdata) {
            onAnyEvent();
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnEncounteredError();
            }
        }

        private void playerInternal_PositionChanged(IntPtr libvlc_event, IntPtr userdata) {
            onAnyEvent();
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnPositionChanged();
            }
        }

        private void playerInternal_Stopped(IntPtr libvlc_event, IntPtr userdata) {
            onAnyEvent();
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnStopped();
            }
        }

        private void playerInternal_TimeChanged(IntPtr libvlc_event, IntPtr userdata) {
            onAnyEvent();
            foreach (PlayerEventsReceiver receiver in eventsReceivers) {
                receiver.OnTimeChanged();
            }
        }

        private void stop() {
            if (currentMediaInternal != null) {
                try {
                    if ((currentMediaInternal.State != VlcMediaState.Ended) &&
                        (currentMediaInternal.State != VlcMediaState.NothingSpecial)) {
                        //
                        internalPlayer.Stop();
                        waitForStateChangeOrFail(VlcMediaState.Stopped);
                    }
                } finally {
                    if (disposeCurrentMedia) {
					    currentMediaInternal.Dispose();
                    	currentMediaInternal = null;
					}
                }
            }
        }
		
		/// <summary>
        /// Stops player and cleans up resources.
        /// </summary>
        protected override void Dispose(bool isDisposing) {
            try {
                if (isDisposing) {
                    // Suppress exceptions in Dispose()
                    try {
                        stop();
                    } catch (VlcTimeoutException exc) {
                        if (logger.IsErrorEnabled) {
                            logger.Error("Timeout while stopping player in Dispose() method.", exc);
                        }
                    }
                    //
                    if (internalPlayer != null) {
                        internalPlayer.Dispose();
                    }
                    if (currentMediaInternal != null && disposeCurrentMedia) {
                        currentMediaInternal.Dispose();
                        currentMediaInternal = null;
                    }
					//
					if (stateChangeEventHandle != null) {
						stateChangeEventHandle.Close();
					}
                }
            } finally {
                base.Dispose(isDisposing);
            }
        }

        private VlcPlayerState getVlcState() {
            if (currentMediaInternal == null) {
                return VlcPlayerState.Idle;
            }
            //
            return (VlcPlayerState) currentMediaInternal.State;
        }

        private void makeFullScreen(bool value) {
            internalPlayer.IsFullScreen = value;
        }
		
		/// <summary>
		/// Initializes media to play.
		/// </summary>
        /// <param name="mediaInput">A <see cref="MediaInput"/> instance which identifies resource to 
        /// play after <see cref="Play"/> method call.
        /// </param>
        public override void SetMediaInput(MediaInput mediaInput) {
            VerifyObjectIsNotDisposed();
            //
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            currentMedia = mediaInput;
			// precreating of libvlc media
			if ( currentMediaInternal != currentMediaInternalPreprepared ) {
				if ( currentMediaInternalPreprepared != null ) {
					currentMediaInternalPreprepared.Dispose();
					currentMediaInternalPreprepared = null;
				}
			}
			currentMediaInternalPreprepared = internalObjectsFactory.CreateVlcMediaInternal(currentMedia);
            currentMediaInternalPreprepared.SetOutput(playerOutput);
			disposeCurrentMediaPreprepared = true;
        }
		
		/// <summary>
		/// Initializes previously preparsed media to play.
		/// </summary>
		/// <param name="preparsedMedia">
		/// A <see cref="PreparsedMedia"/> instance returned from <see cref="ParseMediaInput"/> method.
		/// </param>
		public override void SetMediaInput(PreparsedMedia preparsedMedia) {
			VerifyObjectIsNotDisposed();
            //
            if (preparsedMedia == null) {
                throw new ArgumentNullException("preparsedMedia");
            }
            //
            currentMedia = preparsedMedia.MediaInput;
			// precreating of libvlc media
			if ( currentMediaInternal != currentMediaInternalPreprepared ) {
				if ( currentMediaInternalPreprepared != null ) {
					currentMediaInternalPreprepared.Dispose();
					currentMediaInternalPreprepared = null;
				}
			}
			VlcMediaInternal media = GetPreparsedMediaInternal(preparsedMedia);
            media.SetOutput(playerOutput);
			//
			currentMediaInternalPreprepared = media;
			disposeCurrentMediaPreprepared = false;
		}

        /// <summary>
        /// Specifies next media input.
        /// </summary>
        public override void SetNextMediaInput(MediaInput mediaInput) {
            VerifyObjectIsNotDisposed();
            //
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            nextMedia = mediaInput;
			// precreating of libvlc media
			if ( currentMediaInternal != currentMediaInternalPreprepared ) {
				if ( nextMediaInternalPreprepared != null ) {
					nextMediaInternalPreprepared.Dispose();
					nextMediaInternalPreprepared = null;
				}
			}
			nextMediaInternalPreprepared = internalObjectsFactory.CreateVlcMediaInternal(nextMedia);
            nextMediaInternalPreprepared.SetOutput(playerOutput);
			disposeNextMediaPreprepared = true;
        }
		
		/// <summary>
		/// Initializes previously preparsed next media to play.
		/// </summary>
		/// <param name="preparsedMedia">
		/// A <see cref="PreparsedMedia"/> instance returned from <see cref="ParseMediaInput"/> method.
		/// </param>
		public override void SetNextMediaInput(PreparsedMedia preparsedMedia) {
			VerifyObjectIsNotDisposed();
            //
            if (preparsedMedia == null) {
                throw new ArgumentNullException("mediaInput");
            }
            //
            nextMedia = preparsedMedia.MediaInput;
			// precreating of libvlc media
			if ( currentMediaInternal != currentMediaInternalPreprepared ) {
				if ( nextMediaInternalPreprepared != null ) {
					nextMediaInternalPreprepared.Dispose();
					nextMediaInternalPreprepared = null;
				}
			}
			VlcMediaInternal media = GetPreparsedMediaInternal(preparsedMedia);
            media.SetOutput(playerOutput);
			//
			nextMediaInternalPreprepared = media;
			disposeNextMediaPreprepared = true;
		}

        /// <summary>
        /// Starts playing of media which was initialized using <see cref="SetMediaInput"/> method.
        /// If some media is playing now it will be simply restarted.
        /// </summary>
        protected override void PlayInternal() {
            if (currentMedia == null) {
                throw new MediaPlayerException("Current media is null.");
            }
            if (playerOutput == null) {
                throw new MediaPlayerException("Player output is null.");
            }
            //
            if (currentMediaInternal != null) {
                if (currentMediaInternal.State == VlcMediaState.Paused) {
                    Resume();
                    return;
                }
                if (currentMediaInternal.State == VlcMediaState.Playing) {
                    Position = 0f;
                    return;
                }
                Stop();
            }
            // Verify currentMedia
            if (currentMedia.Type == MediaInputType.File) {
                if (!File.Exists(currentMedia.Source)) {
                    throw new FileNotFoundException("File of media specified was not found.", currentMedia.Source);
                }
            }
			if ( currentMediaInternalPreprepared == null ) {
				currentMediaInternalPreprepared = internalObjectsFactory.CreateVlcMediaInternal(currentMedia);
            	currentMediaInternalPreprepared.SetOutput(playerOutput);
			}
			// 
			currentMediaInternal = currentMediaInternalPreprepared;
			currentMediaInternalPreprepared = null;
			disposeCurrentMedia = disposeCurrentMediaPreprepared;
			//
            internalPlayer.SetMedia(currentMediaInternal);
            if (playerOutput.IsWindowDefined) {
				if ( playerOutput.Window != null && playerOutput.Window is DoubleWindowBase) {
					// support old code
					internalPlayer.SetDisplayOutputHwnd(((DoubleWindowBase) playerOutput.Window).GetActiveWindowHandleInternal());
				} else if ( playerOutput.NativeWindow != null && playerOutput.NativeWindow is VlcNativeMediaWindow ) {
					// this will tell what to method we should call to initialize window
					VlcNativeMediaWindow window = (VlcNativeMediaWindow)playerOutput.NativeWindow;
					if (window.WindowType == VlcWindowType.HWND) {
						internalPlayer.SetDisplayOutputHwnd(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.NSObject) {
						internalPlayer.SetDisplayOutputNSObject(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.XWindow) {
						internalPlayer.SetDisplayOutputXWindow(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.Agl) {
						internalPlayer.SetDisplayOutputAgl(window.NativeWindowHandle);
					}
				}
            }
            //
            startPlaying();
        }

        private void startPlaying() {
            internalPlayer.Play();
            waitForStateChangeOrFail(VlcMediaState.Playing);
        }

        /// <summary>
		/// Implementation used inside <see cref="Player.Pause"/>.
		/// </summary>
        protected override void PauseInternal() {
			PauseInternal(true);
        }
		
		/// <summary>
		/// Advanched <see cref="BasePlayer.PauseInternal"/> implementation allows to wait if the
		/// player is really paused.
		/// </summary>
		/// <param name="waitForPausedState">
		/// A <see cref="bool"/> value which indicates if we want to wait the player, or not.
		/// </param>
		private void PauseInternal(bool waitForPausedState) {
			if (currentMediaInternal == null) {
                throw new MediaPlayerException("Current media is not loaded.");
            }
            if (currentMediaInternal.State == VlcMediaState.Paused || currentMediaInternal.State == VlcMediaState.Ended) {
                return;
            }
            if (currentMediaInternal.State != VlcMediaState.Playing) {
                throw new MediaPlayerException("Unexpected media state, must be playing.");
            }
            //
            internalPlayer.Pause();
            if (waitForPausedState) {
                waitForStateChangeOrFail(VlcMediaState.Paused);
            }
		}

        /// <summary>
        /// Pauses the current player.
        /// </summary>
        /// <param name="waitForPausedState">If true, operation is synchronized. Else - asynchronous.</param>
        public void Pause(bool waitForPausedState) {
            VerifyObjectIsNotDisposed();
            //
			try {
				PauseInternal(waitForPausedState);
				playerState = PlayerState.Paused;
			} catch(Exception exception) {
				playerState = PlayerState.Stopped;
				StopInternal(exception);
				throw;
			}
        }

		
		/// <summary>
		/// Implementation used inside <see cref="Player.Resume"/>.
		/// </summary>
		protected sealed override void ResumeInternal() {
            ResumeInternal(true);
        }
		
		/// <summary>
		/// Advanched <see cref="BasePlayer.ResumeInternal"/> implementation allows to control
		/// if we want to wait the player to actually start playing, or just to send the message.
		/// </summary>
		/// <param name="waitForResume">
		/// A <see cref="bool"/> value, Should be <code>True</code> if we want to wait the
		/// player to change it's state, or <code>False</code> if we don't.
		/// </param>
		private void ResumeInternal(bool waitForResume) {
			if (currentMediaInternal == null) {
                throw new MediaPlayerException("Current media is not loaded.");
            }
            if (currentMediaInternal.State == VlcMediaState.Playing || currentMediaInternal.State == VlcMediaState.Ended) {
                return;
            }
            if (currentMediaInternal.State != VlcMediaState.Paused) {
                throw new MediaPlayerException("Unexpected media state, must be paused.");
            }
            internalPlayer.Pause();
            if (waitForResume) { 
                waitForStateChangeOrFail(VlcMediaState.Playing);
            }
		}

        /// <summary>
        /// Resumes the current player.
        /// </summary>
        /// <param name="waitForResume">If true, operation is synchronized. Else - asynchronous.</param>
        public void Resume(bool waitForResume) {
            VerifyObjectIsNotDisposed();
            //
			try {
				ResumeInternal(waitForResume);
				playerState = PlayerState.Playing;
			} catch(Exception exc) {
				playerState = PlayerState.Stopped;
				StopInternal(exc);
				throw;
			}
        }
		
		/// <summary>
		/// Stops playing.
		/// </summary>
		/// <param name="exception">
		/// A <see cref="Exception"/> instance.
		/// </param>
		protected override void StopInternal(Exception exception) {
            stop();
        }
		
		/// <summary>
		/// Internal.
		/// </summary>
		protected override void PlayNextInternal() {
            if (nextMedia == null) {
                throw new MediaPlayerException("Next media is not selected.");
            }
            // Verify nexyMedia
            if (nextMedia.Type == MediaInputType.File) {
                if (!File.Exists(nextMedia.Source)) {
                    throw new FileNotFoundException("File of media specified was not found.", nextMedia.Source);
                }
            }
            //
            VlcMediaPlayerInternal mediaplayer = internalPlayer;
            // create nextMediaInternal and start playing 
            VlcMediaInternal nextMediaInternal = nextMediaInternalPreprepared;
			nextMediaInternalPreprepared = null;
            //
            nextMediaInternal.SetOutput(playerOutput);
            mediaplayer.SetMedia(nextMediaInternal);
            if (playerOutput.IsWindowDefined) {
				if ( playerOutput.Window != null && playerOutput.Window is DoubleWindowBase) {
					// support old code
					internalPlayer.SetDisplayOutputHwnd(((DoubleWindowBase) playerOutput.Window).GetActiveWindowHandleInternal());
				} else if ( playerOutput.NativeWindow != null && playerOutput.NativeWindow is VlcNativeMediaWindow ) {
					// this will tell what to method we should call to initialize window
					VlcNativeMediaWindow window = (VlcNativeMediaWindow)playerOutput.NativeWindow;
					if (window.WindowType == VlcWindowType.HWND) {
						internalPlayer.SetDisplayOutputHwnd(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.NSObject) {
						internalPlayer.SetDisplayOutputNSObject(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.XWindow) {
						internalPlayer.SetDisplayOutputXWindow(window.NativeWindowHandle);
					} else if (window.WindowType == VlcWindowType.Agl) {
						internalPlayer.SetDisplayOutputAgl(window.NativeWindowHandle);
					}
				}
            }
            //
            if (currentMediaInternal != null && disposeCurrentMedia) {
                currentMediaInternal.Dispose();
                currentMediaInternal = null;
            }
			//
            currentMediaInternal = nextMediaInternal;
			disposeCurrentMedia = disposeNextMediaPreprepared;
            //
			startPlaying();
        }

        /// <summary>
        /// Takes the snapshot of currently playing media. Snapshot will be stored in specified file.
        /// </summary>
        /// <param name="filePath">Path where to save snapshot.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        public override void TakeSnapshot(string filePath, int width, int height) {
            VerifyObjectIsNotDisposed();
            //
            if (currentMediaInternal == null) {
                throw new MediaPlayerException("Player is empty.");
            }
            //
            if ((currentMediaInternal.State != VlcMediaState.Playing) &&
                (currentMediaInternal.State != VlcMediaState.Paused)) {
                throw new MediaPlayerException("Unexpected player state.");
            }
            //
            internalPlayer.TakeSnapshot(filePath, width, height);
        }
    }
}