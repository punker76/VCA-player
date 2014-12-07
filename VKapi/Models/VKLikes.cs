using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, представляющий информация о лайках
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKLikes
    {
        /// <summary>
        ///     Число пользователей, которым понравилась запись
        /// </summary>
        [JsonProperty("count")]
        public Int64 Count { get; set; }

        /// <summary>
        ///     Наличие отметки «Мне нравится» от текущего пользователя
        /// </summary>
        [JsonProperty("user_likes")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean UserLikes { get; set; }

        /// <summary>
        ///     Информация о том, может ли текущий пользователь поставить отметку «Мне нравится»
        /// </summary>
        [JsonProperty("can_like")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean CanLike { get; set; }

        /// <summary>
        ///     Информация о том, может ли текущий пользователь сделать репост записи
        /// </summary>
        [JsonProperty("can_publish")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean CanPublish { get; set; }
    }
}