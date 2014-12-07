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
        private const String GetPopularMethod = "getPopular";

        public VKList<VKAudio> GetPopular(
            VKGenreEnum genre,
            Boolean? onlyEng = null,
            Int32? offset = null,
            Int32? count = null)
        {
            VKParams param = GetPopularParams(genre, onlyEng, offset, count);

            return new VKList<VKAudio>(base.Execute<IEnumerable<VKAudio>>(GetPopularMethod, param));
        }

        public async Task<VKList<VKAudio>> GetPopularAsync(
            VKGenreEnum genre,
            Boolean? onlyEng = null,
            Int32? offset = null,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = GetPopularParams(genre, onlyEng, offset, count);

            var list = await base.ExecuteAsync<IEnumerable<VKAudio>>(GetPopularMethod, param, token);

            return new VKList<VKAudio>(list);
        }

        private VKParams GetPopularParams(
            VKGenreEnum genre,
            Boolean? onlyEng,
            Int32? offset,
            Int32? count)
        {
            VKParams param = new VKParams();

            param.Add("genre_id", (Int32)genre);
            if (onlyEng.HasValue) param.Add("only_eng", onlyEng.Value.ToVKValue());
            if (offset.HasValue) param.Add("offset", offset.Value);
            if (count.HasValue) param.Add("count", count.Value);

            return param;
        }
    }
}
