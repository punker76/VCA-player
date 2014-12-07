using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using VKapi.Models;

namespace VKapi.Audio
{
    public partial class AudioAPI
    {
        private const String GetMethod = "get";
        /// <summary>
        ///     Возвращает список аудиозаписей пользователя или сообщества.
        ///     <para>
        ///         Обратите внимание, что даже с использованием параметра offset получить информацию об аудиозаписях,
        ///         находящихся после первых 6000 в списке пользователя или сообщества, невозможно.
        ///     </para>
        ///     <para>Обратите внимание, что ссылки на mp3 привязаны к ip-адресу.</para>
        /// </summary>
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="albumId">Идентификатор альбома с аудиозаписями</param>
        /// <param name="audioIds">Идентификаторы аудиозаписей, информацию о которых необходимо вернуть</param>
        /// <param name="needUser">Возвращать ли информацию о пользователях, загрузивших аудиозапись</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного количества аудиозаписей. По умолчанию — 0</param>
        /// <param name="count">Количество аудиозаписей, информацию о которых необходимо вернуть. Максимальное значение — 6000</param>
        /// <returns>Возвращает список объектов VKAudio</returns>
        public VKList<VKAudio> Get(
            Int64? ownerId = null,
            Int64? albumId = null,
            IEnumerable<Int64> audioIds = null,
            Boolean? needUser = null,
            Int32 offset = 0,
            Int32? count = null
            )
        {
            VKParams param = GetForGetParams(ownerId, albumId, audioIds, needUser, offset, count);
            return base.Execute<VKList<VKAudio>>(GetMethod, param);
        }

        /// <summary>
        ///     Возвращает список аудиозаписей пользователя или сообщества.
        ///     <para>
        ///         Обратите внимание, что даже с использованием параметра offset получить информацию об аудиозаписях,
        ///         находящихся после первых 6000 в списке пользователя или сообщества, невозможно.
        ///     </para>
        ///     <para>Обратите внимание, что ссылки на mp3 привязаны к ip-адресу.</para>
        /// </summary>
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="albumId">Идентификатор альбома с аудиозаписями</param>
        /// <param name="audioIds">Идентификаторы аудиозаписей, информацию о которых необходимо вернуть</param>
        /// <param name="needUser">Возвращать ли информацию о пользователях, загрузивших аудиозапись</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного количества аудиозаписей. По умолчанию — 0</param>
        /// <param name="count">Количество аудиозаписей, информацию о которых необходимо вернуть. Максимальное значение — 6000</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Возвращает список объектов VKAudio</returns>
        public async Task<VKList<VKAudio>> GetAsync(
            Int64? ownerId = null,
            Int64? albumId = null,
            IEnumerable<Int64> audioIds = null,
            Boolean? needUser = null,
            Int32 offset = 0,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = GetForGetParams(ownerId, albumId, audioIds, needUser, offset, count);
            return await base.ExecuteAsync<VKList<VKAudio>>(GetMethod, param, token);
        }

        private VKParams GetForGetParams(
            Int64? ownerId,
            Int64? albumId,
            IEnumerable<Int64> audioIds,
            Boolean? needUser,
            Int32 offset,
            Int32? count)
        {
            VKParams param = new VKParams();

            if (ownerId.HasValue)
            {
                param.Add("owner_id", ownerId.Value);
            }
            if (albumId.HasValue)
            {
                param.Add("album_id", albumId.Value);
            }
            if (audioIds != null)
            {
                param.Add("audio_ids", String.Join(",", audioIds));
            }
            if (needUser.HasValue)
            {
                param.Add("need_user", (needUser.Value ? "1" : "0"));
            }
            param.Add("offset", offset);
            if (count.HasValue)
            {
                param.Add("count", count.Value);
            }

            return param;
        }
    }
}
