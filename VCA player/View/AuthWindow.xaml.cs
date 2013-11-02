using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web;
using VKapi;
using VCA_player.Model;
using VKapi.Audio;
using System.Reflection;

namespace VCA_player.View
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        public AuthWindow()
        {
            InitializeComponent();
            HideScriptErrors(AuthBrowser, true);
            AuthBrowser.Navigate(VKSession.Instance.GetAuthURL());
        }

        public AuthWindow(string url)
        {
            InitializeComponent();
            HideScriptErrors(AuthBrowser, true);
            AuthBrowser.Navigate(url);
        }

        private void AuthBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.IndexOf(VKSettings.RedirectUrl) >= 0 && e.Uri.Fragment.Length > 0)
            {
                var urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment.Substring(1));
                VKSession.Instance.AccessToken = urlParams.Get("access_token");
                VKSession.Instance.UserId = Convert.ToUInt64(urlParams.Get("user_id"));
            }
            if (e.Uri.AbsoluteUri.IndexOf(VKSettings.RedirectUrl) == 0)
            {
                Close();
            }
        }
    }
}