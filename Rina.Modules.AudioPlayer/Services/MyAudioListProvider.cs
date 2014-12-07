using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;
using VKapi;
using Rina.Infastructure.Interfaces;
using System.Threading;
using Rina.Infastructure.Models;
using Rina.Infastructure.Extensions;
using Rina.Modules.AudioPlayer.Helpers;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class MyAudioListProvider : IAudioListProvider, IAudioReorderProvider
    {
        public Boolean CanReorder { get { return true; } }
        public Int64 OwnerId { get { return VKSession.Instance.UserId; } }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Int32 count = await VKApi.Audio.GetCountAsync(VKSession.Instance.UserId, token);
            await AudioProviderHelper.PartialLoading((o, c, t) =>
                VKApi.Audio.GetAsync(VKSession.Instance.UserId, offset: o, count: c, token: token),
                addItem, count, token);
        }
    }
}
