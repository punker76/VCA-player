using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi.Audio
{
    /// <summary>
    /// Класс, описывающий текст аудиозаписи
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKLyrics
    {
        /// <summary>
        /// Идентификатор текста
        /// </summary>
        [JsonProperty("lyrics_id")]
        public ulong LyricsId { get; set; }

        /// <summary>
        /// Текст аудиозаписи
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
