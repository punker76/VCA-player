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
        private const String SetBroadcastMethod = "setBroadcast";

        public VKList<Int32> SetBroadcast(
            IEnumerable<Int32> ids,
            Int32? ownerId = null,
            Int32? audioId = null)
        {
            VKParams param = GetSetBroadcastParams(ownerId, audioId, ids);

            var list = base.Execute<IEnumerable<Int32>>(SetBroadcastMethod, param);

            return new VKList<int>(list);
        }

        public async Task<VKList<Int32>> SetBroadcastAsync(
            IEnumerable<Int32> ids,
            Int32? ownerId = null,
            Int32? audioId = null,
            CancellationToken? token = null)
        {
            VKParams param = GetSetBroadcastParams(ownerId, audioId, ids);

            var list = await base.ExecuteAsync<IEnumerable<Int32>>(SetBroadcastMethod, param, token);

            return new VKList<int>(list);
        }

        private VKParams GetSetBroadcastParams(
            Int32? ownerId,
            Int32? audioId,
            IEnumerable<Int32> ids)
        {
            VKParams param = new VKParams();

            if (ownerId.HasValue && audioId.HasValue)
            {
                param.Add("audio", String.Format("{0}_{1}", ownerId.Value, audioId.Value));
            }
            if (ids != null) param.Add("target_ids", String.Join(",", ids));

            return param;
        }
    }
}
