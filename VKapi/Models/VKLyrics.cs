using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, описывающий текст аудиозаписи
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKLyrics
    {
        /// <summary>
        ///     Идентификатор текста
        /// </summary>
        [JsonProperty("lyrics_id")]
        public Int64 LyricsId { get; set; }

        /// <summary>
        ///     Текст аудиозаписи
        /// </summary>
        [JsonProperty("text")]
        public String Text { get; set; }
    }
}