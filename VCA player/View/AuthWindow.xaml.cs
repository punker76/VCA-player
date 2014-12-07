using System;
using System.Reflection;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VKapi;

namespace VCA_player.View
{
    /// <summary>
    ///     Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
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

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof (WebBrowser).GetField("_axIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType()
                .InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] {hide});
        }

        private void AuthBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.IndexOf(VKSettings.RedirectUrl, StringComparison.Ordinal) >= 0 &&
                e.Uri.Fragment.Length > 0)
            {
                var urlParams = HttpUtility.ParseQueryString(e.Uri.Fragment.Substring(1));
                VKSession.Instance.AccessToken = urlParams.Get("access_token");
                VKSession.Instance.UserId = Convert.ToInt64(urlParams.Get("user_id"));
            }
            if (e.Uri.AbsoluteUri.IndexOf(VKSettings.RedirectUrl, StringComparison.Ordinal) == 0)
            {
                Close();
            }
        }
    }
}