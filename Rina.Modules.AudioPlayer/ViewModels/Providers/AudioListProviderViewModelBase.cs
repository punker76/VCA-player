using System.ComponentModel;
using Microsoft.Practices.Prism;
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
using Rina.Infastructure.Behaviors;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    public abstract class AudioListProviderViewModelBase : ActiveAwareNotificationObject, IHederInfoProvider<String>
    {
        private readonly IAudioService audioService;
        private readonly IAudioController audioController;
        private readonly Dispatcher uiDispatcher;
        
        protected Dispatcher UiDispatcher
        {
            get { return this.uiDispatcher; }
        }

        protected IAudioService AudioService
        {
            get { return this.audioService; }
        }

        protected IAudioController AudioController
        {
            get { return this.audioController; }
        }

        public abstract IAudioListProvider AudioListProvider { get; }

        public abstract String HeaderInfo { get; }

        protected AudioListProviderViewModelBase(IAudioService audioService, IAudioController audioController)
        {
            Contract.Requires(audioService != null);
            Contract.Requires(audioController != null);

            this.audioService = audioService;
            this.audioController = audioController;
            
            IsActiveChanged += FilterIsActiveChanged;

            this.uiDispatcher = Application.Current.Dispatcher;
        }

        protected virtual async Task SetProvider()
        {
            this.audioService.AudioProvider = this.AudioListProvider;
            await AudioController.RefreshAsync();
        }

        private async void FilterIsActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                await SetProvider();
            }
        }
    }
}
