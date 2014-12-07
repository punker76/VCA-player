using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using VKapi;
using VKapi.Models;
using System.Threading.Tasks;

namespace Rina.Modules.AudioPlayer.ViewModels
{
    [Export(typeof(IAudioListItemViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AudioListItemViewModel : NotificationObject, IAudioListItemViewModel
    {
        private readonly IPlayerService playerService;
        private readonly IEventAggregator eventAggregator;
        private readonly AudioCommandsProxy commandProxy;
        private readonly IDownloadManagerService downloadService;

        private AudioListItemModel item;
        private TimeSpan playProgress;
        private Double downloadProgress;
        private PlayState playState;
        private LyricsViewModel lyricsView;
        private Boolean showLyrics;
        private Boolean isDeleted;
        private Boolean isAdded;

        public ICommand PlayCommand { get; private set; }
        public ICommand ChangeStateCommand { get; private set; }
        public ICommand SwitchLyricsStateCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand RestoreCommand { get; private set; }
        public ICommand DownloadCommand { get; private set; }

        #region Properties

        public Boolean ShowLyrics
        {
            get { return this.showLyrics; }
            private set
            {
                this.showLyrics = value;
                RaisePropertyChanged(() => ShowLyrics);
            }
        }
        public LyricsViewModel LyricsView
        {
            get { return this.lyricsView; }
            private set
            {
                this.lyricsView = value;
                RaisePropertyChanged(() => LyricsView);
            }
        }
        public PlayState State
        {
            get { return this.playState; }
            private set
            {
                this.playState = value;
                RaisePropertyChanged(() => State);
            }
        }

        public Boolean IsDeleted
        {
            get { return this.isDeleted; }
            private set
            {
                this.isDeleted = value;
                RaisePropertyChanged(() => IsDeleted);
            }
        }

        public Boolean IsAdded
        {
            get { return this.isAdded; }
            private set
            {
                this.isAdded = value;
                RaisePropertyChanged(() => IsAdded);
            }
        }

        public AudioListItemModel Item
        {
            get { return this.item; }
            set
            {
                Debug.Assert(this.item == null);
                this.item = value;
                RaisePropertyChanged(() => Item);
                LyricsView = new LyricsViewModel(this.item.Audio);
            }
        }

        public Double DownloadProgress
        {
            get { return this.downloadProgress; }
            private set
            {
                this.downloadProgress = value;
                RaisePropertyChanged(() => DownloadProgress);
            }
        }

        public TimeSpan PlayProgress
        {
            get { return playProgress; }
            private set
            {
                this.playProgress = value;
                RaisePropertyChanged(() => PlayProgress);
            }
        }

        public Boolean IsOwnAudio
        {
            get { return VKapi.VKSession.Instance.UserId == Item.Audio.OwnerId; }
        }

        #endregion

        [ImportingConstructor]
        public AudioListItemViewModel(
            IPlayerService playerService,
            IEventAggregator eventAggregator,
            IDownloadManagerService downloadService,
            AudioCommandsProxy commandProxy)
        {
            Contract.Requires(playerService != null);
            Contract.Requires(eventAggregator != null);
            Contract.Requires(commandProxy != null);
            Contract.Requires(downloadService != null);

            this.playerService = playerService;
            this.eventAggregator = eventAggregator;
            this.commandProxy = commandProxy;
            this.downloadService = downloadService;

            State = PlayState.Stop;
            DownloadProgress = 0;

            SwitchLyricsStateCommand = new DelegateCommand(this.SwitchLyricsState);
            PlayCommand = new DelegateCommand(this.Play);
            ChangeStateCommand = new DelegateCommand(this.ChangeState);
            AddCommand = new DelegateCommand(async () => await this.Add());
            DeleteCommand = new DelegateCommand(async () => await this.Delete());
            RestoreCommand = new DelegateCommand(async () => await this.Restore());
            DownloadCommand = new DelegateCommand(this.StartDownload);

            this.eventAggregator.GetEvent<AudioSelectedEvent>().Subscribe(
                audio =>
                {
                    State = PlayState.Stop;
                    Deattach();
                }, ThreadOption.UIThread, false, audio => audio != this && State != PlayState.Stop);
        }

        private async void SwitchLyricsState()
        {
            ShowLyrics = !ShowLyrics;

            if (ShowLyrics)
            {
                await LyricsView.Show();
            }
        }

        private void StartDownload()
        {
            this.downloadService.Start(Item.Audio.Url, Item.ToString(), DownloadType.Music);
        }

        private async Task Add()
        {
            Int64 result = await VKApi.Audio.AddAsync(Item.Audio.Id, Item.Audio.OwnerId);
            if (result != -1)
            {
                IsAdded = true;
            }
        }

        private async Task Delete()
        {
            Boolean result = await VKApi.Audio.DeleteAsync(Item.Audio.Id, Item.Audio.OwnerId);
            if (result)
            {
                IsDeleted = true;
            }
        }

        private async Task Restore()
        {
            VKAudio restored = await VKApi.Audio.RestoreAsync(Item.Audio.Id, Item.Audio.OwnerId);
            if (restored != null)
            {
                Item.Audio.Url = restored.Url;
                Item.Audio.Id = restored.Id;
                Item.Audio.OwnerId = restored.OwnerId;
                IsDeleted = false;
            }
        }

        private void ChangeState()
        {
            Debug.Assert(this.playState != PlayState.Stop);

            switch (this.playState)
            {
                case PlayState.Play:
                    this.playerService.Pause();
                    break;
                case PlayState.Pause:
                    this.playerService.Play();
                    break;
            }
        }

        private void Play()
        {
            State = PlayState.Loading;
            eventAggregator.GetEvent<AudioSelectedEvent>().Publish(this);
            this.playerService.Play(item.Uri);
            Attach();
        }

        private void Paused(object sender, EventArgs e)
        {
            State = PlayState.Pause;
        }

        private void Stoped(object sender, EventArgs e)
        {
            State = PlayState.Stop;
            Deattach();
        }

        private void Started(object sender, EventArgs e)
        {
            State = PlayState.Play;
        }

        private void LoadedChanged(object sender, DataEventArgs<Double> e)
        {
            DownloadProgress = e.Value;
        }

        private void PositionChanged(object sender, PositionChangedEventArgs e)
        {
            PlayProgress = e.Position;
        }

        private void Attach()
        {
            this.playerService.PositionChanged += PositionChanged;
            this.playerService.LoadedChanged += LoadedChanged;
            this.playerService.Started += Started;
            this.playerService.Stoped += Stoped;
            this.playerService.Paused += Paused;
            if (!this.commandProxy.ChangeStateCommand.RegisteredCommands.Contains(this.ChangeStateCommand))
            {
                this.commandProxy.ChangeStateCommand.RegisterCommand(this.ChangeStateCommand);
            }
        }

        private void Deattach()
        {
            this.playerService.PositionChanged -= PositionChanged;
            this.playerService.LoadedChanged -= LoadedChanged;
            this.playerService.Started -= Started;
            this.playerService.Stoped -= Stoped;
            this.playerService.Paused -= Paused;
            this.commandProxy.ChangeStateCommand.UnregisterCommand(this.ChangeStateCommand);
        }
    }
}
