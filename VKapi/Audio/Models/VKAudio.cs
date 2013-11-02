using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;

namespace VKapi.Audio
{
    /// <summary>
    /// Класс, описывающий аудиозапись
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKAudio
    {
        /// <summary>
        /// Идентификатор аудиозаписи
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; set; }

        /// <summary>
        /// Идентификатор владельца аудиозаписи
        /// </summary>
        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        [JsonProperty("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// Название композиции
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Длительность аудиозаписи в секундах
        /// </summary>
        [JsonProperty("duration")]
        public uint Duration { get; set; }

        /// <summary>
        /// Идентификатор текста аудиозаписи
        /// </summary>
        [JsonProperty("lyrics_id")]
        public ulong LyricsId { get; set; }

        /// <summary>
        /// Идентификатор альбома, в котором находится аудиозапись
        /// </summary>
        [JsonProperty("album_id")]
        public ulong AlbumId { get; set; }

        /// <summary>
        /// Идентификатор альбома, в котором находится аудиозапись
        /// </summary>
        [JsonProperty("genre_id")]
        public byte GenreId { get; set; }

        /// <summary>
        /// Ссылка на mp3
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Длительность в формате {m:ss}
        /// </summary>
        public string FormatDuration
        {
            get
            {
                int duration = Convert.ToInt32(Duration);
                int mm = duration / 60;
                int ss = duration % 60;

                return mm + ":" + ((ss < 10) ? ("0" + ss.ToString()) : ss.ToString());
            }
        }
    }
}
