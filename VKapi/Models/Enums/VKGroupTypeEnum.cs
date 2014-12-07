using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKGroupTypeEnum
    {
        [EnumValue("group")]
        [VKDescription("Группа")]
        Group,
        [EnumValue("page")]
        [VKDescription("Публичная страница")]
        Page,
        [EnumValue("event")]
        [VKDescription("Мероприятие")]
        Event
    }
}
