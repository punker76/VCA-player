using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;

namespace VKapi
{
    public sealed class VKSession
    {
        private static VKSession instance = new VKSession();
        public static VKSession Instance { get { return instance; } }
        public string AccessToken { get; set; }
        public ulong UserId { get; set; }
        public readonly string UrlAPI = @"https://api.vk.com/method/";

        private VKSession() { }

        private string BuildRequest(string method, VKParams param)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(UrlAPI).Append(method).Append("?");

            foreach (var item in param)
            {
                builder.AppendFormat("{0}={1}&", item.Key, item.Value);
            }

            builder.AppendFormat("{0}={1}&", "access_token", AccessToken);
            builder.AppendFormat("{0}={1}", "v", VKSettings.APIVer);

            return builder.ToString();
        }

        internal string DoRequest(string method, VKParams param)
        {
            string requestUri = BuildRequest(method, param);

            WebRequest request =
                WebRequest.Create(requestUri);

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return responseFromServer;
        }

        internal async Task<string> DoRequestAsync(string method, VKParams param)
        {
            string requestUri = BuildRequest(method, param);

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string res = await reader.ReadToEndAsync();

            reader.Close();
            response.Close();

            return res;
        }

        public string GetAuthURL()
        {
            string url = String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=popup&api_version={3}&response_type=token",
                VKSettings.AppId, VKSettings.Scope, VKSettings.RedirectUrl, VKSettings.APIVer);

            return url;
        }
    }
}
