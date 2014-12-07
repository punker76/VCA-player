using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism;
using Rina.Infastructure.Interfaces;
using Rina.Modules.AudioPlayer.Services;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics.Contracts;
using VKapi.Friends;
using Rina.Infastructure.Extensions;
using VKapi.Audio;
using VKapi;
using System.Collections.ObjectModel;
using Rina.Modules.AudioPlayer.Services.Logics;
using Rina.Infastructure.Models;
using VKapi.Models;
using Rina.Modules.AudioPlayer.Models;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class FriendsProviderViewModel : FilteredProviderViewModel<VKFriend, FriendParameters>
    {
        private Boolean isAlbumChanged = false;

        private readonly IAudioListProviderParameters<FriendParameters> audioListProviderParameters = new FriendsAudioListProvider();
        private readonly IFilterLogic<VKFriend> filterLogic = new FriendFilterLogic();

        protected override IFilterLogic<VKFriend> FilterLogic
        {
            get { return this.filterLogic; }
        }

        public override IAudioListProviderParameters<FriendParameters> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override String HeaderInfo
        {
            get { return "Друзья"; }
        }

        public ObservableCollection<VKAlbum> FriendAlbums { get; private set; }
        
        [ImportingConstructor]
        public FriendsProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            FriendAlbums = new ObservableCollection<VKAlbum>();
        }

        protected override async Task<IEnumerable<VKFriend>> RefreshCore()
        {
            var list = await VKApi.Audio.GetBroadcastListAsync(VKBroadcastFilterEnum.All);
            var friendList = await FriendsAPI.GetExtendedAsync(new[] {FriendsAPI.FieldsEnum.photo_50});
            Debug.Assert(list != null);
            Debug.Assert(friendList != null);

            list.Items.ForEach(b =>
            {
                var friend = friendList.Items.FirstOrDefault(v => v.Id == b.Id);
                if (friend != null) b.Photo50 = friend.Photo50;
            });

            return list.Items;
        }

        protected override async void OnStatePropertyChanged(object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (isAlbumChanged) return;
            //base.OnStatePropertyChanged(sender, e);
            Debug.WriteLine("Friends property changed");
            if (e.PropertyName == "Content")
            {
                isAlbumChanged = true;
                State.Album = null;
                isAlbumChanged = false;

                FriendAlbums.Clear();
                var list = await VKApi.Audio.GetAlbumsAsync(State.Content.Id);
                if (list != null)
                {
                    list.Items.ForEach(v => FriendAlbums.Add(v));
                }
            }

            base.OnStatePropertyChanged(sender, e);
        }
    }
}
