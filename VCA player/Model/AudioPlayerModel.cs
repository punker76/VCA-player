using System;
using System.Collections.ObjectModel;
using VCA_player.Kernel;
using VCA_player.Model.List;
using VKapi.Audio;

namespace VCA_player.Model
{
    internal class AudioPlayerModel
    {
        #region PrivateFiels

        private readonly VCAAudioList _audioList = new VCAAudioList();
        private readonly IKernelPlayer _player = new StandartMediaPlayer();
        private bool _isLoading;
        private bool _isPlaying;
        private bool _isShuffle;
        private double _position;
        private int _volume;

        #endregion

        #region Events

        public event EventHandler PreparePlaying;
        public event EventHandler FinishPlaying;
        public event EventHandler StartPlaying;
        public event EventHandler Pause;
        public event EventHandler OnStop;
        public event EventHandler<PositionChangedEventArgs> PositionChanged;
        public event EventHandler<TimeChangedEventArgs> TimeChanged;
        public event EventHandler<SelectedChangedEventArgs<VKAudio>> SelectedChanged;
        public event EventHandler LoadingStateChanged;
        public event EventHandler PlayingStateChanged;
        public event EventHandler ShuffleStateChanged;

        #endregion

        #region Properties

        public int Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                _player.SetVolume(_volume);
            }
        }

        public double Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _player.SetPositionPercentage(_position);
            }
        }

        public bool IsShuffle
        {
            get { return _isShuffle; }
            set
            {
                _isShuffle = value;
                if (ShuffleStateChanged != null)
                {
                    ShuffleStateChanged(this, new EventArgs());
                }
                if (_isShuffle)
                {
                    _audioList.ShuffleItems();
                }
                else
                {
                    _audioList.RestoreItems();
                }
            }
        }

        public bool IsRepeat { get; set; }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            private set
            {
                _isPlaying = value;
                if (PlayingStateChanged != null)
                {
                    PlayingStateChanged(this, new EventArgs());
                }
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                if (LoadingStateChanged != null)
                {
                    LoadingStateChanged(this, new EventArgs());
                }
            }
        }

        public VKAudio SelectedT
        {
            get { return _audioList.SelectedT; }
        }

        public VCAListItem<VKAudio> SelectedItem
        {
            get { return _audioList.SelectedItem; }
            set { _audioList.SelectedItem = value; }
        }

        public string Filter
        {
            get { return _audioList.Filter; }
            set { _audioList.Filter = value; }
        }

        public TimeSpan Time { get; private set; }

        public ObservableCollection<VCAListItem<VKAudio>> Items
        {
            get { return _audioList.Items; }
        }

        #endregion

        #region Constructor

        public AudioPlayerModel()
        {
            _player.FinishPlaying += _player_OnFinishPlayingEventHandler;
            _player.PausePlaying += _player_OnPauseEventHandler;
            _player.PositionChanged += _player_OnPositionChangedEventHandler;
            _player.PreparePlaying += _player_OnPreparePlayingEventHandler;
            _player.StartPlaying += _player_OnStartPlayingEventHandler;
            _player.StopPlaying += _player_OnStopEventHandler;
            _player.TimeChanged += _player_OnTimeChangedEventHandler;
            _audioList.OnSelectedChanged += _audioList_OnSelectedChanged;
            _audioList.OnStartRefreshList += PlayList_OnStartRefreshPlayList;
            _audioList.OnFinishRefreshList += PlayList_OnFinishRefreshPlayList;
        }

        #endregion

        #region EventHandlers

        private void _player_OnPreparePlayingEventHandler(object sender, EventArgs e)
        {
            IsLoading = true;
            if (PreparePlaying != null)
            {
                PreparePlaying(this, e);
            }
        }

        private void _player_OnPositionChangedEventHandler(object sender, PositionChangedEventArgs e)
        {
            _position = e.Position;
            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        private void _player_OnPauseEventHandler(object sender, EventArgs e)
        {
            IsPlaying = false;
            if (Pause != null)
            {
                Pause(this, e);
            }
        }

        private void _player_OnFinishPlayingEventHandler(object sender, EventArgs e)
        {
            IsPlaying = false;
            if (FinishPlaying != null)
            {
                FinishPlaying(this, e);
            }
        }

        private void _player_OnTimeChangedEventHandler(object sender, TimeChangedEventArgs e)
        {
            Time = e.Time;
            if (TimeChanged != null)
            {
                TimeChanged(this, e);
            }
        }

        private void _player_OnStopEventHandler(object sender, EventArgs e)
        {
            IsPlaying = false;
            if (OnStop != null)
            {
                OnStop(this, e);
            }
        }

        private void _player_OnStartPlayingEventHandler(object sender, EventArgs e)
        {
            IsLoading = false;
            IsPlaying = true;
            if (StartPlaying != null)
            {
                StartPlaying(this, e);
            }
        }

        private void PlayList_OnFinishRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        private void PlayList_OnStartRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = true;
        }

        private void _audioList_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKAudio> e)
        {
            if (SelectedChanged != null && !IsLoading)
            {
                SelectedChanged(this, e);
            }
        }

        #endregion

        #region Methods

        public void MoveNext()
        {
            int idx = _audioList.Items.IndexOf(_audioList.SelectedItem);
            if (idx < 0)
            {
                return;
            }
            idx++;
            if (idx >= _audioList.Items.Count) idx = 0;
            if (idx >= _audioList.Items.Count || idx < 0) return;

            _audioList.SelectedItem = _audioList.Items[idx];
        }

        public void MovePrev()
        {
            int idx = _audioList.Items.IndexOf(_audioList.SelectedItem);
            if (idx < 0)
            {
                return;
            }
            idx--;
            if (idx < 0) idx = _audioList.Items.Count - 1;
            if (idx >= _audioList.Items.Count || idx < 0) return;

            _audioList.SelectedItem = _audioList.Items[idx];
        }

        public void Play()
        {
            if (_audioList.SelectedItem == null) return;

            _player.Play(_audioList.SelectedItem.Item.Url);
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void TogglePause()
        {
            _player.TogglePause();
        }

        public void SetMyAudios()
        {
            _audioList.GetPlayList = AudioGetPlayLists.GetFriendPlayList;
            _audioList.Refresh();
        }

        public void Refresh()
        {
            IsShuffle = false;
            _audioList.Refresh();
        }

        public void SetListSourceFunc(GetPlayListFunc func)
        {
            _audioList.GetPlayList = func;
        }

        #endregion
    }
}