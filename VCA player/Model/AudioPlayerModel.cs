using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using VCA_player;
using VCA_player.Kernel;
using System.ComponentModel.Design;
using System.Windows.Input;
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;

namespace VCA_player.Model
{
    class AudioPlayerModel
    {
        #region PrivateFiels
        private IKernelPlayer _player = new VLCPlayer();
        private int _volume;
        private double _position;
        private bool _isLoading;
        private bool _isPlaying;
        private bool _isShuffle;
        private VCAAudioList _audioList = new VCAAudioList();
        #endregion

        #region Events
        public event EventHandler<PreparePlayingEventArgs> OnPreparePlaying;
        public event EventHandler<FinishPlayingEventArgs> OnFinishPlaying;
        public event EventHandler<StartPlayingEventArgs> OnStartPlaying;
        public event EventHandler<PauseEventArgs> OnPause;
        public event EventHandler<StopEventArgs> OnStop;
        public event EventHandler<PositionChangedEventArgs> OnPositionChanged;
        public event EventHandler<TimeChangedEventArgs> OnTimeChanged;
        public event EventHandler<SelectedChangedEventArgs<VKAudio>> OnSelectedChanged;
        public event EventHandler OnLoadingStateChanged;
        public event EventHandler OnPlayingStateChanged;
        public event EventHandler OnShuffleStateChanged;
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
                if (OnShuffleStateChanged != null) { OnShuffleStateChanged(this, new EventArgs()); }
                if (_isShuffle) { _audioList.ShuffleItems(); }
                else { _audioList.RestoreItems(); }
            }
        }
        public bool IsRepeat { get; set; }
        public bool IsPlaying
        {
            get { return _isPlaying; }
            private set
            {
                _isPlaying = value;
                if (OnPlayingStateChanged != null)
                {
                    OnPlayingStateChanged(this, new EventArgs());
                }
            }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                if (OnLoadingStateChanged != null)
                {
                    OnLoadingStateChanged(this, new EventArgs());
                }
            }
        }
        public VKAudio SelectedT { get { return _audioList.SelectedT; } }
        public VCAListItem<VKAudio> SelectedItem
        {
            get { return _audioList.SelectedItem; }
            set { _audioList.SelectedItem = value; }
        }

        public string Filter { get { return _audioList.Filter; } set { _audioList.Filter = value; } }
        public TimeSpan Time { get; private set; }
        public ObservableCollection<VCAListItem<VKAudio>> Items { get { return _audioList.Items; } }
        #endregion

        #region Constructor
        public AudioPlayerModel()
        {
            _player.FinishPlaying += _player_OnFinishPlayingEventHandler;
            _player.PausePlaying += _player_OnPauseEventHandler;
            _player.PositionChanged += _player_OnPositionChangedEventHandler;
            _player.PreparePlaying += _player_OnPreparePlayingEventHandler;
            _player.StartPlaying += _player_OnStartPlayingEventHandler;
            _player.StopPplaying += _player_OnStopEventHandler;
            _player.TimeChanged += _player_OnTimeChangedEventHandler;
            _audioList.OnSelectedChanged += _audioList_OnSelectedChanged;
            _audioList.OnStartRefreshList += PlayList_OnStartRefreshPlayList;
            _audioList.OnFinishRefreshList += PlayList_OnFinishRefreshPlayList;
        }
        #endregion

        #region EventHandlers
        void _player_OnPreparePlayingEventHandler(object sender, PreparePlayingEventArgs e)
        {
            IsLoading = true;
            if (OnPreparePlaying != null)
            {
                OnPreparePlaying(this, e);
            }
        }

        void _player_OnPositionChangedEventHandler(object sender, PositionChangedEventArgs e)
        {
            _position = e.Position;
            if (OnPositionChanged != null)
            {
                OnPositionChanged(this, e);
            }
        }

        void _player_OnPauseEventHandler(object sender, PauseEventArgs e)
        {
            IsPlaying = false;
            if (OnPause != null)
            {
                OnPause(this, e);
            }
        }

        void _player_OnFinishPlayingEventHandler(object sender, FinishPlayingEventArgs e)
        {
            IsPlaying = false;
            if (OnFinishPlaying != null)
            {
                OnFinishPlaying(this, e);
            }
        }

        void _player_OnTimeChangedEventHandler(object sender, TimeChangedEventArgs e)
        {
            Time = e.Time;
            if (OnTimeChanged != null)
            {
                OnTimeChanged(this, e);
            }
        }

        void _player_OnStopEventHandler(object sender, StopEventArgs e)
        {
            IsPlaying = false;
            if (OnStop != null)
            {
                OnStop(this, e);
            }
        }

        void _player_OnStartPlayingEventHandler(object sender, StartPlayingEventArgs e)
        {
            IsLoading = false;
            IsPlaying = true;
            if (OnStartPlaying != null)
            {
                OnStartPlaying(this, e);
            }
        }

        void PlayList_OnFinishRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        void PlayList_OnStartRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = true;
        }

        void _audioList_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKAudio> e)
        {
            if (OnSelectedChanged != null && !IsLoading)
            {
                OnSelectedChanged(this, e);
            }
        }
        #endregion

        #region Methods
        public void MoveNext()
        {
            int idx = _audioList.Items.IndexOf(_audioList.SelectedItem);
            if (idx < 0) { return; }
            idx++;
            if (idx >= _audioList.Items.Count) idx = 0;
            if (idx >= _audioList.Items.Count || idx < 0) return;

            _audioList.SelectedItem = _audioList.Items[idx];
        }
        public void MovePrev()
        {
            int idx = _audioList.Items.IndexOf(_audioList.SelectedItem);
            if (idx < 0) { return; }
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
