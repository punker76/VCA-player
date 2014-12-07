using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rina.Infastructure.Interfaces
{
    public interface IAudioReorderProvider
    {
        Boolean CanReorder { get; }
        Int64 OwnerId { get; }
    }
}
