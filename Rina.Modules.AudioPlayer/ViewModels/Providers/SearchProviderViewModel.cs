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
    public sealed class SearchProviderViewModel : AudioListProviderParametersViewModelBase<SearchParameters>
    {
        private readonly IAudioListProviderParameters<SearchParameters> audioListProviderParameters = new SearchAudioListProvider();

        public override IAudioListProviderParameters<SearchParameters> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override string HeaderInfo
        {
            get { return "Поиск"; }
        }

        [ImportingConstructor]
        public SearchProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
        }
    }
}
