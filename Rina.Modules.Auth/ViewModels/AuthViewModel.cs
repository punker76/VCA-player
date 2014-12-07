using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel.Composition;
using Rina.Modules.Auth.Interfaces;
using System.Diagnostics.Contracts;
using Rina.Infastructure;
using Rina.Infastructure.Interfaces;

namespace Rina.Modules.Auth.ViewModels
{
    [Export(typeof(AuthViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AuthViewModel : NotificationObject
    {
        private readonly IAuthService authService;
        private readonly ILeftMenuService leftMenuService;

        public Uri AuthUri
        {
            get { return authService.AuthUri; }
        }

        [ImportingConstructor]
        public AuthViewModel(IAuthService authService, ILeftMenuService leftMenuService)
        {
            Contract.Requires(authService != null);
            Contract.Requires(leftMenuService != null);

            this.leftMenuService = leftMenuService;
            this.authService = authService;
        }

        public void IsNavigate(Uri uri)
        {
            if (this.authService.IsAuth(uri))
            {
                this.leftMenuService.NavigateTo(ViewNames.AudioPlayerView);
            }
        }
    }
}
