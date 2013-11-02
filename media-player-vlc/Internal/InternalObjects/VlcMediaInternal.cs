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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interop;
using Common.Logging;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.InternalObjects
{
    internal sealed class VlcMediaInternal : InternalObjectBase {
		private static readonly ILog logger = LogManager.GetLogger(typeof(VlcMediaInternal));
        private readonly IntPtr descriptor;
		private libvlc_media_track_info_t[] tracksInfo;
		private TimeSpan parseTimeout = TimeSpan.FromSeconds(5);
		private static ParseState parseState;
		private static readonly Object staticLock = new Object();

        public VlcMediaInternal(IntPtr descriptor) {
            if (descriptor == IntPtr.Zero) {
                throw new ArgumentException("Zero pointer.", "descriptor");
            }
            //
            this.descriptor = descriptor;
        }
		
		/// <summary>
		/// Timeout used during <see cref="Parse"/> call.
		/// </summary>
		public TimeSpan ParseTimeout {
			get {
				return (parseTimeout);
			}
			set {
				parseTimeout = value;
			}
		}
		
		/// <summary>
		/// Returns information about tracks retrieved after <see cref="Parse"/> call.
		/// </summary>
		/// <returns>
		/// A <see cref="libvlc_media_track_info_t[]"/> array of tracks.
		/// </returns>
		public libvlc_media_track_info_t[] GetTracksInfo() {
			if ( tracksInfo != null && tracksInfo.Length > 0 ) {
				libvlc_media_track_info_t[] tracks = new libvlc_media_track_info_t[tracksInfo.Length];
				tracksInfo.CopyTo(tracks, 0);
				return tracks;
			} else {
				return new libvlc_media_track_info_t[] {};
			}
		}

		public string GetMrl() {
			VerifyObjectIsNotDisposed();
			string mrl = LibVlcInterop.libvlc_media_get_mrl(this.descriptor);
			return mrl;
		}
		
		public bool IsParsed() {
			VerifyObjectIsNotDisposed();
			return LibVlcInterop.libvlc_media_is_parsed(this.descriptor);
		}
		
		public TimeSpan Duration {
			get {
				VerifyObjectIsNotDisposed();
				return new TimeSpan(LibVlcInterop.libvlc_media_get_duration(this.descriptor) * 10000);
			}
		}
		
		public void Parse() {
			VerifyObjectIsNotDisposed();
			//
			lock (staticLock) {
				if (!IsParsed()) { 
					LibVlcInterop.libvlc_media_parse(this.descriptor);
				}
				//
				if (tracksInfo == null) {
					parseState = new ParseState();
					parseState.waitHandle = new EventWaitHandle(false, 
						EventResetMode.AutoReset);
					try {
						IntPtr player = LibVlcInterop.libvlc_media_player_new_from_media(this.descriptor);
						if ( player == IntPtr.Zero ) {
							throw new VlcInternalException(String.Format("Can't create an instance of player when parsing. LibVlc tells: {0}", 
									LibVlcInterop.libvlc_errmsg()));
						}
						IntPtr playerEvents = LibVlcInterop.libvlc_media_player_event_manager(player);
						if ( playerEvents == IntPtr.Zero ) {
							throw new VlcInternalException(String.Format("Can't get event manager of player when parsing. LibVlc tells: {0}", 
									LibVlcInterop.libvlc_errmsg()));
						}
						//
						try {
							parseState.eventHandler = new LibVlcInterop.VlcEventHandlerDelegate(onTrackInfoEndReached);
							IntPtr function = Marshal.GetFunctionPointerForDelegate(parseState.eventHandler);
							//
							int res = LibVlcInterop.libvlc_event_attach(playerEvents, libvlc_event_type_e.libvlc_MediaPlayerEndReached,
							                                  function,
							                                  IntPtr.Zero);
							if (res != 0) {
								throw new VlcInternalException(String.Format("Unable to attach event {0}", 
									libvlc_event_type_e.libvlc_MediaPlayerEndReached.ToString()));
							}
							//
							LibVlcInterop.libvlc_media_add_option(descriptor, "sout=#description");
							LibVlcInterop.libvlc_media_player_play(player);
							//
							if (parseState.waitHandle.WaitOne(parseTimeout)) {
								//
								LibVlcInterop.libvlc_media_player_release(player);
								player = IntPtr.Zero;
								//
								tracksInfo = LibVlcInterop.libvlc_media_get_tracks_info(descriptor);
								//
								if (logger.IsInfoEnabled) {
									StringBuilder builder = new StringBuilder();
									builder.Append("Parsed tracks info: ");
									//
									foreach (libvlc_media_track_info_t track in tracksInfo) {
										builder.AppendLine("");
										if ( track.i_type == libvlc_track_t.libvlc_track_audio ) {
											builder.AppendLine("Type: audio");
											builder.AppendLine("Channels: " + track.i_channels);
											builder.AppendLine("Rate: " + track.i_rate);
										} else if ( track.i_type == libvlc_track_t.libvlc_track_text ) {
											builder.AppendLine("Type: text");
										} else if ( track.i_type == libvlc_track_t.libvlc_track_video ) {
											builder.AppendLine("Type: video");
										} else {
											builder.AppendLine("Type: unknown");
										}
										builder.AppendLine("Description: " + LibVlcInterop.vlc_fourcc_GetDescription(0, track.i_codec));
									}
									//
									builder.AppendLine("Total tracks: " + tracksInfo.Length);
									logger.Info(builder.ToString());
								}
							} else {
								throw new VlcTimeoutException("Can't get tracks information.");
							}
						} finally {
							if ( player != IntPtr.Zero ) {
								LibVlcInterop.libvlc_media_player_release(player);
							}
						}
					} catch(Exception exc) {
						if (logger.IsErrorEnabled) {
							logger.Error("An error occured during parse.", exc);
						}
						throw;
					} finally {
						parseState.waitHandle.Close();
						parseState = null;
					}
				}
			}
		}
		
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		private static void onTrackInfoEndReached(IntPtr eventInfo, IntPtr userData) {
			ParseState state = parseState;
			state.waitHandle.Set();
		}
		
        public override IntPtr Descriptor {
            get {
                VerifyObjectIsNotDisposed();
                //
                return (descriptor);
            }
        }

        protected override void Dispose(bool isDisposing) {
            try {
                LibVlcInterop.libvlc_media_release(descriptor);
            } finally {
                base.Dispose(isDisposing);
            }
        }

        private void addOption(string option) {
            if (option == null) {
                throw new ArgumentNullException("option");
            }
            if (option.Length == 0) {
                throw new ArgumentException("String is empty.", "option");
            }
            //
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_add_option(descriptor, option);
        }

        #region Public interfaces

        public VlcMediaState State {
            get {
                VerifyObjectIsNotDisposed();
                //
                VlcMediaState res = (VlcMediaState) LibVlcInterop.libvlc_media_get_state(descriptor);
                //
                return (res);
            }
        }

        public void SetOutput(PlayerOutput playerOutput) {
            if (playerOutput == null) {
                throw new ArgumentNullException("playerOutput");
            }
            //
            VerifyObjectIsNotDisposed();
            //
			if ( string.IsNullOrEmpty(playerOutput.OutMrl) ) {
	            if ((playerOutput.Files.Count == 0) && (playerOutput.NetworkStreams.Count == 0)) {
	                //
	                //addOption("--video-filter=adjust@my_label");
	                //
	                return;
	            }
	
	            //string transcodeString = "vcodec=WMV2,vb=800,scale=1,acodec=wma,ab=128,channels=2";
	            const string transcodeString = "vcodec=WMV2,vb=1024,scale=1";
	
	            // Здесь media должна знать, будет ли она дублироваться на экран
	            string duplicateString = (playerOutput.Window != null) ? "dst=display" : String.Empty;
	            foreach (OutFile file in playerOutput.Files) {
	                //dst=std{access=file,mux=ps,dst=\"{0}\"}
	                string s = String.Format("dst=std[access=file,mux=ps,dst=\"{0}\"]", file.FileName);
	
	                if (String.IsNullOrEmpty(duplicateString)) {
	                    duplicateString = s;
	                } else {
	                    duplicateString += "," + s;
	                }
	            }
	
	            foreach (OutputNetworkStream stream in playerOutput.NetworkStreams) {
	                //dst=std{access=http,mux=asf,dst=172.28.1.4:8888}
	                //dst=rtp{dst=1231232,mux=asf,port=1234,port-audio=12367,port-video=31236}
	                string s;
	                if (stream.Protocol != NetworkProtocol.RTP) {
	                    s = String.Format("dst=std[access={0},mux=asf,dst={1}:{2}]",
	                        stream.Protocol.ToString().ToLower(), stream.Ip, stream.Port);
	                } else {
	                    s = String.Format("dst=rtp[dst={0},mux=asf,port={1},port-audio={2},port-video={3}]",
	                        stream.Ip, stream.Port, stream.RtpPortAudio, stream.RtpPortVideo);
	                }
	
	                if (String.IsNullOrEmpty(duplicateString)) {
	                    duplicateString = s;
	                } else {
	                    duplicateString += "," + s;
	                }
	            }
	
	            string optionsString = String.Format(":sout=#transcode[{0}]:duplicate[{1}]", transcodeString, duplicateString);
	
	            addOption(optionsString.Replace('[', '{').Replace(']', '}'));
			} else {
				addOption(playerOutput.OutMrl);
			}
        }

        #endregion
		
		private class ParseState {
			public EventWaitHandle waitHandle;
			public LibVlcInterop.VlcEventHandlerDelegate eventHandler;
		}
    }
}