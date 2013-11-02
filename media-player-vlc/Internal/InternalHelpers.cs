using System;

namespace DZ.MediaPlayer.Vlc.Internal {
    internal static class InternalHelpers {
        public static PlayerState ConvertState(VlcPlayerState vlcState) {
            throw new InvalidOperationException("Cannot convert internal VlcPlayerState to PlayerState.");
        }
    }
}