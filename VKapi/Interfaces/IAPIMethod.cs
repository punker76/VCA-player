using System;

namespace VKapi.Interfaces
{
    internal interface IAPIMethod
    {
        String Method { get; }
        Type ReturnType { get; }
    }
}
/*
        private static String ReorderMethodName = "audio.reorder";
        
        private static VKParams ParseParamsForReorder(
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

        public static Boolean Reorder(
            Int64 audioId,
            Int64? ownerId = null,
            Int64? before = null,
            Int64? after = null
            )
        {
            VKParams param = ParseParamsForReorder(audioId, ownerId, before, after);
            string response = VKSession.Instance.DoRequest(ReorderMethodName, param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return false;
            var objArr = JsonConvert.DeserializeObject<Int32>(obj["response"].ToString());

            return objArr == 1;
        }


        public static async Task<Boolean> ReorderAsync(
            Int64 audioId,
            Int64? ownerId = null,
            Int64? before = null,
            Int64? after = null,
            CancellationToken? token = null
            )
        {
            VKParams param = ParseParamsForReorder(audioId, ownerId, before, after);
            string response = await VKSession.Instance.DoRequestAsync(ReorderMethodName, param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return false;
            var objArr = await JsonConvert.DeserializeObjectAsync<Int32>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr == 1;
        }*/