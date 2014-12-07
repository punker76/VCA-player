using System;
using Newtonsoft.Json;

namespace VKapi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class VKBroadcast : VKFriend
    {
        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("status_audio")]
        public VKAudio StatusAudio { get; set; }

        public Boolean HasStatusAudio
        {
            get { return StatusAudio != null; }
        }
    }
}
