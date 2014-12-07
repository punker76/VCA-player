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
using Microsoft.Practices.ServiceLocation;
using Rina.Infastructure.Interfaces;
using Rina.Modules.DownloadManager.Views;

namespace Rina.Modules.DownloadManager
{
    [ModuleExport(typeof(DownloadManagerModule))]
    public sealed class DownloadManagerModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        [Import]
        public ILeftMenuService LeftMenuService { get; set; }

        public void Initialize()
        {
            LeftMenuService.RegisterItem(ViewNames.DownloadManagerView, typeof(DownloadManagerView), typeof(DownloadManagerIndicator));
        }
    }
}
