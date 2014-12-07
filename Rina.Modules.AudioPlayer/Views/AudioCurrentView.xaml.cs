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
using Rina.Modules.AudioPlayer.ViewModels;
using System.ComponentModel.Composition;
using Rina.Infastructure.Extensions;

namespace Rina.Modules.AudioPlayer.Views
{
    [Export]
    public partial class AudioCurrentView : UserControl
    {
        private Window mainWindow;

        public AudioCurrentView()
        {
            InitializeComponent();
            mainWindow = this.FindParent<Window>();
        }

        [Import]
        private AudioCurrentViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.mainWindow == null)
                {
                    this.mainWindow = this.FindParent<Window>();
                }
                if (this.mainWindow == null) return;
                this.mainWindow.DragMove();
            }
        }
    }
}
