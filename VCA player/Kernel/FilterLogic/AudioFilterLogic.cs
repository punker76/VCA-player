using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;
using VCA_player.Model;

namespace VCA_player.Kernel
{
    public class AudioFilterLogic: FilterLogicBase<VKAudio>
    {
        public override bool Filter(VCAListItem<VKAudio> item)
        {
            if (item == null || item.Item == null)
                return false;
            else if (!checkContains(item.Item.Artist + " " + item.Item.Title, SearchFilter))
                return false;

            return true;
        }
    }
}
