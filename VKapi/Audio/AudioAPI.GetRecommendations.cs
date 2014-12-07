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
        private const String GetRecommendationsMethod = "getRecommendations";

        public VKList<VKAudio> GetRecommendations(
            Int64 targetAudioId,
            Int64 targetAudioOwnerId,
            Int64? userId = null,
            Int64? offset = null,
            Int64? count = null,
            Boolean? shuffle = null
            )
        {
            VKParams param = GetRecommendationsParams(targetAudioId, targetAudioOwnerId, userId, offset, count, shuffle);
            return base.Execute<VKList<VKAudio>>(GetRecommendationsMethod, param);
        }

        public async Task<VKList<VKAudio>> GetRecommendationsAsync(
            Int64 targetAudioId,
            Int64 targetAudioOwnerId,
            Int64? userId = null,
            Int64? offset = null,
            Int64? count = null,
            Boolean? shuffle = null,
            CancellationToken? token = null
            )
        {
            VKParams param = GetRecommendationsParams(targetAudioId, targetAudioOwnerId, userId, offset, count, shuffle);
            return await base.ExecuteAsync<VKList<VKAudio>>(GetRecommendationsMethod, param, token);
        }

        private VKParams GetRecommendationsParams(
            Int64 targetAudioId,
            Int64 targetAudioOwnerId,
            Int64? userId,
            Int64? offset,
            Int64? count,
            Boolean? shuffle
            )
        {
            VKParams param = new VKParams();

            param.Add("target_audio", String.Format("{0}_{1}", targetAudioOwnerId, targetAudioId));
            if (userId.HasValue)
            {
                param.Add("user_id", userId.Value);
            }
            if (offset.HasValue)
            {
                param.Add("offset", offset.Value);
            }
            if (count.HasValue)
            {
                param.Add("count", count.Value);
            }
            if (shuffle.HasValue)
            {
                param.Add("shuffle", shuffle.Value.ToVKValue());
            }

            return param;
        }
    }
}
