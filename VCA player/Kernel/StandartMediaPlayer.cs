using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;

namespace VCA_player.Kernel
{
    public class StandartMediaPlayer : IKernelPlayer
    {
        private readonly MediaPlayer _player = new MediaPlayer();
        private Boolean _isPause = false;
        private DispatcherTimer _timer;

        public StandartMediaPlayer()
        {
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, TimerCallback,
                System.Windows.Application.Current.Dispatcher);
            _player.MediaFailed += (s, e) =>
                {
                    System.Windows.MessageBox.Show(e.ErrorException.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    RaiseEvent(ref StartPlaying);
                    RaiseEvent(ref FinishPlaying);
                };
            _player.MediaOpened += (s, e) =>
            {
                RaiseEvent(ref StartPlaying);
            };
            _player.MediaEnded += (s, e) =>
            {
                RaiseEvent(ref FinishPlaying);
            };
        }

        public void Play(String url)
        {
            _isPause = false;

            RaiseEvent(ref PreparePlaying);
            _player.Open(new Uri(url));
            _player.Play();
        }

        public void SetPositionPercentage(Double percent)
        {
            _player.Position = TimeSpan.FromMilliseconds(_player.NaturalDuration.TimeSpan.TotalMilliseconds*percent);
        }

        public void Stop()
        {
            _isPause = false;

            _player.Stop();
            this.RaiseEvent(ref StopPlaying);
        }

        public void SetVolume(Int32 vol)
        {
            _player.Volume = (double) vol/100;
        }

        public void TogglePause()
        {
            if (_isPause)
            {
                _isPause = false;
                _player.Play();
                this.RaiseEvent(ref StartPlaying);
            }
            else
            {
                _isPause = true;
                _player.Pause();
                this.RaiseEvent(ref PausePlaying);
            }
        }

        private void TimerCallback(object sender, EventArgs e)
        {
            if (_player.HasAudio && _player.NaturalDuration.HasTimeSpan)
            {
                RaiseTimeChanged(new TimeChangedEventArgs(_player.Position));

                Double pos = _player.Position.TotalMilliseconds;
                Double tot = _player.NaturalDuration.TimeSpan.TotalMilliseconds;
                RaisePositionChanged(new PositionChangedEventArgs(pos/tot));
            }
        }

        #region events

        public event EventHandler FinishPlaying;
        public event EventHandler StartPlaying;
        public event EventHandler PreparePlaying;
        public event EventHandler PausePlaying;
        public event EventHandler StopPlaying;

        #region position changed

        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        private void RaisePositionChanged(PositionChangedEventArgs e)
        {
            var handle = Volatile.Read(ref PositionChanged);
            if (handle != null)
            {
                handle(this, e);
            }
        }

        #endregion

        #region time changed

        public event EventHandler<TimeChangedEventArgs> TimeChanged;
        private void RaiseTimeChanged(TimeChangedEventArgs e)
        {
            var handle = Volatile.Read(ref TimeChanged);
            if (handle != null)
            {
                handle(this, e);
            }
        }

        #endregion

        private void RaiseEvent(ref EventHandler handler)
        {
            var handlerCopy = Volatile.Read(ref handler);
            if (handlerCopy != null)
            {
                handlerCopy(this, EventArgs.Empty);
            }
        }


        #endregion
    }
}
