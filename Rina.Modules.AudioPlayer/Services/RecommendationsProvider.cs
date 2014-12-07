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

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class RecommendationsProvider : IAudioListProviderParameters<NotificationWrapper<VKAudio>>
    {
        public Boolean IsStateSet { get { return false; } }
        public NotificationWrapper<VKAudio> State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Debug.Assert(State.Content != null);

            await AudioProviderHelper.PartialLoading(async (o, c, t) =>
                await VKApi.Audio.GetRecommendationsAsync(State.Content.Id, State.Content.OwnerId,
                    offset: o, count: c, token: t),
                addItem, 400, token);
        }

        public Boolean Update()
        {
            if (State.Content == null) return false;

            return true;
        }
    }
}
