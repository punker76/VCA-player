using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VKapi;
using Newtonsoft.Json.Linq;

namespace VKapi.Friends
{
    public static class FriendsRequest
    {
        #region friends.get
        public enum OrderEnum { name, hints, random }
        public enum FieldsEnum
        {
            nickname, screen_name,
            sex, bdate,
            city, country,
            timezone, photo_50,
            photo_100, photo_200_orig,
            has_mobile, contacts,
            education, online,
            relation, last_seen,
            status, can_write_private_message,
            can_see_all_posts,
            can_post, universities
        }
        public enum NameCaseEnum
        {
            nom, gen, dat, acc, ins, abl
        }

        private static VKParams parseParamsForGet(
            IEnumerable<FieldsEnum> fields,
            long? userId,
            OrderEnum? order,
            ulong? listId,
            uint? count,
            uint? offset,
            NameCaseEnum? nameCase
            )
        {
            VKParams param = new VKParams();

            if (fields != null) { param.Add("fields", String.Join(",", fields)); }
            if (userId.HasValue) { param.Add("user_id", userId.Value); }
            if (order.HasValue) { param.Add("order", order.Value); }
            if (listId.HasValue) { param.Add("list_id", listId.Value); }
            if (count.HasValue) { param.Add("count", count.Value); }
            if (offset.HasValue) { param.Add("offset", offset.Value); }
            if (nameCase.HasValue) { param.Add("name_case", nameCase.Value); }

            return param;
        }

        /// <summary>
        /// Возвращает расширенную информацию о друзьях пользователя
        /// </summary>
        /// <param name="fields">Cписок дополнительных полей, которые необходимо вернуть</param>
        /// <param name="userId">Идентификатор пользователя, для которого необходимо получить список друзей. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя</param>
        /// <param name="order">Порядок, в котором нужно вернуть список друзей</param>
        /// <param name="listId">Идентификатор списка друзей, друзей из которого необходимо получить</param>
        /// <param name="count">Количество друзей, которое нужно вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества друзей</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя</param>
        /// <returns>Список друзей</returns>
        public static VKList<VKFriend> GetExtended(
            IEnumerable<FieldsEnum> fields,
            long? userId = null,
            OrderEnum? order = null,
            ulong? listId = null,
            uint? count = null,
            uint? offset = null,
            NameCaseEnum? nameCase = null
            )
        {
            VKParams param = parseParamsForGet(fields, userId, order, listId, count, offset, nameCase);
            string response = VKSession.Instance.DoRequest("friends.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;

            var objArr = JsonConvert.DeserializeObject<VKList<VKFriend>>(obj["response"].ToString());

            return objArr;
        }

        /// <summary>
        /// Возвращает расширенную информацию о друзьях пользователя
        /// </summary>
        /// <param name="fields">Cписок дополнительных полей, которые необходимо вернуть</param>
        /// <param name="userId">Идентификатор пользователя, для которого необходимо получить список друзей. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя</param>
        /// <param name="order">Порядок, в котором нужно вернуть список друзей</param>
        /// <param name="listId">Идентификатор списка друзей, друзей из которого необходимо получить</param>
        /// <param name="count">Количество друзей, которое нужно вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества друзей</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя</param>
        /// <param name="token">Токен для отмены выполнения запроса</param>
        /// <returns>Список друзей</returns>
        public async static Task<VKList<VKFriend>> GetExtendedAsync(
            IEnumerable<FieldsEnum> fields,
            long? userId = null,
            OrderEnum? order = null,
            ulong? listId = null,
            uint? count = null,
            uint? offset = null,
            NameCaseEnum? nameCase = null,
            CancellationToken? token = null
            )
        {
            VKParams param = parseParamsForGet(fields, userId, order, listId, count, offset, nameCase);
            string response = await VKSession.Instance.DoRequestAsync("friends.get", param);

            JObject obj = JObject.Parse(response);

            if (obj["response"] == null) return null;
            var objArr = await JsonConvert.DeserializeObjectAsync<VKList<VKFriend>>(obj["response"].ToString());
            if (token.HasValue) token.Value.ThrowIfCancellationRequested();

            return objArr;
        }
        #endregion
    }
}
