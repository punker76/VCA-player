using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VKapi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VKFriend
    {
        public enum SexEnum
        {
            [EnumValue("1")] Female,
            [EnumValue("2")] Male,
            [EnumValue("0")] Unknown
        }

        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [JsonProperty("first_name")]
        public String FirstName { get; set; }

        [JsonProperty("last_name")]
        public String LastName { get; set; }

        [JsonProperty("sex")]
        [JsonConverter(typeof (JsonEnumConverter<SexEnum>))]
        public SexEnum Sex { get; set; }

        [JsonProperty("bdate")]
        public String BDate { get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        [JsonProperty("country")]
        public Int64 Country { get; set; }

        [JsonProperty("photo_50")]
        public String Photo50 { get; set; }

        [JsonProperty("photo_100")]
        public String Photo100 { get; set; }

        [JsonProperty("photo_200_orig")]
        public String Photo200Orig { get; set; }

        [JsonProperty("photo_200")]
        public String Photo200 { get; set; }

        [JsonProperty("photo_400_orig")]
        public String Photo400Orig { get; set; }

        [JsonProperty("photo_max")]
        public String PhotoMax { get; set; }

        [JsonProperty("photo_max_orig")]
        public String PhotoMaxOrig { get; set; }

        [JsonProperty("online")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean Online { get; set; }

        [JsonProperty("lists")]
        public IEnumerable<String> Lists { get; set; }

        [JsonProperty("screen_name")]
        public String ScreenName { get; set; }

        [JsonProperty("has_mobile")]
        [JsonConverter(typeof (JsonBoolConvereter))]
        public Boolean HasMobile { get; set; }
    }
}