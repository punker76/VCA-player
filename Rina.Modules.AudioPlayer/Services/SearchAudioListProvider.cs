using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using VKapi;
using VKapi.Audio;
using System.Diagnostics;
using VKapi.Friends;
using VKapi.Groups;
using VKapi.Models;
using VKapi.Wall;
using Rina.Infastructure.Models;
using Rina.Infastructure.Extensions;
using Rina.Modules.AudioPlayer.Helpers;
using Rina.Modules.AudioPlayer.Models;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class SearchAudioListProvider : IAudioListProviderParameters<SearchParameters>
    {
        public Boolean IsStateSet { get; private set; }
        public SearchParameters State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            await AudioProviderHelper.PartialLoading(async (o, c, t) =>
                await VKApi.Audio.SearchAsync(
                    State.Query,
                    State.AutoComplete,
                    State.OnlyWithLyrics,
                    State.OnlyByArtist,
                    State.ChosenSort,
                    false, o, c, t),
                addItem, 300, token);
        }

        public Boolean Update()
        {
            IsStateSet = !String.IsNullOrWhiteSpace(State.Query);

            return true;
        }
    }
}
