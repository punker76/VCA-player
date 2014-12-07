using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Models;
using System.Threading;
using Rina.Infastructure.Models;
using System.ComponentModel;

namespace Rina.Infastructure.Interfaces
{
    public delegate Task AddAudioItem(VKAudio item);
    public interface IAudioListProviderParameters<T> : IAudioListProvider
        where T : INotifyPropertyChanged
    {
        Boolean IsStateSet { get; }
        T State { get; set; }

        Boolean Update();
    }
}
