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
        private const String AddMethod = "add";

        public Int64 Add(
            Int64 audioId,
            Int64 ownerId,
            Int64? groupId = null)
        {
            VKParams param = GetAddParams(audioId, ownerId, groupId);
            return base.Execute<Int64>(AddMethod, param);
        }

        public async Task<Int64> AddAsync(
            Int64 audioId,
            Int64 ownerId,
            Int64? groupId = null,
            CancellationToken? token = null)
        {
            VKParams param = GetAddParams(audioId, ownerId, groupId);
            return await base.ExecuteAsync<Int64>(AddMethod, param, token);
        }

        private VKParams GetAddParams(Int64 audioId, Int64 ownerId, Int64? groupId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            param.Add("owner_id", ownerId);
            if (groupId.HasValue) param.Add("group_id", groupId.Value);

            return param;
        }
    }
}
