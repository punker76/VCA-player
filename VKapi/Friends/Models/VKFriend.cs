using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace VKapi.Friends
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VKFriend
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public enum SexEnum
        {
            [EnumValue("1")]
            Female,
            [EnumValue("2")]
            Male,
            [EnumValue("0")]
            Unknown
        }

        [JsonProperty("sex")]
        [JsonConverter(typeof(JsonEnumConverter<SexEnum>))]
        public SexEnum Sex { get; set; }

        [JsonProperty("bdate")]
        public string BDate { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public ulong Country { get; set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200_orig")]
        public string Photo200Orig { get; set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }

        [JsonProperty("photo_400_orig")]
        public string Photo400Orig { get; set; }

        [JsonProperty("photo_max")]
        public string PhotoMax { get; set; }

        [JsonProperty("photo_max_orig")]
        public string PhotoMaxOrig { get; set; }

        [JsonProperty("online")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool Online { get; set; }

        [JsonProperty("lists")]
        public IEnumerable<string> Lists { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("has_mobile")]
        [JsonConverter(typeof(JsonBoolConvereter))]
        public bool HasMobile { get; set; }
    }
}
