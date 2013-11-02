using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.Common;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interop;
using DZ.MediaPlayer.Vlc.Internal.Interfaces;
using DZ.MediaPlayer.Vlc.Internal.InternalObjects;

namespace DZ.MediaPlayer.Vlc.Internal {
	/// <summary>
	/// Because we have many implementations of <see cref="Player"/> we
	/// need base class to remove code duplication.
	/// </summary>
	public abstract class VlcPlayerBase : BasePlayer {
		/// <summary>
		/// These delegates should not be disposed before disposing VlcPlayer.
		/// </summary>
		internal readonly List<AttachedEvent> eventDelegates = new List<AttachedEvent>();
		internal int totalEventsAttached;
		
		private IInternalObjectsFactory internalObjectsFactory;
		
		internal VlcPlayerBase(IInternalObjectsFactory internalObjectsFactory) {
			this.internalObjectsFactory = internalObjectsFactory;
		}

		/// <summary>
		/// Attaches an event to player with selected eventManager.
		/// And stores used delegate in internal list.
		/// </summary>
		internal void attachToEvent(IntPtr eventManager, Delegate eventHandlerDelegate, libvlc_event_type_e eventType) {
			attachToEvent(eventManager, eventHandlerDelegate, eventType, null);
		}

		/// <summary>
		/// Attaches an event to player with selected eventManager.
		/// And stores used delegate in internal list.
		/// </summary>
		internal void attachToEvent(IntPtr eventManager, Delegate eventHandlerDelegate, libvlc_event_type_e eventType, Object tag) {
			if (eventManager == IntPtr.Zero) {
				throw new ArgumentException("IntPtr.Zero is invalid value.", "eventManager");
			}
			if (eventHandlerDelegate == null) {
				throw new ArgumentNullException("eventHandlerDelegate");
			}
			//
			AttachedEvent newEvent = new AttachedEvent(); {
				newEvent.EventManager = eventManager;
				newEvent.EventType = eventType;
				newEvent.UserData = new IntPtr(totalEventsAttached++);
				newEvent.Event = eventHandlerDelegate;
				newEvent.Function = Marshal.GetFunctionPointerForDelegate(eventHandlerDelegate);
				newEvent.Tag = tag;
			};
			//
			int res = LibVlcInterop.libvlc_event_attach(newEvent.EventManager, newEvent.EventType,
			                                            newEvent.Function,
			                                            newEvent.UserData);
			if (res != 0) {
				throw new VlcInternalException(String.Format("Unable to attach event {0}", eventType.ToString()));
			}
			// save delegate to a private list (to suppress finalizing it)
			eventDelegates.Add(newEvent);
		}

		/// <summary>
		/// Detaches events attached previously.
		/// </summary>
		internal static void detachEvent(AttachedEvent eventAttached) {
			LibVlcInterop.libvlc_event_detach(eventAttached.EventManager, eventAttached.EventType,
			                                  eventAttached.Function,
			                                  eventAttached.UserData);
		}

		internal void detachAllEvents() {
			foreach (AttachedEvent eventInfo in eventDelegates) {
				detachEvent(eventInfo);
			}
			eventDelegates.Clear();
		}

		internal void detachAllEventsWhereTag(Object tag) {
			for (int i = 0; i < eventDelegates.Count; ) {
				AttachedEvent eventInfo = eventDelegates[i];
				if (eventInfo.Tag == tag) {
					detachEvent(eventInfo);
					eventDelegates.RemoveAt(i);
				} else {
					i++;
				}
			}
		}
		
		/// <summary>
		/// Parses media and returns information about it.
		/// </summary>
		/// <param name="mediaInput">
		/// A <see cref="MediaInput"/> to locate media to be preparsed.
		/// </param>
		/// <returns>
		/// A <see cref="PreparsedMedia"/> instance with information about media located using
		/// <param cref="mediaInput"/>.
		/// </returns>
		public override PreparsedMedia ParseMediaInput(MediaInput mediaInput) {
			VlcMediaInternal media = internalObjectsFactory.CreateVlcMediaInternal(mediaInput);
			media.Parse();
			return new VlcPreparsedMedia(mediaInput, media);
		}
		
		/// <summary>
		/// Returns <see cref="VlcMediaInternal"/> preparsed using <see cref="ParseMediaInput"/>.
		/// </summary>
		/// <param name="media">
		/// A <see cref="PreparsedMedia"/> instance returned by <see cref="ParseMediaInput"/>,
		/// </param>
		/// <returns>
		/// A <see cref="VlcMediaInternal"/> instance in parsed state.
		/// </returns>
		internal VlcMediaInternal GetPreparsedMediaInternal(PreparsedMedia media) {
			if (media == null) {
				throw new ArgumentNullException("media");
			}
			if (!(media is VlcPreparsedMedia)) {
				throw new ArgumentException("Media is invalid.", "media");
			}
			//
			return (media as VlcPreparsedMedia).MediaInternal;
		}
		
		internal sealed class VlcPreparsedMedia : PreparsedMedia {
			private VlcMediaInternal media;
			
			public VlcPreparsedMedia(Io.MediaInput mediaInput, VlcMediaInternal media) : base(mediaInput) {
				if (media == null) {
					throw new ArgumentNullException("media");
				}
				this.media = media;
				this.Duration = media.Duration;
				//
				List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
				List<VideoTrackInfo> videoTracks = new List<VideoTrackInfo>();
				libvlc_media_track_info_t[] tracks = media.GetTracksInfo();
				foreach (libvlc_media_track_info_t track in tracks) {
					if (track.i_type == libvlc_track_t.libvlc_track_audio) {
						AudioTrackInfo audioTrack = new AudioTrackInfo();
						audioTrack.BitRate = track.i_rate;
						audioTrack.Channels = (int)Math.Min(track.i_channels, (uint)int.MaxValue);
						audioTrack.Code = track.i_codec;
						audioTrack.Description = LibVlcInterop.vlc_fourcc_GetDescription(0, track.i_codec);
						audioTracks.Add(audioTrack);
					} else if (track.i_type == libvlc_track_t.libvlc_track_video) {
						VideoTrackInfo videoTrack = new VideoTrackInfo();
						videoTrack.Code = track.i_codec;
						videoTrack.Description = LibVlcInterop.vlc_fourcc_GetDescription(0, track.i_codec);
						videoTrack.Height = (int)Math.Min((uint)int.MaxValue, (uint)track.i_height);
						videoTrack.Width = (int)Math.Min((uint)int.MaxValue, (uint)track.i_width);
						videoTracks.Add(videoTrack);
					}
				}
				//
				SetTracksInfo(videoTracks, audioTracks);
			}
			
			protected override void Dispose (bool isDisposing) {
				try {
					if (isDisposing) {
						media.Dispose();
					}
				} finally {
					base.Dispose(isDisposing);
				}
			}
			
			public VlcMediaInternal MediaInternal {
				get {
					return (media);
				}
			}
		}

		#region Nested type: AttachedEvent

		internal struct AttachedEvent {
			public Delegate Event;
			public IntPtr EventManager;
			public libvlc_event_type_e EventType;
			public IntPtr Function;
			public IntPtr UserData;
			public Object Tag;
		}

		#endregion
	}
}