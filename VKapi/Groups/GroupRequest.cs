using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VKapi.Groups
{
    public static class GroupRequest
    {
        #region groups.get

        public enum Filter
        {
            admin,
            editor,
            moder,
            groups,
            publics,
            events
        }

        public enum Fields
        {
            city,
            counter,
            place,
            description,
            wiki_page,
            members_count,
            counters,
            start_date,
            end_date,
            can_post,
            can_see_all_posts,
            status,
            contacts,
            links,
            fixed_post,
            verified,
            site,
            can_create_topic
        }

        private static VKParams parseGetParam(
            ulong userId,
            bool extended,
            IEnumerable<Filter> filter,
            IEnumerable<Fields> fields,
            int? offset,
            int? count
            )
        {
            VKParams param = new VKParams();

            param.Add("user_id", userId);
            param.Add("extended", extended ? "1" : "0");
            if (filter != null) { param.Add("filter", String.Join(",", filter)); }
            if (fields != null) { param.Add("fields", String.Join(",", fields)); }
            if (offset.HasValue) param.Add("offset", offset.Value.ToString());
            if (count.HasValue) param.Add("count", count.Value.ToString());

            return param;
        }

        /// <summary>
        /// Возвращает идентификаторы сообществ указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о сообществах которого требуется получить</param>
        /// <param name="filter">Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую.<para>Доступны значения admin, editor, moder, groups, publics, events.</para>По умолчанию возвращаются все сообщества пользователя. При указании фильтра admin будут возвращены сообщества, в которых пользователь является администратором, editor — администратором или редактором, moder — администратором, редактором или модератором.</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества сообществ</param>
        /// <param name="count">Количество сообществ, информацию о которых нужно вернуть</param>
        /// <returns>Список идентификаторов сообществ</returns>
        public static VKList<string> GetIds(
            ulong userId,
            IEnumerable<Filter> filter = null,
            IEnumerable<Fields> fields = null,
            int? offset = null,
            int? count = null
            )
        {
            VKParams param = parseGetParam(userId, false, filter, fields, offset, count);
            string response = VKSession.Instance.DoRequest("groups.get", param);

            JObject obj = JObject.Parse(response);

            var objArr = JsonConvert.DeserializeObject<VKList<string>>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает идентификаторы сообществ указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о сообществах которого требуется получить</param>
        /// <param name="filter">Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую.<para>Доступны значения admin, editor, moder, groups, publics, events.</para>По умолчанию возвращаются все сообщества пользователя. При указании фильтра admin будут возвращены сообщества, в которых пользователь является администратором, editor — администратором или редактором, moder — администратором, редактором или модератором.</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества сообществ</param>
        /// <param name="count">Количество сообществ, информацию о которых нужно вернуть</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список идентификаторов сообществ</returns>
        public static async Task<VKList<string>> GetIdsAsync(
            ulong userId,
            IEnumerable<Filter> filter = null,
            IEnumerable<Fields> fields = null,
            int? offset = null,
            int? count = null,
            CancellationToken? token = null
            )
        {
            VKParams param = parseGetParam(userId, false, filter, fields, offset, count);
            string response = await VKSession.Instance.DoRequestAsync("groups.get", param);

            JObject obj = JObject.Parse(response);

            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<string>>(obj["response"].ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }

        /// <summary>
        /// Возвращает список сообществ указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о сообществах которого требуется получить</param>
        /// <param name="filter">Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую.<para>Доступны значения admin, editor, moder, groups, publics, events.</para>По умолчанию возвращаются все сообщества пользователя. При указании фильтра admin будут возвращены сообщества, в которых пользователь является администратором, editor — администратором или редактором, moder — администратором, редактором или модератором.</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества сообществ</param>
        /// <param name="count">Количество сообществ, информацию о которых нужно вернуть</param>
        /// <returns>Список сообществ</returns>
        public static VKList<VKGroup> GetExtended(
            ulong userId,
            IEnumerable<Filter> filter = null,
            IEnumerable<Fields> fields = null,
            int? offset = null,
            int? count = null
            )
        {
            VKParams param = parseGetParam(userId, true, filter, fields, offset, count);
            string response = VKSession.Instance.DoRequest("groups.get", param);

            JObject obj = JObject.Parse(response);

            VKList<VKGroup> objArr = JsonConvert.DeserializeObject<VKList<VKGroup>>((obj["response"] ?? String.Empty).ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает список сообществ указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о сообществах которого требуется получить</param>
        /// <param name="filter">Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую.<para>Доступны значения admin, editor, moder, groups, publics, events.</para>По умолчанию возвращаются все сообщества пользователя. При указании фильтра admin будут возвращены сообщества, в которых пользователь является администратором, editor — администратором или редактором, moder — администратором, редактором или модератором.</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества сообществ</param>
        /// <param name="count">Количество сообществ, информацию о которых нужно вернуть</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список сообществ</returns>
        public static async Task<VKList<VKGroup>> GetExtendedAsync(
            ulong userId,
            IEnumerable<Filter> filter = null,
            IEnumerable<Fields> fields = null,
            int? offset = null,
            int? count = null,
            CancellationToken? token = null
            )
        {
            VKParams param = parseGetParam(userId, true, filter, fields, offset, count);
            string response = await VKSession.Instance.DoRequestAsync("groups.get", param);

            JObject obj = JObject.Parse(response);

            VKList<VKGroup> objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKGroup>>((obj["response"] ?? String.Empty).ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
        #endregion
    }
}
