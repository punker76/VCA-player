using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Rina.Infastructure.Interfaces;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Rina.Modules.DownloadManager.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class DownloadManagerViewModel
    {
        public IDownloadManagerService DownloadService { get; private set; }
        public ICommand RemoveCompeltedItemsCommand { get; private set; }

        [ImportingConstructor]
        public DownloadManagerViewModel(IDownloadManagerService downloadService)
        {
            DownloadService = downloadService;
            RemoveCompeltedItemsCommand = new DelegateCommand(DownloadService.RemoveCompletedItems);
        }
    }
}
