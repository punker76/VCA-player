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
using Rina.Modules.AudioPlayer.ViewModels;

namespace Rina.Modules.AudioPlayer.Views
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AudioPlayerView : UserControl
    {
        public AudioPlayerView()
        {
            InitializeComponent();
        }
    }
}
