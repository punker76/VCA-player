using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VKapi.Models;

namespace VKapi.Audio
{
    public partial class AudioAPI
    {
        private const String GetBroadcastListMethod = "getBroadcastList";

        public VKList<VKBroadcast> GetBroadcastList(
            VKBroadcastFilterEnum filter = VKBroadcastFilterEnum.All,
            Boolean getOnlyActive = false
            )
        {
            VKParams param = GetBroadcastParams(filter, getOnlyActive);
            var list = base.Execute<IEnumerable<VKBroadcast>>(GetBroadcastListMethod, param);

            return new VKList<VKBroadcast>(list);
        }

        public async Task<VKList<VKBroadcast>> GetBroadcastListAsync(
            VKBroadcastFilterEnum filter = VKBroadcastFilterEnum.All,
            Boolean getOnlyActive = false,
            CancellationToken? token = null
            )
        {
            VKParams param = GetBroadcastParams(filter, getOnlyActive);
            var list = await base.ExecuteAsync<IEnumerable<VKBroadcast>>(GetBroadcastListMethod, param, token);

            return new VKList<VKBroadcast>(list);
        }

        private VKParams GetBroadcastParams(VKBroadcastFilterEnum filter, Boolean getOnlyActive)
        {
            VKParams param = new VKParams();

            param.Add("active", getOnlyActive.ToVKValue());
            param.Add("filter", EnumString.GetStringValue(filter));

            return param;
        }
    }
}
