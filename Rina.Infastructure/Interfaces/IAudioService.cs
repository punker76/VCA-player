using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;
using System.Threading;
using Rina.Infastructure.Models;

namespace Rina.Infastructure.Interfaces
{
    public interface IAudioService
    {
        IAudioListProvider AudioProvider { get; set; }

        Task LoadListAsync(AddAudioItem addItem, CancellationToken token);
    }
}
