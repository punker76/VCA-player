using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, представляющий информацию о комментариях
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKComments
    {
        /// <summary>
        ///     Количество комментариев
        /// </summary>
        [JsonProperty("count")]
        public Int64 Count { get; set; }

        /// <summary>
        ///     Информация о том, может ли текущий пользователь комментировать запись
        /// </summary>
        [JsonProperty("can_post")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean CanPost { get; set; }
    }
}