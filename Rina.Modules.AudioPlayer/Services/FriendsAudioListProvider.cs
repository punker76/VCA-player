using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using Rina.Modules.AudioPlayer.Helpers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VKapi.Models;
using VKapi.Friends;
using VKapi;
using Rina.Modules.AudioPlayer.Models;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class FriendsAudioListProvider : IAudioListProviderParameters<FriendParameters>
    {
        private VKFriend friend;
        private VKAlbum album;

        public Boolean IsStateSet { get; private set; }
        public FriendParameters State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Debug.Assert(this.friend != null);

            Int32 count = await VKApi.Audio.GetCountAsync(this.friend.Id, token);
            Int64? albumId = this.album != null ? this.album.Id : (Int64?)null;
            await AudioProviderHelper.PartialLoading((o, c, t) =>
                VKApi.Audio.GetAsync(this.friend.Id, albumId, offset: o, count: c,
                    token: t),
                addItem, count, token);
        }

        public Boolean Update()
        {
            if (State.Content == null || (this.friend == State.Content && this.album == State.Album)) return false;

            this.friend = State.Content;
            this.album = State.Album;
            IsStateSet = true;

            return true;
        }
    }
}
