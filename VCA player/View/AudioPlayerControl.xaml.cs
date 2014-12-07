using System.Windows.Controls;
using VCA_player.ViewModel;

namespace VCA_player.View
{
    /// <summary>
    ///     Interaction logic for AudioPlayerControl.xaml
    /// </summary>
    public partial class AudioPlayerControl : UserControl
    {
        public AudioPlayerControl()
        {
            DataContext = MainViewModel.Instance; //ViewModel.Instance;
            MainViewModel.Instance.AudioPlayerVM.RefreshCommand.Execute();
            InitializeComponent();
        }
    }
}