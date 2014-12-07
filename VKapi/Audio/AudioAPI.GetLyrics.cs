using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKapi.Models;

namespace VKapi.Audio
{
    public partial class AudioAPI
    {
        private const String GetLyricsMethod = "getLyrics";

        /// <summary>
        ///     Возвращает текст аудиозаписи
        /// </summary>
        /// <param name="lyricsId">
        ///     Идентификатор текста аудиозаписи, информацию о котором необходимо вернуть,
        ///     <para> может быть получен с помощью методов Get, GetById или Search</para>
        /// </param>
        /// <returns>Текст аудиозаписи</returns>
        public VKLyrics GetLyrics(Int64 lyricsId)
        {
            VKParams param = new VKParams();
            param.Add("lyrics_id", lyricsId);

            return base.Execute<VKLyrics>(GetLyricsMethod, param);
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
        public async Task<VKLyrics> GetLyricsAsync(Int64 lyricsId, CancellationToken? token = null)
        {
            VKParams param = new VKParams();
            param.Add("lyrics_id", lyricsId);

            return await base.ExecuteAsync<VKLyrics>(GetLyricsMethod, param, token);
        }
    }
}
