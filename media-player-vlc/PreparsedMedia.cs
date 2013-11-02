using System;
using System.Collections.Generic;
using System.Text;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.Common;

namespace DZ.MediaPlayer {
	
	/// <summary>
	/// Contains information about preparsed <see cref="MediaInput"/>. This class
	/// becames mandatory to dispose.
	/// </summary>
	public class PreparsedMedia : DisposingRequiredObjectBase {
		
		private MediaInput mediaInput;
		private List<AudioTrackInfo> audioTracks = new List<AudioTrackInfo>();
		private List<VideoTrackInfo> videoTracks = new List<VideoTrackInfo>();
		
		/// <summary>
		/// Default contructor with <see cref="MediaInput"/> which was passed
		/// to <see cref="Player.ParseMediaInput"/>.
		/// </summary>
		/// <param name="mediaInput">
		/// A <see cref="MediaInput"/> instance to preparse.
		/// </param>
		public PreparsedMedia(MediaInput mediaInput) {
			if (mediaInput == null) {
				throw new ArgumentNullException("mediaInput");
			}
			this.mediaInput = mediaInput;
		}
		
		/// <summary>
		/// <see cref="MediaInput"/> instance used to create <see cref="PreparsedMedia"/>.
		/// </summary>
		public MediaInput MediaInput {
			get {
				return (mediaInput);
			}
		}
		
		/// <summary>
		/// The duration of the media.
		/// </summary>
		public TimeSpan Duration {
			get;
			protected set;
		}
		
		/// <summary>
		/// Identifies if there are any video track inside.
		/// </summary>
		public bool ContainsVideo {
			get {
				return videoTracks.Count > 0;
			}
		}
		
		/// <summary>
		/// Identifies if there are any audio track inside.
		/// </summary>
		public bool ContainsAudio {
			get {
				return audioTracks.Count > 0;
			}
		}
		
		/// <summary>
		/// Returns an array of audio tracks contained within media.
		/// </summary>
		/// <returns>
		/// An enumeration of audio tracks.
		/// </returns>
		public AudioTrackInfo[] GetAudioTracks() {
			return audioTracks.ToArray();
		}
		
		/// <summary>
		/// Returns an array of video tracks contained within media.
		/// </summary>
		/// <returns>
		/// An enumeration of video tracks.
		/// </returns>
		public VideoTrackInfo[] GetVideoTracks() {
			return videoTracks.ToArray();
		}
		
		/// <summary>
		/// Set information about tracks contained inside media.
		/// </summary>
		/// <param name="videoTracks">
		/// A list of <see cref="VideoTrackInfo"/> video tracks.
		/// </param>
		/// <param name="audioTracks">
		/// A list of <see cref="AudioTrackInfo"/> audio tracks.
		/// </param>
		protected void SetTracksInfo(IList<VideoTrackInfo> videoTracks, IList<AudioTrackInfo> audioTracks) {
			this.audioTracks = new List<AudioTrackInfo>(audioTracks);
			this.videoTracks = new List<VideoTrackInfo>(videoTracks);
		}
	}
	
	/// <summary>
	/// Incapsulates information about audio track.
	/// </summary>
	public class AudioTrackInfo {
		
		/// <summary>
		/// Codec FOURCC code.
		/// </summary>
		public UInt32 Code {
			get;
			set;
		}
		
		/// <summary>
		/// Codec description.
		/// </summary>
		public string Description {
			get;
			set;
		}
		
		/// <summary>
		/// Sample rate. Samples per second (Hz).
		/// </summary>
		public double BitRate {
			get;
			set;
		}
		
		/// <summary>
		/// Channels count.
		/// </summary>
		public int Channels {
			get;
			set;
		}
		
		/// <summary>
		/// Indicates if the track is stereo.
		/// </summary>
		public bool IsStereo {
			get {
				return Channels == 2;
			}
		}
	}
	
	/// <summary>
	/// Incapsulates information about video track.
	/// </summary>
	public class VideoTrackInfo {
		
		/// <summary>
		/// Codec FOURCC code.
		/// </summary>
		public UInt32 Code {
			get;
			set;
		}
		
		/// <summary>
		/// Codec description.
		/// </summary>
		public string Description {
			get;
			set;
		}
		
		/// <summary>
		/// Video frame width.
		/// </summary>
		public int Width {
			get;
			set;
		}
		
		/// <summary>
		/// Video frame height.
		/// </summary>
		public int Height {
			get;
			set;
		}
	}
}
