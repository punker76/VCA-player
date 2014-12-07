using VCA_player.Model.List;
using VKapi.Audio;

namespace VCA_player.Kernel.FilterLogic
{
    public class AudioFilterLogic : FilterLogicBase<VKAudio>
    {
        public override bool Filter(VCAListItem<VKAudio> item)
        {
            if (item == null || item.Item == null)
                return false;
            if (!CheckContains(item.Item.Artist + " " + item.Item.Title, SearchFilter))
                return false;

            return true;
        }
    }
}