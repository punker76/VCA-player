using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using DZ.MediaPlayer;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.Internal;
/*
using DZ.MediaPlayer;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.WindowsForms;*/

namespace VCA_player.Kernel
{
   /* public abstract class NAudioPlayer: IKernelPlayer
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();

        private string urlNextAudio;

        private WaveStream blockAlignedStream;
        private DirectSoundOut currentSoundOut;
        private string currentUrl;
        private float p_lastVolume;

        public event EventHandler<PreparePlayingEventArgs> OnPreparePlayingEventHandler;
        public event EventHandler<FinishPlayingEventArgs> OnFinishPlayingEventHandler;
        public event EventHandler<StartPlayingEventArgs> OnStartPlayingEventHandler;
        public event EventHandler<PauseEventArgs> OnPauseEventHandler;
        public event EventHandler<StopEventArgs> OnStopEventHandler;
        public event EventHandler<PositionChangedEventArgs> OnPositionChangedEventHandler;
        public event EventHandler<TimeChangedEventArgs> OnTimeChangedEventHandler;
        public double ProgressPlay { get; private set; }
        public TimeSpan Time { get; private set; }

        public NAudioPlayer()
        {
            urlNextAudio = String.Empty;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            p_lastVolume = 1.0f;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (OnPositionChangedEventHandler != null)
            {
                OnPositionChangedEventHandler(sender, new PositionChangedEventArgs(e.ProgressPercentage / 10000.0));
            }
        }

        public void KernelPlay(string url)
        {
            if (OnPreparePlayingEventHandler != null)
            {
                OnPreparePlayingEventHandler(this, new PreparePlayingEventArgs());
            }

            if (worker.IsBusy)
            {
                worker.CancelAsync();
                urlNextAudio = url;
            }
            else
            {
                worker.RunWorkerAsync(url);
            }
        }

        /// <summary>
        /// Задать процент проигрывания песни
        /// </summary>
        /// <param name="percent">Процент проигрывания в диапозоне от 0 до 1</param>
        public void KernelSetPositionPercentage(double percent)
        {
            if (blockAlignedStream == null) return;

            long pos = (long)(percent * blockAlignedStream.Length);

            blockAlignedStream.Position = (long)(pos - (pos % blockAlignedStream.BlockAlign));
        }

        public void KernelStop()
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        public void KernelSetVolume(int vol)
        {
            if (currentSoundOut == null) return;

            if (vol < 0) vol = 0;
            if (vol > 100) vol = 100;
            p_lastVolume = vol / 100.0f;
            currentSoundOut.Volume = vol / 100.0f;
        }

        public void KernelPause()
        {
            if (currentSoundOut == null) return;

            currentSoundOut.Pause();

            if (OnPauseEventHandler != null)
            {
                OnPauseEventHandler(this, new PauseEventArgs());
            }
        }

        public void KernelUnpause()
        {
            if (currentSoundOut == null) return;

            currentSoundOut.Play();
            if (OnStartPlayingEventHandler != null)
            {
                OnStartPlayingEventHandler(this, new StartPlayingEventArgs(currentUrl));
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(urlNextAudio))
            {
                worker.RunWorkerAsync(urlNextAudio);
                urlNextAudio = "";
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string url = (string)e.Argument;

                using (Stream ms = new MemoryStream())
                {
                    using (Stream stream = WebRequest.Create(url)
                        .GetResponse().GetResponseStream())
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                    }

                    ms.Position = 0;
                    using (blockAlignedStream =
                        new BlockAlignReductionStream(
                            WaveFormatConversionStream.CreatePcmStream(
                                new Mp3FileReader(ms))))
                    {
                        using (currentSoundOut = new DirectSoundOut())
                        {
                            
                            currentSoundOut.Volume = p_lastVolume;
                            ProgressPlay = 0;
                            currentUrl = url;
                            if (OnStartPlayingEventHandler != null)
                            {
                                OnStartPlayingEventHandler(this, new StartPlayingEventArgs(url));
                            }

                            currentSoundOut.Init(blockAlignedStream);
                            currentSoundOut.Play();

                            while (currentSoundOut.PlaybackState != PlaybackState.Stopped)
                            {
                                worker.ReportProgress(Convert.ToInt32(blockAlignedStream.Position * 10000 / blockAlignedStream.Length));

                                if (worker.CancellationPending)
                                {
                                    currentSoundOut.Stop();
                                    e.Cancel = true;

                                    if (OnStopEventHandler != null)
                                    {
                                        OnStopEventHandler(this, new StopEventArgs());
                                    }
                                    return;
                                }
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }


                if (OnFinishPlayingEventHandler != null)
                {
                    OnFinishPlayingEventHandler(this, new FinishPlayingEventArgs());
                }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message, "Ошибка при загрузке файла", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                e.Cancel = true;
                return;
            }
            finally
            {
                blockAlignedStream = null;
                currentSoundOut = null;
                currentUrl = "";
            }
        }
    }*/
}
