using VCA_player.Model.List;
using VKapi.Groups;

namespace VCA_player.Kernel.FilterLogic
{
    internal class GroupFilterLogic : FilterLogicBase<VKGroup>
    {
        public override bool Filter(VCAListItem<VKGroup> item)
        {
            if (item == null || item.Item == null)
                return false;
            if (!CheckContains(item.Item.Name, SearchFilter))
                return false;

            return true;
        }
    }
}