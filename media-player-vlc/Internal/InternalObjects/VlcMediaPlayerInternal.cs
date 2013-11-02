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
using System.Runtime.InteropServices;
using Common.Logging;
using DZ.MediaPlayer.Vlc.Exceptions;
using DZ.MediaPlayer.Vlc.Internal.Interop;

#endregion

namespace DZ.MediaPlayer.Vlc.Internal.InternalObjects
{
    internal sealed class VlcMediaPlayerInternal : InternalObjectBase
    {
        private readonly IntPtr descriptor;
    	//private static readonly ILog logger = LogManager.GetLogger(typeof (VlcMediaPlayerInternal));

        public VlcMediaPlayerInternal(IntPtr descriptor) {
            if (descriptor == IntPtr.Zero) {
                throw new ArgumentException("Zero pointer.", "descriptor");
            }
            //
            this.descriptor = descriptor;
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
                LibVlcInterop.libvlc_media_player_release(descriptor);
            } finally {
                base.Dispose(isDisposing);
            }
        }

        #region Public Interfaces

        public TimeSpan Length {
            get {
                VerifyObjectIsNotDisposed();
                //
                Int64 res = LibVlcInterop.libvlc_media_player_get_length(descriptor);
                if (-1 == res) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
                //
                return new TimeSpan(res * 10000);
            }
        }

        public TimeSpan Time {
            get {
                VerifyObjectIsNotDisposed();
                //
                Int64 res = LibVlcInterop.libvlc_media_player_get_time(descriptor);
                if (res == -1) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
                //
                return new TimeSpan(res * 10000);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                LibVlcInterop.libvlc_media_player_set_time(descriptor, Convert.ToInt64(value.TotalMilliseconds));
            }
        }

        public float Position {
            get {
                VerifyObjectIsNotDisposed();
                //
                float res = LibVlcInterop.libvlc_media_player_get_position(descriptor);
                if (-1.0 == res) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
                //
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                LibVlcInterop.libvlc_media_player_set_position(descriptor, value);
            }
        }

        public int SPU {
            get {
                VerifyObjectIsNotDisposed();
                //
                int res = LibVlcInterop.libvlc_video_get_spu(descriptor);
                //
                return (res);
            }
            set {
                VerifyObjectIsNotDisposed();
                //
                int res = LibVlcInterop.libvlc_video_set_spu(descriptor, value);
                if (-1 == res) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
            }
        }

        /// <summary>
        /// Frames per second.
        /// </summary>
        public float FPS {
            get {
                VerifyObjectIsNotDisposed();
                //
                float res = LibVlcInterop.libvlc_media_player_get_fps(descriptor);
                return (res);
            }
        }

    	public bool IsFullScreen {
    		get {
    			VerifyObjectIsNotDisposed();
				//
    			int fullscreen = LibVlcInterop.libvlc_get_fullscreen(descriptor);
    			return fullscreen == 1;
    		}
    		set {
    			VerifyObjectIsNotDisposed();
				//
				LibVlcInterop.libvlc_set_fullscreen(descriptor, value ? 1 : 0);
    		}
    	}

    	public void SetMedia(VlcMediaInternal media) {
            if (media == null) {
                throw new ArgumentNullException("media");
            }
            //
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_set_media(descriptor, media.Descriptor);
        }

        public void SetDisplayOutputHwnd(IntPtr handle) {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_set_hwnd(descriptor, handle);
        }
		
		public void SetDisplayOutputNSObject(IntPtr handle) {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_set_nsobject(descriptor, handle);
        }
		
		public void SetDisplayOutputXWindow(IntPtr handle) {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_set_xwindow(descriptor, handle);
        }
		
		public void SetDisplayOutputAgl(IntPtr handle) {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_set_agl(descriptor, handle);
        }

        public void Play() {
            VerifyObjectIsNotDisposed();
            //
            int res = LibVlcInterop.libvlc_media_player_play(descriptor);
            if (res != 0) {
                throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
            }
        }

        public void Stop() {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_stop(descriptor);
        }

        public void Pause() {
            VerifyObjectIsNotDisposed();
            //
            LibVlcInterop.libvlc_media_player_pause(descriptor);
        }

        public void TakeSnapshot(string filePath, int width, int height) {
            VerifyObjectIsNotDisposed();
            //
            IntPtr filePathPtr = Marshal.StringToHGlobalAnsi(filePath);
            //
            uint uwidth = Convert.ToUInt32(width);
            uint uheight = Convert.ToUInt32(height);
            //
            try {
                int res = LibVlcInterop.libvlc_video_take_snapshot(descriptor, 0, filePathPtr, uwidth, uheight);
                if (-1 == res) {
                    throw new VlcInternalException(LibVlcInterop.libvlc_errmsg());
                }
            } finally {
                Marshal.FreeHGlobal(filePathPtr);
            }
        }

        #endregion
    }
}