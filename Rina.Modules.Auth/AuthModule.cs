using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Rina.Infastructure;
using Rina.Modules.Auth.Views;

namespace Rina.Modules.Auth
{
    [ModuleExport(typeof(AuthModule))]
    public class AuthModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof (AuthView));
        }
    }
}
