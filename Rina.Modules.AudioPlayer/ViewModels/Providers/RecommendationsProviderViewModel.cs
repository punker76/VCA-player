using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Rina.Infastructure.Interfaces;
using Rina.Modules.AudioPlayer.Services;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics.Contracts;
using Rina.Modules.AudioPlayer.Models;
using VKapi.Models;
using Rina.Infastructure.Models;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class RecommendationsProviderViewModel : AudioListProviderParametersViewModelBase<NotificationWrapper<VKAudio>>
    {
        private readonly IAudioListProviderParameters<NotificationWrapper<VKAudio>> audioListProviderParameters = new RecommendationsProvider();

        public override IAudioListProviderParameters<NotificationWrapper<VKAudio>> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override string HeaderInfo
        {
            get { return "Рекоммендации"; }
        }

        [ImportingConstructor]
        public RecommendationsProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
        }
    }
}
