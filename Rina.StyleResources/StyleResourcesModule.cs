using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Rina.Infastructure;
using Rina.Infastructure.Interfaces;
using Rina.StyleResources.Views;
using System.ComponentModel.Composition;

namespace Rina.StyleResources
{
    [ModuleExport(typeof(StyleResourcesModule))]
    public sealed class StyleResourcesModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        [Import]
        public ILeftMenuService LeftMenuService { get; set; }

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(RegionNames.LeftMenuRegion, typeof (LeftMenuView));
        }
    }
}
