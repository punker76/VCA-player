using System;
namespace DZ.MediaPlayer {
	
	/// <summary>
	/// Base class for all windows. This class identifies window where player should output
	/// and depends on Platform and implementation of Player.
	/// </summary>
	public class NativeMediaWindow : IDisposable {
		
		private IntPtr _nativeWindowHandle;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public NativeMediaWindow() {
		}
		
		/// <summary>
		/// Constructor with parameter which identifies window.
		/// </summary>
		/// <param name="handle">
		/// A <see cref="IntPtr"/> which identifies window.
		/// </param>
		public NativeMediaWindow(IntPtr handle) {
			if (handle == IntPtr.Zero) {
				throw new ArgumentException("Parameter cannot be zero.");
			}
			_nativeWindowHandle = handle;
		}
		
		/// <summary>
		/// Window handle which is used internally by specific implementation of Player.
		/// </summary>
		public virtual IntPtr NativeWindowHandle {
			get {
				return (_nativeWindowHandle);
			}
			set {
				_nativeWindowHandle = value;
			}
		}
		
		#region IDisposable Members

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose() {
            Dispose(true);
        }

        #endregion

        /// <summary>
        /// Disposes resources.
        /// </summary>
        /// <param name="isDisposing">Defines where this method is invoked. <code>True</code> if from Dispose call, else from finalizer.</param>
        protected virtual void Dispose(bool isDisposing) {
        }
	}
}

