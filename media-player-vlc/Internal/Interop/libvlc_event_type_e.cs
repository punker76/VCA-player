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

namespace DZ.MediaPlayer.Vlc.Internal.Interop
{
    internal enum libvlc_event_type_e
    {
        /* Append new event types at the end of a category.
     * Do not remove, insert or re-order any entry.
     * Keep this in sync with src/control/event.c:libvlc_event_type_name(). */
        libvlc_MediaMetaChanged = 0,
        libvlc_MediaSubItemAdded,
        libvlc_MediaDurationChanged,
        libvlc_MediaParsedChanged,
        libvlc_MediaFreed,
        libvlc_MediaStateChanged,

        libvlc_MediaPlayerMediaChanged = 0x100,
        libvlc_MediaPlayerNothingSpecial,
        libvlc_MediaPlayerOpening,
        libvlc_MediaPlayerBuffering,
        libvlc_MediaPlayerPlaying,
        libvlc_MediaPlayerPaused,
        libvlc_MediaPlayerStopped,
        libvlc_MediaPlayerForward,
        libvlc_MediaPlayerBackward,
        libvlc_MediaPlayerEndReached,
        libvlc_MediaPlayerEncounteredError,
        libvlc_MediaPlayerTimeChanged,
        libvlc_MediaPlayerPositionChanged,
        libvlc_MediaPlayerSeekableChanged,
        libvlc_MediaPlayerPausableChanged,
        libvlc_MediaPlayerTitleChanged,
        libvlc_MediaPlayerSnapshotTaken,
        libvlc_MediaPlayerLengthChanged,

        libvlc_MediaListItemAdded = 0x200,
        libvlc_MediaListWillAddItem,
        libvlc_MediaListItemDeleted,
        libvlc_MediaListWillDeleteItem,

        libvlc_MediaListViewItemAdded = 0x300,
        libvlc_MediaListViewWillAddItem,
        libvlc_MediaListViewItemDeleted,
        libvlc_MediaListViewWillDeleteItem,

        libvlc_MediaListPlayerPlayed = 0x400,
        libvlc_MediaListPlayerNextItemSet,
        libvlc_MediaListPlayerStopped,

        libvlc_MediaDiscovererStarted = 0x500,
        libvlc_MediaDiscovererEnded,

        libvlc_VlmMediaAdded = 0x600,
        libvlc_VlmMediaRemoved,
        libvlc_VlmMediaChanged,
        libvlc_VlmMediaInstanceStarted,
        libvlc_VlmMediaInstanceStopped,
        libvlc_VlmMediaInstanceStatusInit,
        libvlc_VlmMediaInstanceStatusOpening,
        libvlc_VlmMediaInstanceStatusPlaying,
        libvlc_VlmMediaInstanceStatusPause,
        libvlc_VlmMediaInstanceStatusEnd,
        libvlc_VlmMediaInstanceStatusError,
    }
}
