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
        private const String GetAlbumsMethod = "getAlbums";

        public VKList<VKAlbum> GetAlbums(
            Int64? ownerId = null,
            Int32? offset = null,
            Int32? count = null)
        {
            VKParams param = GetForGetAlbumsParams(ownerId, offset, count);
            return base.Execute<VKList<VKAlbum>>(GetAlbumsMethod, param);
        }

        public async Task<VKList<VKAlbum>> GetAlbumsAsync(
            Int64? ownerId = null,
            Int32? offset = null,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = GetForGetAlbumsParams(ownerId, offset, count);
            return await base.ExecuteAsync<VKList<VKAlbum>>(GetAlbumsMethod, param, token);
        }

        private VKParams GetForGetAlbumsParams(Int64? ownerId, Int32? offset, Int32? count)
        {
            VKParams param = new VKParams();
            if (ownerId.HasValue) param.Add("owner_id", ownerId.Value);
            if (offset.HasValue) param.Add("offset", offset.Value);
            if (count.HasValue) param.Add("count", count.Value);

            return param;
        }
    }
}
