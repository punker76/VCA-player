using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using DZ.MediaPlayer;
using DZ.MediaPlayer.Vlc;
using DZ.MediaPlayer.Vlc.Deployment;
using DZ.MediaPlayer.Vlc.Common;
using DZ.MediaPlayer.Vlc.Io;
using System.Reflection;

namespace VCA_player.Kernel
{
    public class VLCPlayer : PlayerEventsReceiver, IKernelPlayer
    {
        private VlcSinglePlayer p_player;
        private string p_currentUrl;

        public event EventHandler<PreparePlayingEventArgs> PreparePlaying;
        public event EventHandler<FinishPlayingEventArgs> FinishPlaying;
        public event EventHandler<StartPlayingEventArgs> StartPlaying;
        public event EventHandler<PauseEventArgs> PausePlaying;
        public event EventHandler<StopEventArgs> StopPplaying;
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        public event EventHandler<TimeChangedEventArgs> TimeChanged;
        public double ProgressPlay { get; private set; }
        public TimeSpan Time { get; private set; }

        public VLCPlayer()
        {
            p_currentUrl = String.Empty;
            VlcDeployment vd = VlcDeployment.Default;
            if (!vd.CheckVlcLibraryExistence(false, false))
            {
                vd.Install(true);
            }

            VlcMediaLibraryFactory factory = new VlcMediaLibraryFactory(new string[] {
                "--no-snapshot-preview",
                "--ignore-config",
                "--no-osd",
                "--plugin-path", System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "plugins")
            });
            factory.CreateSinglePlayers = true;
            
            p_player = (VlcSinglePlayer)factory.CreatePlayer(new PlayerOutput());
            p_player.Volume = 100;
            List<PlayerEventsReceiver> eventList = new List<PlayerEventsReceiver>();

            p_player.EventsReceivers.Add(this);
        }

        sealed public override void OnTimeChanged()
        {
            if (TimeChanged != null)
            {
                TimeChanged(this, new TimeChangedEventArgs(p_player.Time));
            }
        }

        sealed public override void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, new PositionChangedEventArgs(p_player.Position));
            }
        }

        public override void OnStopped()
        {
            if (StopPplaying != null)
            {
                StopPplaying(this, new StopEventArgs());
            }
        }

        public override void OnEndReached()
        {
            if (FinishPlaying != null)
            {
                FinishPlaying(this, new FinishPlayingEventArgs());
            }
        }

        public void Play(string url)
        {
            if (PreparePlaying != null)
            {
                PreparePlaying(this, new PreparePlayingEventArgs());
            }
            new Thread(() =>
                {
                    MediaInput input = new MediaInput(MediaInputType.NetworkStream, url);
                    p_currentUrl = url;

                    p_player.SetNextMediaInput(input);
                    p_player.PlayNext();

                    if (StartPlaying != null)
                    {
                        StartPlaying(this, new StartPlayingEventArgs(url));
                    }
                }).Start();
        }

        /// <summary>
        /// Задать процент проигрывания песни
        /// </summary>
        /// <param name="percent">Процент проигрывания в диапозоне от 0 до 1</param>
        public void SetPositionPercentage(double percent)
        {
            p_player.Position = (float)percent;
        }

        public void Stop()
        {
            new Thread(() =>
                {
                    p_player.Stop();
                    p_currentUrl = String.Empty;
                });
        }

        public void SetVolume(int vol)
        {
            if (vol < 0) vol = 0;
            if (vol > 200) vol = 200;

            p_player.Volume = Convert.ToInt32(vol);
        }
        
        public void TogglePause()
        {
            new Thread(() =>
            {
                switch (p_player.State)
                {
                    case PlayerState.Playing:
                        p_player.Pause();
                        if (PausePlaying != null)
                        {
                            PausePlaying(this, new PauseEventArgs());
                        }
                        break;
                    case PlayerState.Paused:
                        p_player.Resume();
                        if (StartPlaying != null)
                        {
                            StartPlaying(this, new StartPlayingEventArgs(p_currentUrl));
                        }
                        break;
                    default:
                        break;
                }
            }).Start();
        }
    }
}
