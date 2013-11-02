using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Groups;
using VCA_player.Model;

namespace VCA_player.Kernel
{
    class GroupFilterLogic: FilterLogicBase<VKGroup>
    {
        public override bool Filter(VCAListItem<VKGroup> item)
        {
            if (item == null || item.Item == null)
                return false;
            else if (!checkContains(item.Item.Name, SearchFilter))
                return false;

            return true;
        }
    }
}
