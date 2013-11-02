using System;
using System.Runtime.InteropServices;

namespace DZ.MediaPlayer.Vlc.Internal.Interop {
	
	/// <summary>
	/// Track type.
	/// </summary>
	internal enum libvlc_track_t : int {
		/// <summary>
		/// Unknown track type.
		/// </summary>
		libvlc_track_unknown = -1,
		/// <summary>
		/// Track is audio.
		/// </summary>
		libvlc_track_audio = 0,
		/// <summary>
		/// Track is video.
		/// </summary>
		libvlc_track_video = 1,
		/// <summary>
		/// Track is text.
		/// </summary>
		libvlc_track_text = 2
	}
	
	/// <summary>
	/// Track information.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal class libvlc_media_track_info_t {
		// fourcc
		[FieldOffset(0)]
		public uint i_codec;
		[FieldOffset(4)]
		public int i_id;
		
		[FieldOffset(8)]
		public libvlc_track_t i_type;
		
		// video codec info
		[FieldOffset(12)]
		public int i_profile;
		[FieldOffset(16)]
		public int i_level;
		
		[FieldOffset(20)]
		public uint i_channels;
		[FieldOffset(24)]
		public uint i_rate;
		
		[FieldOffset(20)]
		public uint i_height;
		[FieldOffset(24)]
		public uint i_width;
	}
	
	/*
	 *  typedef struct libvlc_media_track_info_t
		{
		    uint32_t    i_codec;
		    int         i_id;
		    libvlc_track_type_t i_type;
		
		    int         i_profile;
		    int         i_level;
		
		    union {
		        struct {
		            unsigned    i_channels;
		            unsigned    i_rate;
		        } audio;
		        struct {
		            unsigned    i_height;
		            unsigned    i_width;
		        } video;
		    } u;
		
		}
	 */
}

