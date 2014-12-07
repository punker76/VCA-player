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
        private const String DeleteMethod = "delete";

        public Boolean Delete(
            Int64 audioId,
            Int64 ownerId)
        {
            VKParams param = GetDeleteParams(audioId, ownerId);

            return (base.Execute<Int32>(DeleteMethod, param) == 1);
        }

        public async Task<Boolean> DeleteAsync(
            Int64 audioId,
            Int64 ownerId,
            CancellationToken? token = null)
        {
            VKParams param = GetDeleteParams(audioId, ownerId);

            return (await base.ExecuteAsync<Int32>(DeleteMethod, param, token) == 1);
        }

        private VKParams GetDeleteParams(Int64 audioId, Int64 ownerId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            param.Add("owner_id", ownerId);

            return param;
        }
    }
}
