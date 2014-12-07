using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VKapi;

namespace VKapi.Audio
{/*
    public static partial class AudioAPIOld
    {
        #region audio.getCount

        /// <summary>
        ///     Возвращает количество аудиозаписей пользователя или сообщества.
        ///     <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество).</param>
        /// <returns>Количество аудиозаписей</returns>
        public static Int32 GetCount(Int64 ownerId)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);
            string response = VKSession.Instance.DoRequest("audio.getCount", param);

            JObject obj = JObject.Parse(response);

            return obj["response"].Value<Int32>();
        }

        /// <summary>
        ///     Возвращает количество аудиозаписей пользователя или сообщества.
        ///     <remarks>Идентификатор сообщества в параметре <paramref name="ownerId" /> необходимо указывать со знаком "-"</remarks>
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписей (пользователь или сообщество)</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Количество аудиозаписей</returns>
        public static async Task<Int32> GetCountAsync(Int64 ownerId, CancellationToken? token = null)
        {
            VKParams param = new VKParams();
            param.Add("owner_id", ownerId);
            string response = await VKSession.Instance.DoRequestAsync("audio.getCount", param);

            JObject obj = JObject.Parse(response);

            return obj["response"].Value<Int32>();
        }

        #endregion

        #region audio.getById

        /// <summary>
        ///     Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <returns>Список объектов VKAudio</returns>
        public static IEnumerable<VKAudio> GetById(Int64 ownerId, IEnumerable<Int64> audios)
        {
            VKParams param = new VKParams();

            param.Add("audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString())));

            string response = VKSession.Instance.DoRequest("audio.getById", param);

            JObject obj = JObject.Parse(response);

            VKAudio[] objArr = JsonConvert.DeserializeObject<VKAudio[]>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        ///     Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца аудиозаписи</param>
        /// <param name="audios">Идентификаторы аудиозаписей</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список объектов VKAudio</returns>
        public static async Task<IEnumerable<VKAudio>> GetByIdAsync(Int64 ownerId, IEnumerable<Int64> audios,
            CancellationToken? token = null)
        {
            VKParams param = new VKParams
            {
                {"audios", String.Join(",", audios.Select(x => ownerId + "_" + x.ToString()))}
            };

            string response = await VKSession.Instance.DoRequestAsync("audio.getById", param);

            JObject obj = JObject.Parse(response);

            var objArr = JsonConvert.DeserializeObjectAsync<VKAudio[]>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();
            return await objArr;
        }

        #endregion

        #region audio.get

        private static VKParams ParseParamsForGet(
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
        public static VKList<VKAudio> Get(
            Int64? ownerId = null,
            Int64? albumId = null,
            IEnumerable<Int64> audioIds = null,
            Boolean? needUser = null,
            Int32 offset = 0,
            Int32? count = null
            )
        {
            VKParams param = ParseParamsForGet(ownerId, albumId, audioIds, needUser, offset, count);
            string response = VKSession.Instance.DoRequest("audio.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = JsonConvert.DeserializeObject<VKList<VKAudio>>(obj["response"].ToString());

            return objArr;
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
        public static async Task<VKList<VKAudio>> GetAsync(
            Int64? ownerId = null,
            Int64? albumId = null,
            IEnumerable<Int64> audioIds = null,
            Boolean? needUser = null,
            Int32 offset = 0,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForGet(ownerId, albumId, audioIds, needUser, offset, count);
            string response = await VKSession.Instance.DoRequestAsync("audio.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKAudio>>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        #endregion

        #region audio.getBroadcastList

        public enum BroadcastFilter
        {
            Friends,
            Groups,
            All
        }

        private static VKParams ParseParamsForGetBroadcast(BroadcastFilter filter, Boolean getOnlyActive)
        {
            VKParams param = new VKParams();
            param.Add("active", getOnlyActive ? "1" : "0");
            String filterValue;
            switch (filter)
            {
                case BroadcastFilter.Friends:
                    filterValue = "friends";
                    break;
                case BroadcastFilter.Groups:
                    filterValue = "groups";
                    break;
                default:
                case BroadcastFilter.All:
                    filterValue = "all";
                    break;
            }

            param.Add("filter", filterValue);

            return param;
        }

        public static async Task<IEnumerable<VKBroadcast>> GetBroadcastListAsync(BroadcastFilter filter = BroadcastFilter.All, Boolean getOnlyActive = false, CancellationToken? token = null)
        {
            VKParams param = ParseParamsForGetBroadcast(filter, getOnlyActive);
            String response = await VKSession.Instance.DoRequestAsync("audio.getBroadcastList", param);

            JObject obj = JObject.Parse(response);
            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<IEnumerable<VKBroadcast>>(obj["response"].ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        #endregion

        #region audio.getLyrics

        /// <summary>
        ///     Возвращает текст аудиозаписи
        /// </summary>
        /// <param name="lyricsId">
        ///     Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть,
        ///     <para> может быть получен с помощью методов Get, GetById или Search</para>
        /// </param>
        /// <returns>Текст аудиозаписи</returns>
        public static VKLyrics GetLyrics(Int64 lyricsId)
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
        ///     Возвращает текст аудиозаписи
        /// </summary>
        /// <param name="lyricsId">
        ///     Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть,
        ///     <para> может быть получен с помощью методов Get, GetById или Search</para>
        /// </param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Текст аудиозаписи</returns>
        public static async Task<VKLyrics> GetLyricsAsync(Int64 lyricsId,
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

        #region audio.getAlbums

        private static VKParams ParseParamsForGetAlbums(Int64? ownerId, Int32? offset, Int32? count)
        {
            VKParams param = new VKParams();
            if (ownerId.HasValue) param.Add("owner_id", ownerId.Value);
            if (offset.HasValue) param.Add("offset", offset.Value);
            if (count.HasValue) param.Add("count", count.Value);

            return param;
        }

        public static VKList<VKAlbum> GetAlbums(
            Int64? ownerId = null,
            Int32? offset = null,
            Int32? count = null)
        {
            VKParams param = ParseParamsForGetAlbums(ownerId, offset, count);
            string response = VKSession.Instance.DoRequest("audio.getAlbums", param);
            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = JsonConvert.DeserializeObject<VKList<VKAlbum>>(obj["response"].ToString());

            return objArr;
        }

        public static async Task<VKList<VKAlbum>> GetAlbumsAsync(
            Int64? ownerId = null,
            Int32? offset = null,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForGetAlbums(ownerId, offset, count);
            string response = await VKSession.Instance.DoRequestAsync("audio.getAlbums", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKAlbum>>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        #endregion

        #region audio.add

        private static VKParams ParseParamsForAdd(Int64 audioId, Int64 ownerId, Int64? groupId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            param.Add("owner_id", ownerId);
            if (groupId.HasValue) param.Add("group_id", groupId.Value);

            return param;
        }

        public static Int64 Add(
            Int64 audioId,
            Int64 ownerId,
            Int64? groupId = null)
        {
            VKParams param = ParseParamsForAdd(audioId, ownerId, groupId);
            string response = VKSession.Instance.DoRequest("audio.add", param);

            JObject obj = JObject.Parse(response);
            if (obj["response"] == null) return -1;
            var aid = JsonConvert.DeserializeObject<Int64>(obj["response"].ToString());

            return aid;
        }

        public static async Task<Int64> AddAsync(
            Int64 audioId,
            Int64 ownerId,
            Int64? groupId = null,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForAdd(audioId, ownerId, groupId);
            string response = await VKSession.Instance.DoRequestAsync("audio.add", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return -1;
            var aid = await JsonConvert.DeserializeObjectAsync<Int64>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return aid;
        }

        #endregion

        #region audio.delete

        private static VKParams ParseParamsForDelete(Int64 audioId, Int64 ownerId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            param.Add("owner_id", ownerId);

            return param;
        }

        public static Boolean Delete(
            Int64 audioId,
            Int64 ownerId)
        {
            VKParams param = ParseParamsForDelete(audioId, ownerId);
            string response = VKSession.Instance.DoRequest("audio.delete", param);

            JObject obj = JObject.Parse(response);
            if (obj["response"] == null) return false;
            var aid = JsonConvert.DeserializeObject<Int32>(obj["response"].ToString());

            return (aid == 1);
        }

        public static async Task<Boolean> DeleteAsync(
            Int64 audioId,
            Int64 ownerId,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForDelete(audioId, ownerId);
            string response = await VKSession.Instance.DoRequestAsync("audio.delete", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return false;
            var aid = await JsonConvert.DeserializeObjectAsync<Int32>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return aid == 1;
        }

        #endregion

        #region audio.restore

        private static VKParams ParseParamsForRestore(Int64 audioId, Int64? ownerId)
        {
            VKParams param = new VKParams();
            param.Add("audio_id", audioId);
            if (ownerId.HasValue) param.Add("owner_id", ownerId.Value);

            return param;
        }

        public static VKAudio Restore(
            Int64 audioId,
            Int64? ownerId = null)
        {
            VKParams param = ParseParamsForRestore(audioId, ownerId);
            string response = VKSession.Instance.DoRequest("audio.restore", param);

            JObject obj = JObject.Parse(response);
            if (obj["response"] == null) return null;
            var audio = JsonConvert.DeserializeObject<VKAudio>(obj["response"].ToString());

            return audio;
        }

        public static async Task<VKAudio> RestoreAsync(
            Int64 audioId,
            Int64? ownerId = null,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForRestore(audioId, ownerId);
            string response = await VKSession.Instance.DoRequestAsync("audio.restore", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var audio = await JsonConvert.DeserializeObjectAsync<VKAudio>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return audio;
        }

        #endregion

        #region audio.search

        private static VKParams ParseParamsForSearch(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            SearchSort sort,
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

        public static VKList<VKAudio> Search(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            SearchSort sort,
            Boolean searchOwn,
            Int32 offset,
            Int32? count = null)
        {
            VKParams param = ParseParamsForSearch(query, autoComplete, lyrics, performerOnly, sort, searchOwn, offset,
                count);
            string response = VKSession.Instance.DoRequest("audio.search", param);

            JObject obj = JObject.Parse(response);
            if (obj["response"] == null) return null;
            var objArr = JsonConvert.DeserializeObject<VKList<VKAudio>>(obj["response"].ToString());

            return objArr;
        }

        public static async Task<VKList<VKAudio>> SearchAsync(
            String query,
            Boolean autoComplete,
            Boolean lyrics,
            Boolean performerOnly,
            SearchSort sort,
            Boolean searchOwn,
            Int32 offset,
            Int32? count = null,
            CancellationToken? token = null)
        {
            VKParams param = ParseParamsForSearch(query, autoComplete, lyrics, performerOnly, sort, searchOwn, offset,
                count);
            string response = await VKSession.Instance.DoRequestAsync("audio.search", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKAudio>>(obj["response"].ToString());

            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        #endregion
    }*/
}