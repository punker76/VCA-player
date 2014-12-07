using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Rina.Infastructure.Interfaces;
using Rina.Modules.AudioPlayer.Services;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics.Contracts;
using VKapi.Friends;
using VKapi.Audio;
using VKapi;
using System.Collections.ObjectModel;
using System.Windows;
using Rina.Infastructure.Models;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class AlbumsProviderViewModel : AudioListProviderItemsViewModelBase<VKAlbum, NotificationWrapper<VKAlbum>>
    {
        private readonly IAudioListProviderParameters<NotificationWrapper<VKAlbum>> audioListProviderParameters = new AlbumAudioListProvider();

        public override IAudioListProviderParameters<NotificationWrapper<VKAlbum>> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override String HeaderInfo
        {
            get { return "Альбомы"; }
        }

        [ImportingConstructor]
        public AlbumsProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
        }

        protected override async Task<IEnumerable<VKAlbum>> RefreshCore()
        {
            var list = await VKApi.Audio.GetAlbumsAsync();
            Debug.Assert(list != null);
            return list.Items;
        }
    }
}
