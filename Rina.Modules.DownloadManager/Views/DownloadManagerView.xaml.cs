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
using System.ComponentModel.Composition;
using Rina.Infastructure;
using Rina.Modules.DownloadManager.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Rina.Modules.DownloadManager.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DownloadManagerView : UserControl
    {
        public DownloadManagerView()
        {
            InitializeComponent();
        }

        [Import]
        public DownloadManagerViewModel ViewModel
        {
            get { return DataContext as DownloadManagerViewModel; }
            set { DataContext = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();

            dlg.Title = "Select Folder";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = UiFolder.Text;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = UiFolder.Text;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ViewModel.DownloadService.Folder = dlg.FileName;
            }
        }
    }
}
