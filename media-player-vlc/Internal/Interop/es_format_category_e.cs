using System;
namespace DZ.MediaPlayer.Vlc {
	internal enum es_format_category_e {
	    UNKNOWN_ES = 0x00,
	    VIDEO_ES   = 0x01,
	    AUDIO_ES   = 0x02,
	    SPU_ES     = 0x03,
	    NAV_ES     = 0x04,
	}
}

