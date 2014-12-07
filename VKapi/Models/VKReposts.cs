using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, представляющей информацию о репостах
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKReposts
    {
        /// <summary>
        ///     Число пользователей, скопировавших запись
        /// </summary>
        [JsonProperty("count")]
        public Int32 Count { get; set; }

        /// <summary>
        ///     Наличие репоста от текущего пользователя
        /// </summary>
        [JsonProperty("user_reposted")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean UserReposted { get; set; }
    }
}