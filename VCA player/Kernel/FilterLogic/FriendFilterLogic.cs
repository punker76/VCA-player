using VCA_player.Model.List;
using VKapi.Friends;

namespace VCA_player.Kernel.FilterLogic
{
    internal class FriendFilterLogic : FilterLogicBase<VKFriend>
    {
        public override bool Filter(VCAListItem<VKFriend> item)
        {
            if (item == null || item.Item == null)
                return false;
            if (!CheckContains(item.Item.FirstName + " " + item.Item.LastName, SearchFilter))
                return false;

            return true;
        }
    }
}