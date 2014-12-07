using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Events;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;
using System.Diagnostics.Contracts;
using Rina.Infastructure.Models;

namespace Rina.Modules.AudioPlayer.Services
{
    [Export(typeof(IPlayerService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class PlayerService : IPlayerService
    {
        private readonly MediaPlayer player = new MediaPlayer();
        private readonly DispatcherTimer timer;

        private Boolean isDecresingProgress;

        public event EventHandler Starting;
        public event EventHandler Started;
        public event EventHandler Finished;
        public event EventHandler Paused;
        public event EventHandler Stoped;
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        public event EventHandler<DataEventArgs<Double>> LoadedChanged;
        public event EventHandler<DataEventArgs<Double>> VolumeChanged;

        public Double Volume
        {
            get
            {
                return this.player.Volume;
            }
        }

        public Boolean IsDecresingProgress
        {
            get { return this.isDecresingProgress; }
            set
            {
                this.isDecresingProgress = value;
                RaiseUpdatePosition();
            }
        }

        public PlayerService()
        {
            Contract.Ensures(this.player != null);

            this.timer = new DispatcherTimer(TimeSpan.FromMilliseconds(300), DispatcherPriority.Normal, TimerEllapsed,
                System.Windows.Application.Current.Dispatcher);
            this.player.MediaFailed += (s, e) =>
            {
                Debug.WriteLine(e.ErrorException.Message);
                this.RaiseEvent(ref Started);
                this.RaiseEvent(ref Finished);
            };
            this.player.MediaOpened += (s, e) => this.RaiseEvent(ref Started);
            this.player.MediaEnded += (s, e) => this.RaiseEvent(ref Finished);
            this.player.Changed += (s, e) => Debug.WriteLine("Changed");
        }

        public void Play(Uri uri)
        {
            if (this.player.HasAudio)
            {
                StopPlaying();
            }

            RaiseEvent(ref Starting);
            this.player.Open(uri);
            this.player.Play();
        }

        public void SetVolume(Double vol)
        {
            Contract.Requires(vol >= 0 && vol <= 1);

            this.player.Volume = vol;
            RaiseVolumeChanged(this.player.Volume);
        }

        public void SetPosition(Double position)
        {
            Contract.Requires(position >= 0 && position <= 1);
            Debug.Assert(this.player.HasAudio);
            Debug.Assert(this.player.NaturalDuration.HasTimeSpan);

            this.player.Position = TimeSpan.FromSeconds(this.player.NaturalDuration.TimeSpan.TotalSeconds * position);
        }

        public void StopPlaying()
        {
            Debug.Assert(this.player.HasAudio);
            this.player.Stop();
            this.RaiseEvent(ref Stoped);
        }

        public void Play()
        {
            Debug.Assert(this.player.HasAudio);

            this.player.Play();
            this.RaiseEvent(ref Started);
        }

        public void Pause()
        {
            Debug.Assert(this.player.HasAudio);

            this.player.Pause();
            this.RaiseEvent(ref Paused);
        }

        #region events

        private void RaiseEvent(ref EventHandler handler)
        {
            var handle = Volatile.Read(ref handler);
            if (handle != null)
            {
                handle(this, EventArgs.Empty);
            }
        }

        private void RaisePositionChanged(TimeSpan position, Double relativePosition)
        {
            var handle = Volatile.Read(ref this.PositionChanged);
            if (handle != null)
            {
                handle(this, new PositionChangedEventArgs(position, relativePosition));
            }
        }

        private void RaiseLoadedChanged(Double progress)
        {
            var handle = Volatile.Read(ref this.LoadedChanged);
            if (handle != null)
            {
                handle(this, new DataEventArgs<Double>(progress));
            }
        }

        private void RaiseVolumeChanged(Double volume)
        {
            var handle = Volatile.Read(ref this.VolumeChanged);
            if (handle != null)
            {
                handle(this, new DataEventArgs<Double>(volume));
            }
        }

        private void TimerEllapsed(object sender, EventArgs e)
        {
            RaiseUpdatePosition();
        }

        private void RaiseUpdatePosition()
        {
            if (this.player.HasAudio && this.player.NaturalDuration.HasTimeSpan)
            {
                Double relative = this.player.Position.TotalSeconds / this.player.NaturalDuration.TimeSpan.TotalSeconds;

                RaisePositionChanged(GetPosition(), relative);
                RaiseLoadedChanged(this.player.DownloadProgress);
            }
        }

        private TimeSpan GetPosition()
        {
            if (IsDecresingProgress)
            {
                return this.player.NaturalDuration.TimeSpan.Subtract(this.player.Position).Negate();
            }
            else
            {
                return this.player.Position;
            }
        }

        #endregion
    }
}
