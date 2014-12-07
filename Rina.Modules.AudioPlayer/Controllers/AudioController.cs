using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using Rina.Infastructure.Extensions;
using VKapi;
using VKapi.Models;
using System.Collections.ObjectModel;

namespace Rina.Modules.AudioPlayer.Controllers
{
    [Export(typeof(IAudioController))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AudioController : NotificationObject, IAudioController, IDisposable
    {
        private readonly IAudioService audioService;
        private readonly AudioCommandsProxy commandsProxy;
        private readonly IEventAggregator eventAggregator;
        private readonly IPlayerService playerService;
        private readonly Random random = new Random();
        private readonly Dispatcher uiDispatcher = Application.Current.Dispatcher;
        private readonly ObservableCollection<IAudioListItemViewModel> items;

        private IAudioListItemViewModel currentItem;
        private String textFilter;
        private Boolean isShuffle;
        private Double playProgress;
        private TimeSpan playPosition;
        private Double downloadProgress;
        private Double volume;
        private Boolean isRepeat;
        private Boolean isLoading;
        private CancellationTokenSource audioToken;

        public DelegateCommand MoveNextCommand { get; private set; }
        public DelegateCommand MovePrevCommand { get; private set; }
        public DelegateCommand SwitchTimeProgressCommand { get; private set; }

        public String TextFilter
        {
            get { return this.textFilter; }
            set
            {
                this.textFilter = value;
                uiDispatcher.BeginInvoke(new Action(() =>
                    ItemsView.Filter = (obj) =>
                    {
                        var audio = (IAudioListItemViewModel) obj;
                        return audio == null ? false : audio.Item.ToString().ToUpperInvariant().Contains(TextFilter.ToUpperInvariant());
                    }), DispatcherPriority.Background);

                ItemsView.MoveCurrentTo(this.currentItem);
                RaisePropertyChanged(() => TextFilter);
                MoveNextCommand.RaiseCanExecuteChanged();
                MovePrevCommand.RaiseCanExecuteChanged();
            }
        }

        public ICollectionView ItemsView { get; private set; }

        public Boolean IsShuffle
        {
            get { return this.isShuffle; }
            set
            {
                this.isShuffle = value;
                RaisePropertyChanged(() => IsShuffle);
            }
        }

        public Boolean IsRepeat
        {
            get { return this.isRepeat; }
            set
            {
                this.isRepeat = value;
                RaisePropertyChanged(() => IsRepeat);
            }
        }

        public IAudioListItemViewModel CurrentItem
        {
            get { return this.currentItem; }
            private set
            {
                this.currentItem = value;
                RaisePropertyChanged(() => CurrentItem);
            }
        }

        public Double Volume
        {
            get { return this.volume; }
            set
            {
                this.volume = value;
                this.playerService.SetVolume(this.volume);
            }
        }

        public Double PlayProgress
        {
            get { return this.playProgress; }
            set
            {
                this.playProgress = value;
                this.playerService.SetPosition(this.playProgress);
            }
        }

        public TimeSpan PlayPosition
        {
            get { return this.playPosition; }
            private set
            {
                this.playPosition = value;
                RaisePropertyChanged(() => PlayPosition);
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

        public Boolean IsLoading
        {
            get { return this.isLoading; }
            private set
            {
                this.isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        [ImportingConstructor]
        public AudioController(IAudioService audioService, IEventAggregator eventAggregator,
            IPlayerService playerService, AudioCommandsProxy commandsProxy)
        {
            Contract.Requires(audioService != null);
            Contract.Requires(eventAggregator != null);
            Contract.Requires(commandsProxy != null);
            Contract.Requires(playerService != null);

            this.commandsProxy = commandsProxy;
            this.audioService = audioService;
            this.eventAggregator = eventAggregator;
            this.playerService = playerService;

            this.items = new ObservableCollection<IAudioListItemViewModel>();
            ItemsView = CollectionViewSource.GetDefaultView(this.items);

            this.isShuffle = false;
            this.MoveNextCommand = new DelegateCommand(this.MoveNext, this.ValidateMove);
            this.MovePrevCommand = new DelegateCommand(this.MovePrev, this.ValidateMove);
            this.SwitchTimeProgressCommand = new DelegateCommand(this.SwitchTimeProgress, () => CurrentItem != null);

            Attach();
        }

        private void SwitchTimeProgress()
        {
            this.playerService.IsDecresingProgress = !this.playerService.IsDecresingProgress;
        }

        private void CheckAndCloseAudioToken()
        {
            if (this.audioToken != null && this.audioToken.Token.CanBeCanceled)
            {
                this.audioToken.Cancel();
            }

            this.audioToken = new CancellationTokenSource();
            Debug.Assert(this.audioToken != null);
        }

        public void Dispose()
        {
            if (this.audioToken != null)
            {
                this.audioToken.Dispose();
            }
        }

        public void DownloadAll()
        {
            foreach (var item in ItemsView)
            {
                var cast = (item as IAudioListItemViewModel);
                if (cast != null)
                {
                    cast.DownloadCommand.Execute(null);
                }
            }
        }

        private Int32 MoveListItem(Int32 fromIdx, Int32 toIdx)
        {
            var content = this.items[fromIdx];

            if (fromIdx < toIdx)
            {
                this.items.Insert(toIdx, content);
                this.items.RemoveAt(fromIdx);

                return fromIdx;
            }
            else
            {
                Int32 afterFromIdx = fromIdx + 1;
                if (this.items.Count + 1 > afterFromIdx)
                {
                    this.items.Insert(toIdx, content);
                    this.items.RemoveAt(afterFromIdx);
                }

                return afterFromIdx;
            }
        }

        public async Task<Boolean> MoveItem(IAudioListItemViewModel from, IAudioListItemViewModel to)
        {
            Int32 fromIdx = this.items.IndexOf(from);
            Int32 toIdx = this.items.IndexOf(to);

            Debug.Assert(fromIdx >= 0 && toIdx >= 0);

            Int32 oldFromIdx = MoveListItem(fromIdx, toIdx);

            Boolean canReorder = true;
            var reorderProvider = this.audioService.AudioProvider as IAudioReorderProvider;
            if (reorderProvider != null)
            {
                canReorder = await VKApi.Audio.ReorderAsync(from.Item.Audio.Id, reorderProvider.OwnerId, to.Item.Audio.Id);
            }

            if (!canReorder)
            {
                Int32 newFromIdx = this.items.IndexOf(from);
                MoveListItem(newFromIdx, oldFromIdx);
            }

            return canReorder;
        }

        public async Task RefreshAsync()
        {
            try
            {
                IsLoading = true;
                CheckAndCloseAudioToken();

                await ClearItems();
                await this.audioService.LoadListAsync(AddItemToList, this.audioToken.Token);

                IsLoading = false;

                if (ItemsView.Contains(this.currentItem))
                {
                    ItemsView.MoveCurrentTo(this.currentItem);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Catch OperationCanceledException in AudioController");
            }
        }

        private async Task AddItemToList(VKAudio item)
        {
            var audioWrapper = new AudioListItemModel(item);

            if (this.currentItem != null && this.currentItem.Item == audioWrapper)
            {
                await AddItem(this.currentItem);
            }
            else
            {
                var itemVM = ServiceLocator.Current.GetInstance<IAudioListItemViewModel>();
                itemVM.Item = audioWrapper;
                await AddItem(itemVM);
            }
        }

        private async Task AddItem(IAudioListItemViewModel item)
        {
            await this.uiDispatcher.BeginInvoke(new Action(() => this.items.Add(item)), DispatcherPriority.Input);
        }

        private async Task ClearItems()
        {
            await this.uiDispatcher.BeginInvoke(new Action(() => this.items.Clear()), DispatcherPriority.Input);   
        }

        private void MoveNext()
        {
            if (IsShuffle)
            {
                ItemsView.MoveCurrentToPosition(this.random.Next(0, ItemsView.Cast<Object>().Count()));
            }
            else
            {
                ItemsView.MoveCurrentToNext();

                if (ItemsView.IsCurrentAfterLast)
                {
                    ItemsView.MoveCurrentToFirst();
                }
            }
            ((IAudioListItemViewModel)ItemsView.CurrentItem).PlayCommand.Execute(null);
        }

        private void MovePrev()
        {
            ItemsView.MoveCurrentToPrevious();

            if (ItemsView.IsCurrentBeforeFirst)
            {
                ItemsView.MoveCurrentToLast();
            }

            ((IAudioListItemViewModel)ItemsView.CurrentItem).PlayCommand.Execute(null);
        }

        private Boolean ValidateMove()
        {
            return ItemsView != null && !ItemsView.IsEmpty;
        }

        private void Attach()
        {
            this.eventAggregator.GetEvent<AudioSelectedEvent>().Subscribe(item =>
            {
                CurrentItem = item;
                ItemsView.MoveCurrentTo(item);
                SwitchTimeProgressCommand.RaiseCanExecuteChanged();
                MoveNextCommand.RaiseCanExecuteChanged();
                MovePrevCommand.RaiseCanExecuteChanged();
            });
            this.commandsProxy.MoveNextCommand.RegisterCommand(this.MoveNextCommand);
            this.commandsProxy.MovePrevCommand.RegisterCommand(this.MovePrevCommand);
            this.playerService.Finished += PlayFinished;
            this.playerService.PositionChanged += PositionChanged;
            this.playerService.LoadedChanged += LoadedChanged;
            this.playerService.VolumeChanged += VolumeChanged;
            ShowVolume(this.playerService.Volume);

        }

        private void PlayFinished(object sender, EventArgs e)
        {
            if (IsRepeat)
            {
                PlayProgress = 0;
            }
            else
            {
                if (this.commandsProxy.MoveNextCommand.CanExecute(null))
                {
                    this.commandsProxy.MoveNextCommand.Execute(null);
                }
            }

        }

        private void VolumeChanged(object sender, DataEventArgs<double> e)
        {
            ShowVolume(e.Value);
        }

        private void LoadedChanged(object sender, DataEventArgs<double> e)
        {
            DownloadProgress = e.Value;
        }

        private void PositionChanged(object sender, PositionChangedEventArgs e)
        {
            ShowPlayProgress(e.RelativePositioin);
            PlayPosition = e.Position;
        }

        private void ShowPlayProgress(Double progress)
        {
            this.playProgress = progress;
            RaisePropertyChanged(() => PlayProgress);
        }

        private void ShowVolume(Double value)
        {
            this.volume = value;
            RaisePropertyChanged(() => Volume);
        }
    }
}
