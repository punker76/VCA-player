using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure;
using Rina.Infastructure.Interfaces;
using System.ComponentModel.Composition;
using VKapi.Audio;
using VKapi;
using System.Diagnostics;
using System.Threading;
using Rina.Infastructure.Models;
using System.Diagnostics.Contracts;

namespace Rina.Modules.AudioPlayer.Services
{
    [Export(typeof(IAudioService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class AudioService : IAudioService
    {
        public IAudioListProvider AudioProvider { get; set; }

        public async Task LoadListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Contract.Requires(AudioProvider != null);

            await AudioProvider.GetListAsync(addItem, token);
        }
    }
}
