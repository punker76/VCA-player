using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VKapi.Models;

namespace VKapi.Audio
{
    public partial class AudioAPI
    {
        private const String GetCountMethod = "getCount";

        /// <summary>
        ///     Возвращает количество аудиозаписей пользователя или сообщества.
        ///     <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество).</param>
        /// <returns>Количество аудиозаписей</returns>
        public Int32 GetCount(Int64 ownerId)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);

            return base.Execute<Int32>(GetCountMethod, param);
        }

        /// <summary>
        ///     Возвращает количество аудиозаписей пользователя или сообщества.
        ///     <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Количество аудиозаписей</returns>
        public async Task<Int32> GetCountAsync(Int64 ownerId, CancellationToken? token = null)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);

            return await base.ExecuteAsync<Int32>(GetCountMethod, param, token);
        }
    }
}
