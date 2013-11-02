using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi.Wall
{
    /// <summary>
    /// Класс, представляющий информацию о комментариях
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKComments
    {
        /// <summary>
        /// Количество комментариев
        /// </summary>
        [JsonProperty("count")]
        public uint Count { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь комментировать запись
        /// </summary>
        [JsonProperty("can_post")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool CanPost { get; set; }
    }
}
