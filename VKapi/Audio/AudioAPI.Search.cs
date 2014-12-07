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
        private const String SearchMethod = "search";

        public VKList<VKAudio> Search(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            VKSearchSortEnum sort,
            Boolean searchOwn,
            Int32 offset,
            Int32? count = null)
        {
            VKParams param = GetSearchParams(query, autoComplete, lyrics, performerOnly, sort, searchOwn, offset,
                count);

            return base.Execute<VKList<VKAudio>>(SearchMethod, param);
        }

        public async Task<VKList<VKAudio>> SearchAsync(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            VKSearchSortEnum sort,
            Boolean searchOwn,
            Int32 offset,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = GetSearchParams(query, autoComplete, lyrics, performerOnly, sort, searchOwn, offset,
                count);

            return await base.ExecuteAsync<VKList<VKAudio>>(SearchMethod, param, token);
        }

        private VKParams GetSearchParams(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            VKSearchSortEnum sort,
            Boolean searchOwn,
            Int32 offset,
            Int32? count)
        {
            VKParams param = new VKParams();
            param.Add("q", query);
            param.Add("auto_complete", autoComplete.ToVKValue());
            param.Add("lyrics", lyrics.ToVKValue());
            param.Add("performer_only", performerOnly.ToVKValue());
            param.Add("sort", EnumString.GetStringValue(sort));
            param.Add("search_own", searchOwn.ToVKValue());
            param.Add("offset", offset);
            if (count.HasValue) param.Add("count", count);

            return param;
        }
    }
}
