using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using Rina.Modules.AudioPlayer.ViewModels.Providers;
using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Rina.Modules.AudioPlayer.ViewModels;
using System.Windows.Data;
using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Behaviors;

namespace Rina.Modules.AudioPlayer.Views.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ViewSortHint("07")]
    public partial class PopularProviderView : ActiveAwareUserControl
    {
        public PopularProviderView()
        {
            InitializeComponent();
        }

        [Import]
        public PopularListProviderViewModel ViewModel
        {
            get { return DataContext as PopularListProviderViewModel; }
            set { DataContext = value; }
        }
    }
}
