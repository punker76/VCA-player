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
        private const String RestoreMethod = "restore";

        public VKAudio Restore(
            Int64 audioId,
            Int64? ownerId = null)
        {
            VKParams param = GetRestoreParams(audioId, ownerId);

            return base.Execute<VKAudio>(RestoreMethod, param);
        }

        public async Task<VKAudio> RestoreAsync(
            Int64 audioId,
            Int64? ownerId = null,
            CancellationToken? token = null)
        {
            VKParams param = GetRestoreParams(audioId, ownerId);

            return await base.ExecuteAsync<VKAudio>(RestoreMethod, param, token);
        }

        private VKParams GetRestoreParams(Int64 audioId, Int64? ownerId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            if (ownerId.HasValue) param.Add("owner_id", ownerId.Value);

            return param;
        }
    }
}
