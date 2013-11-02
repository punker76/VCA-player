using System;
namespace DZ.MediaPlayer.Vlc {
	
	/// <summary>
	/// Type of window defines type of handle stored in
	/// <see cref="VlcNativeMediaWindow"/>. 
	/// </summary>
	public enum VlcWindowType {
		/// <summary>
		/// Windows OS window handle.
		/// </summary>
		HWND,
		/// <summary>
		/// Mac OS NSView * instance.
		/// </summary>
		NSObject,
		/// <summary>
		/// X Windows subsystem drawable.
		/// </summary>
		XWindow,
		/// <summary>
		/// Agl rendering output.
		/// </summary>
		Agl
	}
	
	/// <summary>
	/// Holds information about type of window and pointer which identifies window.
	/// </summary>
	public class VlcNativeMediaWindow : NativeMediaWindow {
		
		private VlcWindowType _windowType;
		
		/// <summary>
		/// Default constructor takes handle and type of window
		/// </summary>
		/// <param name="handle">
		/// A <see cref="IntPtr"/> which identifies the window.
		/// </param>
		/// <param name="type">
		/// A <see cref="VlcWindowType"/> defines type of the window.
		/// </param>
		public VlcNativeMediaWindow (IntPtr handle, VlcWindowType type) : base(handle) {
			if (type < VlcWindowType.HWND || type > VlcWindowType.Agl) {
				throw new ArgumentOutOfRangeException("type");
			}
			_windowType = type;
		}
		
		/// <summary>
		/// Type of the window.
		/// </summary>
		public VlcWindowType WindowType {
			get {
				return (_windowType);
			}
		}
	}
}

