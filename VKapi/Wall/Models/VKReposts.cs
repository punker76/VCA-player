using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi.Wall
{
    /// <summary>
    /// Класс, представляющей информацию о репостах
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKReposts
    {
        /// <summary>
        /// Число пользователей, скопировавших запись
        /// </summary>
        [JsonProperty("count")]
        public uint Count { get; set; }

        /// <summary>
        /// Наличие репоста от текущего пользователя
        /// </summary>
        [JsonProperty("user_reposted")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool UserReposted { get; set; }
    }
}
