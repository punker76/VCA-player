using System;

namespace VCA_player.Kernel
{
    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(double position)
        {
            Position = position;
        }

        public double Position { get; private set; }
    }

    public class TimeChangedEventArgs : EventArgs
    {
        public TimeChangedEventArgs(TimeSpan time)
        {
            Time = time;
        }

        public TimeSpan Time { get; private set; }
    }

    public class LoadingStateChangedEventArgs : EventArgs
    {
        public LoadingStateChangedEventArgs(bool loading)
        {
            Loading = loading;
        }

        public bool Loading { get; private set; }
    }

    internal interface IKernelPlayer
    {
        event EventHandler PreparePlaying;
        event EventHandler FinishPlaying;
        event EventHandler StartPlaying;
        event EventHandler PausePlaying;
        event EventHandler StopPlaying;
        event EventHandler<PositionChangedEventArgs> PositionChanged;
        event EventHandler<TimeChangedEventArgs> TimeChanged;

        void Play(string url);
        void SetPositionPercentage(double percent);
        void Stop();
        void SetVolume(int vol);
        void TogglePause();
    }
}