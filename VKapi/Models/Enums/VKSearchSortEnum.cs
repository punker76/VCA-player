using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKSearchSortEnum
    {
        [EnumValue("2")]
        [VKDescription("По популярности")]
        ByRating,
        [EnumValue("1")]
        [VKDescription("По длительности")]
        ByDuration,
        [EnumValue("0")]
        [VKDescription("По дате добавления")]
        ByAddingDate
    }
}
