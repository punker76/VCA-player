using System;
namespace DZ.MediaPlayer.Vlc {
	/// <summary>
	/// This class implements <see cref="PlayerEventsReceiver"/> class by calling
	/// events declared inside.
	/// </summary>
	public class EventsBasedPlayerEventsReceiver : PlayerEventsReceiver {
		private readonly object sender;
		
		/// <summary>
		/// Default contructor.
		/// </summary>
		public EventsBasedPlayerEventsReceiver() {
			this.sender = this;
		}
		
		/// <summary>
		/// Constructor with parameter which sets sender used in events.
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/> passed to <see cref="EventHandler"/> events as a sender.
		/// </param>
		public EventsBasedPlayerEventsReceiver(object sender) {
			this.sender = sender;
		}
		
		/// <summary>
        /// Some error occured.
        /// </summary>
		public event EventHandler EncounteredError;
		
		/// <summary>
		/// End of media is reached.
		/// </summary>
		public event EventHandler EndReached;
		
		/// <summary>
		/// Position's changed.
		/// </summary>
		public event EventHandler PositionChanged;
		
		/// <summary>
		/// State's changed.
		/// </summary>
		public event EventHandler StateChanged;
		
		/// <summary>
		/// Player is stopped
		/// </summary>
		public event EventHandler Stopped;
		
		/// <summary>
		/// Current time is changed
		/// </summary>
		public event EventHandler TimeChanged;
		
		/// <summary>
		/// Raises error event.
		/// </summary>
		public override void OnEncounteredError () {
			EventHandler handler = EncounteredError;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises end of media reached event.
		/// </summary>
		public override void OnEndReached () {
			EventHandler handler = EndReached;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises position change.
		/// </summary>
		public override void OnPositionChanged () {
			EventHandler handler = PositionChanged;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises state change event.
		/// </summary>
		public override void OnStateChanged () {
			EventHandler handler = StateChanged;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises stop event.
		/// </summary>
		public override void OnStopped () {
			EventHandler handler = Stopped;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
		
		/// <summary>
		/// Raises time change event.
		/// </summary>
		public override void OnTimeChanged () {
			EventHandler handler = TimeChanged;
			if (handler != null) {
				handler(sender, EventArgs.Empty);
			}
		}
	}
}

