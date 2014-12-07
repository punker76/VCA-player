using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Models;

namespace Rina.Infastructure.Models
{
    public class AudioCollection : List<VKAudio>
    {
        public AudioCollection(IEnumerable<VKAudio> collection)
            : base(collection)
        {
            
        }
    }
}
