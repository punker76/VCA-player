using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Rina.Infastructure;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel.Composition;

namespace Rina.Services
{
    [Export(typeof(IApplicationCommands))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class ApplicationCommands : IApplicationCommands
    {
        private readonly IRegionManager regionManager;
        private readonly IServiceLocator serviceLocator;

        [ImportingConstructor]
        public ApplicationCommands(IRegionManager regions, IServiceLocator serviceLocator)
        {
            this.regionManager = regions;
            this.serviceLocator = serviceLocator;
        }

        public void CloseView(Object view)
        {
            this.regionManager.Regions[RegionNames.MainRegion].Remove(view);
        }

        public void OpenView(Type viewType)
        {
            var region = regionManager.Regions[RegionNames.MainRegion];
            var view = this.serviceLocator.GetInstance(viewType);

            if (!region.Views.Contains(view))
            {
                region.Add(view);
            }

            region.Activate(view);
        }
    }
}
