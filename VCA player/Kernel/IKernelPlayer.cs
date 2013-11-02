using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCA_player.Kernel
{
    public class StopEventArgs : EventArgs
    {
        public StopEventArgs() { }
    }

    public class PauseEventArgs : EventArgs
    {
        public PauseEventArgs() { }
    }

    public class FinishPlayingEventArgs : EventArgs
    {
        public FinishPlayingEventArgs() { }
    }

    public class PreparePlayingEventArgs : EventArgs
    {
        public PreparePlayingEventArgs() { }
    }

    public class StartPlayingEventArgs : EventArgs
    {
        public string Url { get; private set; }

        public StartPlayingEventArgs(string url)
        {
            Url = url;
        }
    }

    public class PositionChangedEventArgs : EventArgs
    {
        public double Position { get; private set; }

        public PositionChangedEventArgs(double position)
        {
            Position = position;
        }
    }

    public class TimeChangedEventArgs : EventArgs
    {
        public TimeSpan Time { get; private set; }

        public TimeChangedEventArgs(TimeSpan time)
        {
            Time = time;
        }
    }

    public class LoadingStateChangedEventArgs : EventArgs
    {
        public bool Loading { get; private set; }

        public LoadingStateChangedEventArgs(bool loading) { Loading = loading; }
    }

    interface IKernelPlayer
    {
        event EventHandler<PreparePlayingEventArgs> PreparePlaying;
        event EventHandler<FinishPlayingEventArgs> FinishPlaying;
        event EventHandler<StartPlayingEventArgs> StartPlaying;
        event EventHandler<PauseEventArgs> PausePlaying;
        event EventHandler<StopEventArgs> StopPplaying;
        event EventHandler<PositionChangedEventArgs> PositionChanged;
        event EventHandler<TimeChangedEventArgs> TimeChanged;
        double ProgressPlay { get; }
        TimeSpan Time { get; }

        void Play(string url);
        void SetPositionPercentage(double percent);
        void Stop();
        void SetVolume(int vol);
        void TogglePause();
    }
}
