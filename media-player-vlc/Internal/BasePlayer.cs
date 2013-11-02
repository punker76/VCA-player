using System;
using DZ.MediaPlayer.Vlc;

namespace DZ.MediaPlayer.Vlc.Internal {
	
	#pragma warning disable 0419
	
	/// <summary>
	/// Implements some basic logic.
	/// </summary>
	/// <remarks>
	/// Controls <see cref="Player.State"/> property.
	/// Defines behaviour of <see cref="Player.Play"/>, <see cref="Player.PlayNext"/>, <see cref="Player.Pause"/>, 
	/// <see cref="Player.Resume"/> in case of exceptions.
	/// If any exception is thrown inside that methods, <see cref="BasePlayer.StopInternal"/> is called, and <see cref="Player.State"/> is
	/// set to <see cref="PlayerState.Stopped"/> value.
	/// Player can't be in disposed state during control.
	/// </remarks>
	public abstract class BasePlayer : Player {
		/// <summary>
		/// This property is controlled by <see cref="BasePlayer"/>. 
		/// Please change the value only if required very carefully. Changing it inside
		/// of <see cref="BasePlayer.PlayInternal"/>, <see cref="BasePlayer.PlayNextInternal"/>,
		/// <see cref="BasePlayer.StopInternal"/>, <see cref="BasePlayer.PauseInternal"/>, 
		/// <see cref="BasePlayer.ResumeInternal"/> won't give any effect on <see cref="BasePlayer.State"/>
		/// property. Call it only inside of the new methods.
		/// </summary>
		protected PlayerState playerState;
		
		/// <summary>
		/// Identifies current player state.
		/// </summary>
		public sealed override PlayerState State {
			get {
				VerifyObjectIsNotDisposed();
				return playerState;
			}
		}
		
		/// <summary>
		/// Starts playing of media which was initialized using <see cref="SetMediaInput"/> method.
		/// </summary>
		public sealed override void Play () {
			VerifyObjectIsNotDisposed();
			try {
				PlayInternal();
				playerState = PlayerState.Playing;
			} catch(Exception exc) {
				playerState = PlayerState.Stopped;
				StopInternal(exc);
				throw;
			}
		}
		
		/// <summary>
		/// Plays next media which was initialized using <see cref="SetNextMediaInput"/> method.
		/// </summary>
		public sealed override void PlayNext () {
			VerifyObjectIsNotDisposed();
			try {
				PlayNextInternal();
				playerState = PlayerState.Playing;
			} catch(Exception exc) {
				playerState = PlayerState.Stopped;
				StopInternal(exc);
				throw;
			}
		}
		
		/// <summary>
		/// Resumes playing after <see cref="Pause"/> call.
		/// </summary>
		public sealed override void Resume () {
			VerifyObjectIsNotDisposed();
			try {
				ResumeInternal();
				playerState = PlayerState.Playing;
			} catch(Exception exc) {
				playerState = PlayerState.Stopped;
				StopInternal(exc);
				throw;
			}
		}
		
		/// <summary>
		/// Pauses playing.
		/// </summary>
		public sealed override void Pause () {
			VerifyObjectIsNotDisposed();
			try {
				PauseInternal();
				playerState = PlayerState.Paused;
			} catch(Exception exc) {
				playerState = PlayerState.Stopped;
				StopInternal(exc);
				throw;
			}
		}
		
		/// <summary>
		/// Stops playing.
		/// </summary>
		public sealed override void Stop () {
			VerifyObjectIsNotDisposed();
			StopInternal(null);
			playerState = PlayerState.Stopped;
		}
		
		/// <summary>
		/// This is called from <see cref="Player.Play"/> and should be implemented in derived classes.
		/// </summary>
		protected abstract void PlayInternal();
		
		/// <summary>
		/// This is called from <see cref="Player.PlayNext"/> and should be implemented in derived classes.
		/// </summary>
		protected abstract void PlayNextInternal();
		
		/// <summary>
		/// This is called from <see cref="Player.Resume"/> and should be implemented in derived classes.
		/// </summary>
		protected abstract void ResumeInternal();
		
		/// <summary>
		/// This is called from <see cref="Player.Pause"/> and should be implemented in derived classes.
		/// </summary>
		protected abstract void PauseInternal();
		
		/// <summary>
		/// This is called from <see cref="Player.Stop"/> and should be implemented in derived classes.
		/// </summary>
		/// <remarks>
		/// Also this method is called in case of any exception inside of the <see cref="Player.Play"/>, 
		/// <see cref="Player.PlayNext"/>, <see cref="Player.Pause"/>, <see cref="Player.Resume"/>. Exception
		/// is passed as a parameter. In case of normal <see cref="Player.Stop"/> call that parameter is null.
		/// </remarks>
		/// <param name="exception">
		/// A <see cref="Exception"/> which was thrown in these methods: <see cref="Player.Play"/>, <see cref="Player.PlayNext"/>, <see cref="Player.Pause"/>, 
		/// <see cref="Player.Resume"/>, or null if it's just normal <see cref="Player.Stop"/>.
		/// </param>
		protected abstract void StopInternal(Exception exception);
		
	}
}

