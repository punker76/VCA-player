using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Wall;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace VKapi.Wall
{
    public static class WallRequest
    {
        #region wall.get

        public enum FilterEnum
        {
            suggests, postponed, owner, others, all
        }

        private static VKParams parseGetParam(
            long? ownerId,
            string domain,
            int? offset,
            int? count,
            FilterEnum? filter,
            bool extended
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
        /// Возвращает список записей со стены пользователя или сообщества
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, со стены которого необходимо получить записи (по умолчанию — текущий пользователь). Идентификатор сообщества необходимо указывать со знаком "-"</param>
        /// <param name="domain">Короткий адрес пользователя или сообщества</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества записей</param>
        /// <param name="count">Количество записей, которое необходимо получить (но не более 100)</param>
        /// <param name="filter">Определяет, какие типы записей на стене необходимо получить</param>
        /// <returns>Список записей со стены</returns>
        public static VKList<VKPost> Get(
            long? ownerId = null,
            string domain = "",
            int? offset = null,
            int? count = null,
            FilterEnum? filter = null
            )
        {
            VKParams param = parseGetParam(ownerId, domain, offset, count, filter, false);
            string response = VKSession.Instance.DoRequest("wall.get", param);

            JObject obj = JObject.Parse(response);
            VKList<VKPost> objArr = JsonConvert.DeserializeObject<VKList<VKPost>>((obj["response"] ?? String.Empty).ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает список записей со стены пользователя или сообщества
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, со стены которого необходимо получить записи (по умолчанию — текущий пользователь). Идентификатор сообщества необходимо указывать со знаком "-"</param>
        /// <param name="domain">Короткий адрес пользователя или сообщества</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества записей</param>
        /// <param name="count">Количество записей, которое необходимо получить (но не более 100)</param>
        /// <param name="filter">Определяет, какие типы записей на стене необходимо получить</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список записей со стены</returns>
        public static async Task<VKList<VKPost>> GetAsync(
            long? ownerId = null,
            string domain = "",
            int? offset = null,
            int? count = null,
            FilterEnum? filter = null,
            CancellationToken? token = null
            )
        {
            VKParams param = parseGetParam(ownerId, domain, offset, count, filter, false);
           
            string response = await VKSession.Instance.DoRequestAsync("wall.get", param);
           
            JObject obj = JObject.Parse(response);
            VKList<VKPost> objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKPost>>((obj["response"] ?? String.Empty).ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
        #endregion
    }
}
