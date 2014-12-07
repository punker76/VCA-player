using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    /// <summary>
    ///     Класс, описывающий информацию о местоположении
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VKGeo
    {
        /// <summary>
        ///     Идентификатор места
        /// </summary>
        [JsonProperty("place_id")]
        public Int64 PlaceId { get; set; }

        /// <summary>
        ///     Название места
        /// </summary>
        [JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        ///     Тип места
        /// </summary>
        [JsonProperty("type")]
        public String Type { get; set; }

        /// <summary>
        ///     Идентификатор страны
        /// </summary>
        [JsonProperty("country_id")]
        public Int64 CountryId { get; set; }

        /// <summary>
        ///     Идентификатор города
        /// </summary>
        [JsonProperty("city_id")]
        public Int64 CityId { get; set; }

        /// <summary>
        ///     Строка с указанием адреса места в городе
        /// </summary>
        [JsonProperty("address")]
        public String Address { get; set; }
    }
}