using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using Rina.Modules.AudioPlayer.ViewModels.Providers;
using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Rina.Infastructure.Behaviors;

namespace Rina.Modules.AudioPlayer.Views.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ViewSortHint("05")]
    public partial class SearchProviderView : ActiveAwareUserControl
    {
        public SearchProviderView()
        {
            InitializeComponent();
        }

        [Import]
        public SearchProviderViewModel ViewModel
        {
            get { return DataContext as SearchProviderViewModel; }
            set { DataContext = value; }
        }

        private void DelayTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.State.Query = ((TextBox) e.Source).Text;
        }
    }
}
