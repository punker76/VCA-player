using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel.Composition;
using Rina.Modules.DownloadManager.ViewModels;

namespace Rina.Modules.DownloadManager.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DownloadManagerIndicator : UserControl
    {
        public DownloadManagerIndicator()
        {
            InitializeComponent();
        }

        [Import]
        public DownloadManagerViewModel ViewModel
        {
            get { return DataContext as DownloadManagerViewModel; }
            set { DataContext = value; }
        }
    }
}
