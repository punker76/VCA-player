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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Rina.Infastructure;
using Rina.Modules.Auth.ViewModels;
using System.Diagnostics.Contracts;

namespace Rina.Modules.Auth.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed partial class AuthView : UserControl, IPartImportsSatisfiedNotification
    {
        private AuthViewModel viewModel;

        public AuthView()
        {
            InitializeComponent();
        }

        [Import]
        AuthViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }
            set
            {
                this.DataContext = viewModel = value;
            }
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            if (ViewModel != null)
            {
                NavigateToAuth();
            }
        }

        private void NavigateToAuth()
        {
            this.uiWebBrowser.Navigate(ViewModel.AuthUri);
        }

        private void WebBrowserNavigated(object sender, NavigationEventArgs e)
        {
            ViewModel.IsNavigate(e.Uri);
        }
    }
}
