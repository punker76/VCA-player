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
using VCA_player.ViewModel;

namespace VCA_player.View
{
    /// <summary>
    /// Interaction logic for AudioPlayerControl.xaml
    /// </summary>
    public partial class AudioPlayerControl : UserControl
    {
        public AudioPlayerControl()
        {
            this.DataContext = MainViewModel.Instance;//ViewModel.Instance;
            MainViewModel.Instance.AudioPlayerVM.RefreshCommand.Execute();
            InitializeComponent();
        }
    }
}
