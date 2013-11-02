using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi.Wall
{
    /// <summary>
    /// Класс, описывающий информацию о местоположении
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKGeo
    {
        /// <summary>
        /// Идентификатор места
        /// </summary>
        [JsonProperty("place_id")]
        public ulong PlaceId { get; set; }

        /// <summary>
        /// Название места
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Тип места
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Идентификатор страны
        /// </summary>
        [JsonProperty("country_id")]
        public ulong CountryId { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        [JsonProperty("city_id")]
        public ulong CityId { get; set; }

        /// <summary>
        /// Строка с указанием адреса места в городе
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
