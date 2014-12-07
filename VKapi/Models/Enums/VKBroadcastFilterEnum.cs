using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKBroadcastFilterEnum
    {
        [EnumValue("friends")]
        [VKDescription("Только друзья")]
        Friends,
        [EnumValue("groups")]
        [VKDescription("Только сообщества")]
        Groups,
        [EnumValue("all")]
        [VKDescription("Все")]
        All
    }
}
