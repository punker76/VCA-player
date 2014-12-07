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
using Rina.Modules.AudioPlayer.Views;
using Rina.Modules.AudioPlayer.Views.Providers;
using Microsoft.Practices.ServiceLocation;
using Rina.Infastructure.Interfaces;

namespace Rina.Modules.AudioPlayer
{
    [ModuleExport(typeof(AudioPlayerModule))]
    public class AudioPlayerModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        [Import]
        public ILeftMenuService LeftMenuService { get; set; }

        public void Initialize()
        {
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.ListRegion, typeof (AudioListView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.CurrentRegion, typeof (AudioCurrentView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterRegion, typeof (AudioFilterView));

            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(FilterMyView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(FilterFriendView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(FilterGroupView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(FilterAlbumsView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(SearchProviderView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(RecommendationsProviderView));
            this.RegionManager.RegisterViewWithRegion(AudioPlayerRegionNames.FilterContainerRegion,
                typeof(PopularProviderView));

            LeftMenuService.RegisterItem(ViewNames.AudioPlayerView, typeof(AudioPlayerView), null);
        }
    }
}
