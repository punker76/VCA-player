using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, описывающий аудиозапись
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKAudio
    {
        /// <summary>
        ///     Идентификатор аудиозаписи
        /// </summary>
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        /// <summary>
        ///     Идентификатор владельца аудиозаписи
        /// </summary>
        [JsonProperty("owner_id")]
        public Int64 OwnerId { get; set; }

        /// <summary>
        ///     Исполнитель
        /// </summary>
        [JsonProperty("artist")]
        public String Artist { get; set; }

        /// <summary>
        ///     Название композиции
        /// </summary>
        [JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        ///     Длительность аудиозаписи в секундах
        /// </summary>
        [JsonProperty("duration")]
        public Int32 Duration { get; set; }

        /// <summary>
        ///     Идентификатор текста аудиозаписи
        /// </summary>
        [JsonProperty("lyrics_id")]
        public Int64 LyricsId { get; set; }

        /// <summary>
        ///     Идентификатор альбома, в котором находится аудиозапись
        /// </summary>
        [JsonProperty("album_id")]
        public Int64 AlbumId { get; set; }

        /// <summary>
        ///     Идентификатор альбома, в котором находится аудиозапись
        /// </summary>
        [JsonProperty("genre_id")]
        public Int32 GenreId { get; set; }

        /// <summary>
        ///     Ссылка на mp3
        /// </summary>
        [JsonProperty("url")]
        public String Url { get; set; }
    }
}