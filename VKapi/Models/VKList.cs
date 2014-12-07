using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VKapi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VKList<TItemsType>
    {
        [JsonProperty("count")]
        public Int32 Count { get; set; }

        [JsonProperty("items")]
        public IEnumerable<TItemsType> Items { get; set; }

        public VKList(IEnumerable<TItemsType> items)
        {
            Items = items;
            Count = items.Count();
        }
    }
}