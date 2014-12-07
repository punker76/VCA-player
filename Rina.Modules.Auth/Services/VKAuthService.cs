using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Rina.Modules.Auth.Interfaces;
using VKapi;
using System.Web;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Rina.Modules.Auth.Services
{
    [Export(typeof(IAuthService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VKAuthService : IAuthService
    {
        public Uri AuthUri
        {
            get
            {
                return VKSession.Instance.GetAuthURL();
            }
        }

        public Boolean IsAuth(Uri uri)
        {
            if (uri.AbsoluteUri.IndexOf(VKSettings.RedirectUrl, StringComparison.OrdinalIgnoreCase) >= 0 &&
                uri.Fragment.Length > 0)
            {
                var urlParams = HttpUtility.ParseQueryString(uri.Fragment.Substring(1));
                VKSession.Instance.AccessToken = urlParams.Get("access_token");
                VKSession.Instance.UserId = Convert.ToInt64(urlParams.Get("user_id"), CultureInfo.InvariantCulture);

                return true;
            }

            return false;
        }
    }
}
