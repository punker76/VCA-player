using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKapi
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VKList<ItemsType>
    {
        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("items")]
        public IEnumerable<ItemsType> Items { get; set; }
    }
}
