using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using Rina.Modules.AudioPlayer.ViewModels.Providers;
using Rina.Infastructure.Behaviors;

namespace Rina.Modules.AudioPlayer.Views.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ViewSortHint("01")]
    public partial class FilterMyView : ActiveAwareUserControl
    {
        public FilterMyView()
        {
            InitializeComponent();
        }

        [Import]
        public MyAudioListProviderViewModel ViewModel
        {
            get { return DataContext as MyAudioListProviderViewModel; }
            set { DataContext = value; }
        }
    }
}
