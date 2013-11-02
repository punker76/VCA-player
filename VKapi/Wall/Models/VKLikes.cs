using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi.Wall
{
    /// <summary>
    /// Класс, представляющий информация о лайках
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKLikes
    {
        /// <summary>
        /// Число пользователей, которым понравилась запись
        /// </summary>
        [JsonProperty("count")]
        public uint Count { get; set; }

        /// <summary>
        /// Наличие отметки «Мне нравится» от текущего пользователя
        /// </summary>
        [JsonProperty("user_likes")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool UserLikes { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь поставить отметку «Мне нравится»
        /// </summary>
        [JsonProperty("can_like")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool CanLike { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь сделать репост записи
        /// </summary>
        [JsonProperty("can_publish")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool CanPublish { get; set; }
    }
}
