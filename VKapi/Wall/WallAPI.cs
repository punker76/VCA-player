using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VKapi.Models;

namespace VKapi.Wall
{
    public static class WallAPI
    {
        #region wall.get

        public enum FilterEnum
        {
            suggests,
            postponed,
            owner,
            others,
            all
        }

        private static VKParams parseGetParam(
            Int64? ownerId,
            String domain,
            Int32? offset,
            Int32? count,
            FilterEnum? filter,
            Boolean extended
            )
        {
            VKParams param = new VKParams();

            if (ownerId.HasValue) param.Add("owner_id", ownerId.Value);
            if (!String.IsNullOrWhiteSpace(domain)) param.Add("domain", domain);
            if (offset.HasValue) param.Add("offset", offset.Value);
            if (count.HasValue) param.Add("count", count.Value);
            if (filter.HasValue) param.Add("filter", filter.Value);
            param.Add("extended", extended ? "1" : "0");

            return param;
        }


        /// <summary>
        ///     Возвращает список записей со стены пользователя или сообщества
        /// </summary>
        /// <param name="ownerId">
        ///     Идентификатор пользователя или сообщества, со стены которого необходимо получить записи (по
        ///     умолчанию — текущий пользователь). Идентификатор сообщества необходимо указывать со знаком "-"
        /// </param>
        /// <param name="domain">Короткий адрес пользователя или сообщества</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества записей</param>
        /// <param name="count">Количество записей, которое необходимо получить (но не более 100)</param>
        /// <param name="filter">Определяет, какие типы записей на стене необходимо получить</param>
        /// <returns>Список записей со стены</returns>
        public static VKList<VKPost> Get(
            Int64? ownerId = null,
            String domain = "",
            Int32? offset = null,
            Int32? count = null,
            FilterEnum? filter = null
            )
        {
            VKParams param = parseGetParam(ownerId, domain, offset, count, filter, false);
            string response = VKSession.Instance.DoRequest("wall.get", param);

            JObject obj = JObject.Parse(response);
            VKList<VKPost> objArr =
                JsonConvert.DeserializeObject<VKList<VKPost>>((obj["response"] ?? String.Empty).ToString());

            return objArr;
        }

        /// <summary>
        ///     Возвращает список записей со стены пользователя или сообщества
        /// </summary>
        /// <param name="ownerId">
        ///     Идентификатор пользователя или сообщества, со стены которого необходимо получить записи (по
        ///     умолчанию — текущий пользователь). Идентификатор сообщества необходимо указывать со знаком "-"
        /// </param>
        /// <param name="domain">Короткий адрес пользователя или сообщества</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества записей</param>
        /// <param name="count">Количество записей, которое необходимо получить (но не более 100)</param>
        /// <param name="filter">Определяет, какие типы записей на стене необходимо получить</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список записей со стены</returns>
        public static async Task<VKList<VKPost>> GetAsync(
            Int64? ownerId = null,
            String domain = "",
            Int32? offset = null,
            Int32? count = null,
            FilterEnum? filter = null,
            CancellationToken? token = null
            )
        {
            VKParams param = parseGetParam(ownerId, domain, offset, count, filter, false);

            string response = await VKSession.Instance.DoRequestAsync("wall.get", param);

            JObject obj = JObject.Parse(response);
            VKList<VKPost> objArr =
                await JsonConvert.DeserializeObjectAsync<VKList<VKPost>>((obj["response"] ?? String.Empty).ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        #endregion
    }
}