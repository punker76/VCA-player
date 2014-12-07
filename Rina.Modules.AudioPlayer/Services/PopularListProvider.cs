using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using Rina.Modules.AudioPlayer.Helpers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using VKapi.Audio;
using VKapi.Friends;
using Rina.Modules.AudioPlayer.Models;
using VKapi;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class PopularListProvider : IAudioListProviderParameters<PopularParameters>
    {
        public Boolean IsStateSet { get { return true; } }
        public PopularParameters State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            await AudioProviderHelper.PartialLoading(async (o, c, t) =>
                await VKApi.Audio.GetPopularAsync(State.Genre, State.OnlyEng, o, c, t),
                addItem, 400, token);
        }

        public Boolean Update()
        {
            /*PopularParameters stateCast = state as PopularParameters;
            if (stateCast == null) return false;

            this.popular = stateCast;
            IsStateSet = true;
            */
            return true;
        }
    }
}
