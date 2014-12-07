using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VKapi;
using VKapi.Models;

namespace VKapi.Audio
{
    public partial class AudioAPI
    {
        private const String ReorderMethod = "reorder";
        
        public Boolean Reorder(
            Int64 audioId,
            Int64? ownerId = null,
            Int64? before = null,
            Int64? after = null)
        {
            VKParams param = GetReorderParams(audioId, ownerId, before, after);
            return base.Execute<Int32>(ReorderMethod, param) == 1;
        }

        public async Task<Boolean> ReorderAsync(
            Int64 audioId,
            Int64? ownerId = null,
            Int64? before = null,
            Int64? after = null,
            CancellationToken? token = null)
        {
            VKParams param = GetReorderParams(audioId, ownerId, before, after);
            return await base.ExecuteAsync<Int32>(ReorderMethod, param, token) == 1;
        }

        private VKParams GetReorderParams(
            Int64 audioId,
            Int64? ownerId,
            Int64? before,
            Int64? after)
        {
            VKParams param = new VKParams();

            param.Add("audio_id", audioId);
            if (ownerId.HasValue)
            {
                param.Add("owner_id", ownerId.Value);
            }
            if (before.HasValue)
            {
                param.Add("before", before.Value);
            }
            if (after.HasValue)
            {
                param.Add("after", after.Value);
            }

            return param;
        }
    }
}
