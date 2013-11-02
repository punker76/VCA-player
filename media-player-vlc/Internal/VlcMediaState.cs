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

namespace DZ.MediaPlayer.Vlc.Internal
{
    internal enum VlcMediaState
    {
		NothingSpecial = Interop.libvlc_state_t.libvlc_NothingSpecial,
		Opening = Interop.libvlc_state_t.libvlc_Opening,
		Buffering = Interop.libvlc_state_t.libvlc_Buffering,
		Playing = Interop.libvlc_state_t.libvlc_Playing,
		Paused = Interop.libvlc_state_t.libvlc_Paused,
		Stopped = Interop.libvlc_state_t.libvlc_Stopped,
		Ended = Interop.libvlc_state_t.libvlc_Ended,
		Error = Interop.libvlc_state_t.libvlc_Error,
    }
}