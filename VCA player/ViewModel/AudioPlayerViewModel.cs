using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using VCA_player.Kernel;
using VCA_player.Model;
using VCA_player.Model.List;
using VKapi.Audio;

namespace VCA_player.ViewModel
{
    [ValueConversion(typeof (bool), typeof (bool))]
    public class MyInverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof (bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class AudioPlayerViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly AudioPlayerModel _audioPlayer = new AudioPlayerModel();

        #endregion

        #region Properties

        public int Volume
        {
            get { return _audioPlayer.Volume; }
            set
            {
                _audioPlayer.Volume = value;
                OnPropertyChanged(() => Volume);
            }
        }

        public double Position
        {
            get { return _audioPlayer.Position; }
            set
            {
                _audioPlayer.Position = value;
                OnPropertyChanged(() => Position);
            }
        }

        public bool IsRepeat
        {
            get { return _audioPlayer.IsRepeat; }
            set { _audioPlayer.IsRepeat = value; }
        }

        public bool IsShuffle
        {
            get { return _audioPlayer.IsShuffle; }
            set { _audioPlayer.IsShuffle = value; }
        }

        public bool IsPlaying
        {
            get { return _audioPlayer.IsPlaying; }
        }

        public bool IsLoading
        {
            get { return _audioPlayer.IsLoading; }
        }

        public ObservableCollection<VCAListItem<VKAudio>> Items
        {
            get { return _audioPlayer.Items; }
        }

        public VCAListItem<VKAudio> SelectedItem
        {
            get { return _audioPlayer.SelectedItem; }
            set { _audioPlayer.SelectedItem = value; }
        }

        public string TimeFormat
        {
            get
            {
                if (_audioPlayer.SelectedT == null) return String.Empty;

                int dur = Convert.ToInt32(_audioPlayer.SelectedT.Duration);
                int elp = Convert.ToInt32(_audioPlayer.Time.TotalSeconds);
                int cur = dur - elp;
                int mm = cur/60;
                int ss = Math.Abs(cur%60);

                return "-" + mm + ":" + ((ss < 10) ? "0" : String.Empty) + ss;
            }
        }

        public string SearchFilter
        {
            get { return _audioPlayer.Filter; }
            set
            {
                _audioPlayer.IsLoading = true;
                _audioPlayer.Filter = value;
                _audioPlayer.IsLoading = false;

                OnPropertyChanged(() => SearchFilter);
            }
        }

        #endregion

        #region Commands

        public DelegateCommand PlayNextCommand { get; private set; }
        public DelegateCommand PlayPrevCommand { get; private set; }
        public DelegateCommand TogglePauseCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand<VCAListItem<VKAudio>> PlayAudioCommand { get; private set; }

        #endregion

        #region Public Methods

        public void ShowMyAudios()
        {
            _audioPlayer.SetMyAudios();
        }

        public void SetPlayListGetter(GetPlayListFunc func)
        {
            _audioPlayer.SetListSourceFunc(func);
        }

        #endregion

        #region Constructor

        public AudioPlayerViewModel()
            : base()
        {
            Volume = 30;

            PlayNextCommand = new DelegateCommand(() =>
            {
                _audioPlayer.MoveNext();
                _audioPlayer.Play();
            } /*, () => !IsLoading*/);

            PlayPrevCommand = new DelegateCommand(() =>
            {
                _audioPlayer.MovePrev();
                _audioPlayer.Play();
            } /*, () => !IsLoading*/);

            PlayCommand = new DelegateCommand(() => _audioPlayer.Play()
/*, () => !IsLoading*/);

            StopCommand = new DelegateCommand(() => _audioPlayer.Stop(), () => !IsLoading);

            TogglePauseCommand = new DelegateCommand(() => _audioPlayer.TogglePause()
/*, () => !IsLoading*/);

            RefreshCommand = new DelegateCommand(() => _audioPlayer.Refresh(), () => !IsLoading);
            PlayAudioCommand = new DelegateCommand<VCAListItem<VKAudio>>((audio) =>
            {
                _audioPlayer.SelectedItem = audio;
                _audioPlayer.Play();
            });

            _audioPlayer.TimeChanged += audioPlayer_OnTimeChangedEventHandler;
            _audioPlayer.PositionChanged += audioPlayer_OnPositionChangedEventHandler;
            _audioPlayer.LoadingStateChanged += audioPlayer_OnLoadingStateChanged;
            _audioPlayer.ShuffleStateChanged += audioPlayer_OnShuffleStateChanged;
            _audioPlayer.PlayingStateChanged += audioPlayer_OnPlayingStateChanged;
            _audioPlayer.FinishPlaying += audioPlayer_OnFinishPlaying;
            _audioPlayer.SelectedChanged += audioPlayer_OnSelectedChanged;
        }

        #endregion

        #region Events

        public event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;
        public event EventHandler<PositionChangedEventArgs> OnPositionChanged;

        #endregion

        #region EventHandlers

        private void audioPlayer_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKAudio> e)
        {
            OnPropertyChanged(() => SelectedItem);
        }

        private void audioPlayer_OnFinishPlaying(object sender, EventArgs e)
        {
            if (_audioPlayer.IsRepeat)
            {
                _audioPlayer.Play();
            }
            else
            {
                PlayNextCommand.Execute();
            }
        }

        private void audioPlayer_OnTimeChangedEventHandler(object sender, TimeChangedEventArgs e)
        {
            OnPropertyChanged(() => TimeFormat);
        }

        private void audioPlayer_OnPositionChangedEventHandler(object sender, PositionChangedEventArgs e)
        {
            OnPropertyChanged(() => Position);
            if (OnPositionChanged != null)
            {
                OnPositionChanged(this, e);
            }
        }

        private void audioPlayer_OnPlayingStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsPlaying);
        }

        private void audioPlayer_OnLoadingStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsLoading);

            if (OnLoadingStateChanged != null)
            {
                OnLoadingStateChanged(this, new LoadingStateChangedEventArgs(IsLoading));
            }
        }

        private void audioPlayer_OnShuffleStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(() => IsShuffle);
        }

        #endregion
    }
}