using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;
using Rina.Infastructure.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Data;
using Rina.Infastructure.Extensions;
using System.Windows.Threading;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    public abstract class AudioListProviderItemsViewModelBase<TItem, TParam> : AudioListProviderParametersViewModelBase<TParam>
        where TParam : INotifyPropertyChanged, new()
    {
        private ObservableCollection<TItem> items;
        private Boolean isLoading;
        
        public Boolean IsLoading
        {
            get { return this.isLoading; }
            private set
            {
                this.isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public ICollectionView ItemsView { get; private set; }

        protected AudioListProviderItemsViewModelBase(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            IsActiveChanged += FilterIsActiveChanged;

            this.items = new ObservableCollection<TItem>();
            ItemsView = CollectionViewSource.GetDefaultView(this.items);
        }

        protected abstract Task<IEnumerable<TItem>> RefreshCore();

        public async Task Refresh()
        {
            IsLoading = true;

            var list = await RefreshCore();

            IsLoading = false;

            await ClearItems();

            foreach (var item in list)
            {
                await AddItem(item);
            }
        }

        protected async Task AddItem(TItem item)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => this.items.Add(item)),
                DispatcherPriority.Render);
        }

        protected async Task ClearItems()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => this.items.Clear()),
                DispatcherPriority.Render);
        }

        private async void FilterIsActiveChanged(object sender, EventArgs e)
        {
            if (IsActive && this.items.Count == 0)
            {
                await Refresh();
            }
        }
    }
}
