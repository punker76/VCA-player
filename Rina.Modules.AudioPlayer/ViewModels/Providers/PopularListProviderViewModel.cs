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

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class PopularListProviderViewModel : AudioListProviderParametersViewModelBase<PopularParameters>
    {
        private readonly IAudioListProviderParameters<PopularParameters> audioListProviderParameters = new PopularListProvider();

        public override IAudioListProviderParameters<PopularParameters> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override string HeaderInfo
        {
            get { return "Популярное"; }
        }

        [ImportingConstructor]
        public PopularListProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
        }
    }
}
