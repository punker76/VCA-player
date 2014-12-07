using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VKapi.Models;
using VKapi;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class AlbumAudioListProvider : IAudioListProviderParameters<NotificationWrapper<VKAlbum>>
    {
        private VKAlbum album;

        public Boolean IsStateSet { get; private set; }
        public NotificationWrapper<VKAlbum> State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Debug.Assert(this.album != null);

            var list = await VKApi.Audio.GetAsync(ownerId: this.album.OwnerId, albumId: this.album.Id, token: token);
            if (list != null)
            {
                foreach (var item in list.Items)
                {
                    token.ThrowIfCancellationRequested();
                    await addItem(item);
                }
            }
        }

        public Boolean Update()
        {
            if (State.Content == null || this.album == State.Content) return false;

            this.album = State.Content;
            IsStateSet = true;

            return true;
        }
    }
}
