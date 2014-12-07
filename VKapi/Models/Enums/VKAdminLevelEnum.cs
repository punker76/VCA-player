using VkontakteAPI.Converters;

namespace VKapi.Models
{
    public enum VKAdminLevelEnum
    {
        [EnumValue("1")]
        [VKDescription("Модератор")]
        Moderator,
        [EnumValue("2")]
        [VKDescription("Редактор")]
        Editor,
        [EnumValue("3")]
        [VKDescription("Администратор")]
        Administrator
    }
}
