using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VKapi;

namespace VKapi.Groups
{
    /// <summary>
    /// Класс, описывающий сообщество
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKGroup
    {
        /// <summary>
        /// Идентификатор сообщества
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        /// Название сообщества
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Короткий адрес сообщества
        /// </summary>
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        public enum ClosedType
        {
            [EnumValue("0")]
            Open,
            [EnumValue("1")]
            Close,
            [EnumValue("2")]
            Private
        }
        /// <summary>
        /// Является ли общество закрытым
        /// 
        /// Возможные значения
        /// <list type="bullet">
        /// <item>
        /// <term>0</term>
        /// <description>Открытое</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Закрытое</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Частное</description>
        /// </item>
        /// </list>
        /// </summary>
        [JsonProperty("is_closed")]
        [JsonConverter(typeof(JsonEnumConverter<ClosedType>))]
        public ClosedType IsClosed { get; set; }

        /// <summary>
        /// Является ли текущий пользователь руководителем сообщества
        /// </summary>
        [JsonProperty("is_admin")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool IsAdmin { get; set; }

        public enum AdminLevelEnum
        {
            [EnumValue("1")]
            Moderator,
            [EnumValue("2")]
            Editor,
            [EnumValue("3")]
            Administrator
        }
        /// <summary>
        /// Полномочия текущего пользователя
        /// 
        /// <list type="bullet">
        /// <item>
        /// <term>1</term>
        /// <description>Модератор</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Редактор</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Администратор</description>
        /// </item>
        /// </list>
        /// </summary>
        [JsonProperty("admin_level")]
        [JsonConverter(typeof(JsonEnumConverter<AdminLevelEnum>))]
        public AdminLevelEnum AdminLevel { get; set; }

        /// <summary>
        /// Является ли текущий пользователь участником сообщества
        /// </summary>
        [JsonProperty("is_member")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool IsMember { get; set; }

        public enum TypeEnum
        {
            [EnumValue("group")]
            Group,
            [EnumValue("page")]
            Page,
            [EnumValue("event")]
            Event
        }
        /// <summary>
        /// Тип сообщества
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonEnumConverter<TypeEnum>))]
        public TypeEnum Type { get; set; }

        /// <summary>
        /// url фотографии сообщества с размером 50x50px
        /// </summary>
        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        /// <summary>
        /// url фотографии сообщества с размером 100x100px
        /// </summary>
        [JsonProperty("photo_100")]
        public string Photo100 { get; set; }

        /// <summary>
        /// url фотографии сообщества в максимальном размере
        /// </summary>
        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }
    }
}
