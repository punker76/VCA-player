using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Models;
using System.Collections.ObjectModel;

namespace Rina.Infastructure.Interfaces
{
    public interface IDownloadManagerService
    {
        String Folder { get; set; }
        ObservableCollection<DownloadModel> Items { get; }

        void Start(String url, String title, DownloadType type);
        void RemoveCompletedItems();
    }
}
