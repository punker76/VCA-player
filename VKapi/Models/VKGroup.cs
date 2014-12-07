using System;
using Newtonsoft.Json;
using VKapi.Models;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, описывающий сообщество
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKGroup
    {
        /// <summary>
        ///     Идентификатор сообщества
        /// </summary>
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        /// <summary>
        ///     Название сообщества
        /// </summary>
        [JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        ///     Короткий адрес сообщества
        /// </summary>
        [JsonProperty("screen_name")]
        public String ScreenName { get; set; }

        /// <summary>
        ///     Является ли общество закрытым
        ///     Возможные значения
        ///     <list type="bullet">
        ///         <item>
        ///             <term>0</term>
        ///             <description>Открытое</description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>Закрытое</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>Частное</description>
        ///         </item>
        ///     </list>
        /// </summary>
        [JsonProperty("is_closed")]
        [JsonConverter(typeof(JsonEnumConverter<VKGroupIsClosedEnum>))]
        public VKGroupIsClosedEnum IsClosed { get; set; }

        /// <summary>
        ///     Является ли текущий пользователь руководителем сообщества
        /// </summary>
        [JsonProperty("is_admin")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean IsAdmin { get; set; }

        /// <summary>
        ///     Полномочия текущего пользователя
        ///     <list type="bullet">
        ///         <item>
        ///             <term>1</term>
        ///             <description>Модератор</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>Редактор</description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>Администратор</description>
        ///         </item>
        ///     </list>
        /// </summary>
        [JsonProperty("admin_level")]
        [JsonConverter(typeof(JsonEnumConverter<VKAdminLevelEnum>))]
        public VKAdminLevelEnum AdminLevel { get; set; }

        /// <summary>
        ///     Является ли текущий пользователь участником сообщества
        /// </summary>
        [JsonProperty("is_member")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean IsMember { get; set; }

        /// <summary>
        ///     Тип сообщества
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonEnumConverter<VKGroupTypeEnum>))]
        public VKGroupTypeEnum Type { get; set; }

        /// <summary>
        ///     url фотографии сообщества с размером 50x50px
        /// </summary>
        [JsonProperty("photo_50")]
        public String Photo50 { get; set; }

        /// <summary>
        ///     url фотографии сообщества с размером 100x100px
        /// </summary>
        [JsonProperty("photo_100")]
        public String Photo100 { get; set; }

        /// <summary>
        ///     url фотографии сообщества в максимальном размере
        /// </summary>
        [JsonProperty("photo_200")]
        public String Photo200 { get; set; }
    }
}