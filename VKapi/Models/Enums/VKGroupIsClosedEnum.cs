using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKGroupIsClosedEnum
    {
        [EnumValue("0")]
        [VKDescription("Открытое")]
        Open,
        [EnumValue("1")]
        [VKDescription("Закрытое")]
        Close,
        [EnumValue("2")]
        [VKDescription("Частное")]
        Private
    }
}
