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

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MyAudioListProviderViewModel : AudioListProviderViewModelBase
    {
        private readonly IAudioListProvider audioListProvider = new MyAudioListProvider();

        public override IAudioListProvider AudioListProvider
        {
            get { return this.audioListProvider; }
        }

        public override string HeaderInfo
        {
            get { return "Мои аудиозаписи"; }
        }

        [ImportingConstructor]
        public MyAudioListProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            
        }
    }
}
