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
    public abstract class AudioListProviderParametersViewModelBase<TParam> : AudioListProviderViewModelBase
        where TParam : INotifyPropertyChanged, new()
    {
        public TParam State { get; private set; }
        public abstract IAudioListProviderParameters<TParam> AudioListProviderParameters { get; }
        public override IAudioListProvider AudioListProvider
        {
            get { return AudioListProviderParameters; }
        }

        protected AudioListProviderParametersViewModelBase(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            if (this.AudioListProviderParameters.State == null)
            {
                State = new TParam();
                this.AudioListProviderParameters.State = State;
            }
            else
            {
                State = this.AudioListProviderParameters.State;
            }
            State.PropertyChanged += OnStatePropertyChanged;
        }

        protected override async Task SetProvider()
        {
            AudioService.AudioProvider = this.AudioListProvider;
            if (AudioListProviderParameters.IsStateSet)
            {
                await AudioController.RefreshAsync();
            }
        }

        protected virtual async void OnStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.AudioListProviderParameters.Update())
            {
                await AudioController.RefreshAsync();
            }
        }
    }
}