using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.MefExtensions;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using Rina.Modules.Auth;
using Rina.Modules.AudioPlayer;
using Microsoft.Practices.Prism.Logging;
using Rina.Infastructure;
using Rina.Modules.DownloadManager;
using Rina.StyleResources;


namespace Rina
{
    public class RinaBootstrapper : MefBootstrapper
    {
        private readonly EnterpriseLibraryLoggerAdapter _logger = new EnterpriseLibraryLoggerAdapter();

        protected override void ConfigureAggregateCatalog()
        {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (RegionNames).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (RinaBootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (AuthModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (AudioPlayerModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (DownloadManagerModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof (StyleResourcesModule).Assembly));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            return base.ConfigureDefaultRegionBehaviors();
        }

        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return _logger;
        }
    }
}
