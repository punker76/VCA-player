using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace VKapi.Audio
{
    public static class AudioRequest
    {
        #region audio.getCount
        /// <summary>
        /// Возвращает количество аудиозаписей пользователя или сообщества.
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество).</param>
        /// <returns>Количество аудиозаписей</returns>
        public static int GetCount(long ownerId)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);
            string response = VKSession.Instance.DoRequest("audio.getCount", param);

            JObject obj = JObject.Parse(response);

            return obj["response"].Value<int>();
        }

        /// <summary>
        /// Возвращает количество аудиозаписей пользователя или сообщества.
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Количество аудиозаписей</returns>
        public static async Task<int> GetCountAsync(long ownerId, CancellationToken? token = null)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);
            string response = await VKSession.Instance.DoRequestAsync("audio.getCount", param);

            JObject obj = JObject.Parse(response);

            return obj["response"].Value<int>();
        }
        #endregion

        #region audio.getById
        /// <summary>
        /// Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <returns>Список объектов VKAudio</returns>
        public static IEnumerable<VKAudio> GetById(long ownerId, IEnumerable<ulong> audios)
        {
            VKParams param = new VKParams();

            param.Add("audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString())));

            string response = VKSession.Instance.DoRequest("audio.getById", param);

            JObject obj = JObject.Parse(response);

            VKAudio[] objArr = JsonConvert.DeserializeObject<VKAudio[]>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список объектов VKAudio</returns>
        public static async Task<IEnumerable<VKAudio>> GetByIdAsync(long ownerId, IEnumerable<ulong> audios, CancellationToken? token = null)
        {
            VKParams param = new VKParams();

            param.Add("audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString())));

            string response = await VKSession.Instance.DoRequestAsync("audio.getById", param);

            JObject obj = JObject.Parse(response);

            var objArr = JsonConvert.DeserializeObjectAsync<VKAudio[]>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();
            return await objArr;
        }
        #endregion

        #region audio.get
        private static VKParams parseParamsForGet(
            long? ownerId,
            long? albumId,
            IEnumerable<ulong> audioIds,
            bool? needUser,
            int offset,
            int? count)
        {
            VKParams param = new VKParams();

            if (ownerId.HasValue) { param.Add("owner_id", ownerId.Value); }
            if (albumId.HasValue) { param.Add("album_id", albumId.Value); }
            if (audioIds != null) { param.Add("audio_ids", String.Join(",", audioIds)); }
            if (needUser.HasValue) { param.Add("need_user", (needUser.Value ? "1" : "0")); }
            param.Add("offset", offset);
            if (count.HasValue) { param.Add("count", count.Value); }

            return param;
        }

        /// <summary>
        /// Возвращает список аудиозаписей пользователя или сообщества.
        /// <para>Обратите внимание, что даже с использованием параметра offset получить информацию об аудиозаписях, находящихся после первых 6000 в списке пользователя или сообщества, невозможно.</para>
        /// <para>Обратите внимание, что ссылки на mp3 привязаны к ip-адресу.</para>
        /// </summary>
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-"</remarks>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="albumId">Идентификатор альбома с аудиозаписями</param>
        /// <param name="audioIds">Идентификаторы аудиозаписей, информацию о которых необходимо вернуть</param>
        /// <param name="needUser">Возвращать ли информацию о пользователях, загрузивших аудиозапись</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного количества аудиозаписей. По умолчанию — 0</param>
        /// <param name="count">Количество аудиозаписей, информацию о которых необходимо вернуть. Максимальное значение — 6000</param>
        /// <returns>Возвращает список объектов VKAudio</returns>
        public static VKList<VKAudio> Get(
            long? ownerId = null,
            long? albumId = null,
            IEnumerable<ulong> audioIds = null,
            bool? needUser = null,
            int offset = 0,
            int? count = null
            )
        {
            VKParams param = parseParamsForGet(ownerId, albumId, audioIds, needUser, offset, count);
            string response = VKSession.Instance.DoRequest("audio.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = JsonConvert.DeserializeObject<VKList<VKAudio>>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает список аудиозаписей пользователя или сообщества.
        /// <para>Обратите внимание, что даже с использованием параметра offset получить информацию об аудиозаписях, находящихся после первых 6000 в списке пользователя или сообщества, невозможно.</para>
        /// <para>Обратите внимание, что ссылки на mp3 привязаны к ip-адресу.</para>
        /// </summary>
        /// <remarks>Идентификатор сообщества в параметре <paramref name="ownerId"/> необходимо указывать со знаком "-"</remarks>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="albumId">Идентификатор альбома с аудиозаписями</param>
        /// <param name="audioIds">Идентификаторы аудиозаписей, информацию о которых необходимо вернуть</param>
        /// <param name="needUser">Возвращать ли информацию о пользователях, загрузивших аудиозапись</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного количества аудиозаписей. По умолчанию — 0</param>
        /// <param name="count">Количество аудиозаписей, информацию о которых необходимо вернуть. Максимальное значение — 6000</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Возвращает список объектов VKAudio</returns>
        public static async Task<VKList<VKAudio>> GetAsync(
            long? ownerId = null,
            long? albumId = null,
            IEnumerable<ulong> audioIds = null,
            bool? needUser = null,
            int offset = 0,
            int? count = null,
            CancellationToken? token = null)
        {
            VKParams param = parseParamsForGet(ownerId, albumId, audioIds, needUser, offset, count);
            string response = await VKSession.Instance.DoRequestAsync("audio.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKAudio>>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
        #endregion

        #region audio.getLyrics
        /// <summary>
        /// Возвращает текст аудиозаписи
        /// </summary>
        /// <param name="lyricsId">Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть,<para> может быть получен с помощью методов Get, GetById или Search</para></param>
        /// <returns>Текст аудиозаписи</returns>
        public static VKLyrics GetLyrics(long lyricsId)
        {
            VKParams param = new VKParams();
            param.Add("lyrics_id", lyricsId.ToString());

            string response = VKSession.Instance.DoRequest("audio.getLyrics", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = JsonConvert.DeserializeObject<VKLyrics>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает текст аудиозаписи
        /// </summary>
        /// <param name="lyricsId">Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть,<para> может быть получен с помощью методов Get, GetById или Search</para></param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Текст аудиозаписи</returns>
        public static async Task<VKLyrics> GetLyricsAsync(long lyricsId,
            CancellationToken? token = null)
        {
            VKParams param = new VKParams();
            param.Add("lyrics_id", lyricsId.ToString());

            string response = await VKSession.Instance.DoRequestAsync("audio.getLyrics", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKLyrics>(obj["response"].ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
        #endregion
    }
}
