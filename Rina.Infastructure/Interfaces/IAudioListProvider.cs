using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rina.Infastructure.Interfaces
{
    public interface IAudioListProvider
    {
        Task GetListAsync(AddAudioItem addItem, CancellationToken token);
    }
}
