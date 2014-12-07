using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VKapi.Models;

namespace VKapi
{
    public sealed class VKSession
    {
        private static VKSession instance = new VKSession();
        public readonly String UrlAPI = @"https://api.vk.com/method/";

        private VKSession()
        {
        }

        public static VKSession Instance
        {
            get { return instance; }
        }

        public String AccessToken { get; set; }
        public Int64 UserId { get; set; }

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

        internal String DoRequest(String method, VKParams param)
        {
            String requestUri = BuildRequest(method, param);

            WebRequest request =
                WebRequest.Create(requestUri);

            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            String responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return responseFromServer;
        }

        internal async Task<String> DoRequestAsync(String method, VKParams param)
        {
            String requestUri = BuildRequest(method, param);

            WebRequest request = WebRequest.Create(requestUri);
            WebResponse response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String res = await reader.ReadToEndAsync();

            reader.Close();
            response.Close();

            return res;
        }

        public Uri GetAuthURL()
        {
            Uri url = new Uri(String.Format(
                @"https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display=popup&api_version={3}&response_type=token",
                VKSettings.AppId, VKSettings.Scope, VKSettings.RedirectUrl, VKSettings.APIVer));

            return url;
        }
    }
}