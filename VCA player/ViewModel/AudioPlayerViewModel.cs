using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using VCA_player;
using VCA_player.Kernel;
using System.ComponentModel.Design;
using System.Windows;
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;
using VCA_player.Model;
using VCA_player.Kernel;
using System.Threading;

namespace VCA_player.ViewModel
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class MyInverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class AudioPlayerViewModel : ViewModelBase
    {
        #region Private Fields
        private AudioPlayerModel audioPlayer = new AudioPlayerModel();
        #endregion

        #region Properties
        public int Volume
        {
            get
            {
                return audioPlayer.Volume;
            }
            set
            {
                audioPlayer.Volume = value;
                OnPropertyChanged(() => Volume);
            }
        }
        public double Position
        {
            get
            {
                return audioPlayer.Position;
            }
            set
            {
                audioPlayer.Position = value;
                OnPropertyChanged(() => Position);
            }
        }
        public bool IsPlaying { get { return audioPlayer.IsPlaying; } }
        public bool IsLoading { get { return audioPlayer.IsLoading; } }
        public ObservableCollection<VCAListItem<VKAudio>> Items { get { return audioPlayer.Items; } }
        public VCAListItem<VKAudio> SelectedItem
        {
            get { return audioPlayer.SelectedItem; }
            set { audioPlayer.SelectedItem = value; }
        }

        public string TimeFormat
        {
            get
            {
                if (audioPlayer.Time == null || audioPlayer.SelectedT == null) return String.Empty;

                int dur = Convert.ToInt32(audioPlayer.SelectedT.Duration);
                int elp = Convert.ToInt32(audioPlayer.Time.TotalSeconds);
                int cur = dur - elp;
                int mm = cur / 60;
                int ss = Math.Abs(cur % 60);

                return "-" + mm.ToString() + ":" + ((ss < 10) ? "0" : String.Empty) + ss.ToString();
            }
        }

        public string SearchFilter
        {
            get
            {
                return audioPlayer.Filter;
            }
            set
            {
                audioPlayer.IsLoading = true;
                audioPlayer.Filter = value;
                audioPlayer.IsLoading = false;

                OnPropertyChanged(() => SearchFilter);
            }
        }
        #endregion

        #region Commands
        public DelegateCommand PlayNextCommand      { get; private set; }
        public DelegateCommand PlayPrevCommand      { get; private set; }
        public DelegateCommand TogglePauseCommand   { get; private set; }
        public DelegateCommand PlayCommand          { get; private set; }
        public DelegateCommand StopCommand          { get; private set; }
        public DelegateCommand RefreshCommand       { get; private set; }
        #endregion
        
        #region Public Methods
        public void ShowMyAudios()
        {
            audioPlayer.SetMyAudios();
        }
        public void SetPlayListGetter(GetPlayListFunc func)
        {
            audioPlayer.SetListSourceFunc(func);
        }
        #endregion

        #region Constructor
        public AudioPlayerViewModel()
            : base()
        {
            Volume = 30;

            this.PlayNextCommand = new DelegateCommand(() =>
            {
                audioPlayer.MoveNext();
                /*Application.Current.Dispatcher.BeginInvoke((ThreadStart)delegate()
                    {
                        CollViewSource.View.MoveCurrentToNext();
                        if (CollViewSource.View.IsCurrentAfterLast)
                            CollViewSource.View.MoveCurrentToFirst();

                        audioPlayer.PlayList.SelectedAudio = CollViewSource.View.CurrentItem as VKAudio;
                        audioPlayer.Play();
                    });*/
            }/*, () => !IsLoading*/);

            this.PlayPrevCommand = new DelegateCommand(() =>
            {
                audioPlayer.MovePrev();
               /* Application.Current.Dispatcher.BeginInvoke((ThreadStart)delegate()
                    {
                        CollViewSource.View.MoveCurrentToPrevious();
                        if (CollViewSource.View.IsCurrentBeforeFirst)
                            CollViewSource.View.MoveCurrentToLast();

                        audioPlayer.PlayList.SelectedAudio = CollViewSource.View.CurrentItem as VKAudio;
                        audioPlayer.Play();
                    });*/
            }/*, () => !IsLoading*/);

            this.PlayCommand = new DelegateCommand(() =>
            {
                audioPlayer.Play();
            }/*, () => !IsLoading*/);

            this.StopCommand = new DelegateCommand(() =>
            {
                audioPlayer.Stop();
            }, () => !IsLoading);

            this.TogglePauseCommand = new DelegateCommand(() =>
            {
                audioPlayer.TogglePause();
            }/*, () => !IsLoading*/);

            this.RefreshCommand = new DelegateCommand(() =>
            {
                audioPlayer.Refresh();
            }, () => !IsLoading);

            audioPlayer.OnTimeChanged           += audioPlayer_OnTimeChangedEventHandler;
            audioPlayer.OnPositionChanged       += audioPlayer_OnPositionChangedEventHandler;
            audioPlayer.OnSelectedChanged       += audioPlayer_OnSelectedChanged;
            audioPlayer.OnLoadingStateChanged   += audioPlayer_OnLoadingStateChanged;
            audioPlayer.OnPlayingStateChanged   += audioPlayer_OnPlayingStateChanged;
            audioPlayer.OnFinishPlaying         += audioPlayer_OnFinishPlaying;
        }
        #endregion

        #region Events
        public event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;
        #endregion

        #region EventHandlers
        void audioPlayer_OnFinishPlaying(object sender, Kernel.FinishPlayingEventArgs e)
        {
            PlayNextCommand.Execute();
        }
        void audioPlayer_OnTimeChangedEventHandler(object sender, TimeChangedEventArgs e)
        {
            OnPropertyChanged(() => TimeFormat);
        }

        void audioPlayer_OnPositionChangedEventHandler(object sender, PositionChangedEventArgs e)
        {
            OnPropertyChanged(() => Position);
        }

        void audioPlayer_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKAudio> e)
        {
            PlayCommand.Execute();
            OnPropertyChanged(() => Items);
            OnPropertyChanged(() => SelectedItem);
            OnPropertyChanged("IsSelected");
            OnPropertyChanged("SelectedItem.IsSelected");
        }

        void audioPlayer_OnPlayingStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsPlaying);
        }

        void audioPlayer_OnLoadingStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsLoading);

            if (OnLoadingStateChanged != null)
            {
                OnLoadingStateChanged(this, new LoadingStateChangedEventArgs(IsLoading));
            }
        }
        #endregion
    }
}
