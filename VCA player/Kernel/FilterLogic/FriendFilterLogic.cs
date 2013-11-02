using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi;
using VKapi.Friends;
using VCA_player.Model;

namespace VCA_player.Kernel
{
    class FriendFilterLogic : FilterLogicBase<VKFriend>
    {
        public override bool Filter(VCAListItem<VKFriend> item)
        {
            if (item == null || item.Item == null)
                return false;
            else if (!checkContains(item.Item.FirstName + " " + item.Item.LastName, SearchFilter))
                return false;

            return true;
        }
    }
}
