using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Информация о вложениях записи
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKAttachment
    {
        /// <summary>
        ///     Типа объекта
        /// </summary>
        [JsonProperty("type")]
        public String Type { get; set; }

        /// <summary>
        ///     Аудиозапись
        /// </summary>
        [JsonProperty("audio")]
        public VKAudio Audio { get; set; }
    }
}