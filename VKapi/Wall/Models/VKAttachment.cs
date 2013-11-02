using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VKapi.Audio;

namespace VKapi.Wall
{
    /// <summary>
    /// Информация о вложениях записи
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKAttachment
    {
        /// <summary>
        /// Типа объекта
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Аудиозапись
        /// </summary>
        [JsonProperty("audio")]
        public VKAudio Audio { get; set; }
    }
}
