using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VKAlbum
    {
        [JsonProperty("id")]
        public Int64 Id { get; set; }

        [JsonProperty("owner_id")]
        public Int64 OwnerId { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }
    }
}
