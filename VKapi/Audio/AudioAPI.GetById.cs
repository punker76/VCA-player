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
        private const String GetByIdMethod = "getById";

        /// <summary>
        ///     Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <returns>Список объектов VKAudio</returns>
        public VKList<VKAudio> GetById(Int64 ownerId, IEnumerable<Int64> audios)
        {
            VKParams param = new VKParams
            {
                {"audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString()))}
            };

            var list = base.Execute<IEnumerable<VKAudio>>(GetByIdMethod, param);
            return new VKList<VKAudio>(list);
        }

        /// <summary>
        ///     Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список объектов VKAudio</returns>
        public async Task<VKList<VKAudio>> GetByIdAsync(Int64 ownerId, IEnumerable<Int64> audios,
            CancellationToken? token = null)
        {
            VKParams param = new VKParams
            {
                {"audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString()))}
            };

            var list = await base.ExecuteAsync<IEnumerable<VKAudio>>(GetByIdMethod, param, token);
            return new VKList<VKAudio>(list);
        }
    }
}
