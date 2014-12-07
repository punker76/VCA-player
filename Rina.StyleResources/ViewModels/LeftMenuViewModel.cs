using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.ObjectModel;
using Rina.StyleResources.Models;
using System.ComponentModel.Composition;
using Rina.Infastructure.Interfaces;

namespace Rina.StyleResources.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class LeftMenuViewModel : NotificationObject
    {
        public ILeftMenuService LeftService { get; private set; }

        [ImportingConstructor]
        public LeftMenuViewModel(ILeftMenuService leftService)
        {
            LeftService = leftService;
        }
    }
}
