using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VKapi.Models
{
    public abstract class APIBase
    {
        protected abstract String GroupName { get; }

        protected TReturn Execute<TReturn>(String method, VKParams param)
        {
            string response = VKSession.Instance.DoRequest(String.Format("{0}.{1}", GroupName, method), param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return default(TReturn);
            var objArr = JsonConvert.DeserializeObject<TReturn>(obj["response"].ToString());

            return objArr;
        }

        protected async Task<TReturn> ExecuteAsync<TReturn>(String method, VKParams param, CancellationToken? token)
        {
            string response = await VKSession.Instance.DoRequestAsync(String.Format("{0}.{1}", GroupName, method), param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return default(TReturn);
            var objArr = await JsonConvert.DeserializeObjectAsync<TReturn>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
    }
}
