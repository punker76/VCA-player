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
    [ViewSortHint("06")]
    public partial class RecommendationsProviderView : ActiveAwareUserControl
    {
        public RecommendationsProviderView()
        {
            InitializeComponent();
        }

        [Import]
        public RecommendationsProviderViewModel ViewModel
        {
            get { return DataContext as RecommendationsProviderViewModel; }
            set { DataContext = value; }
        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            ViewModel.State.Content = (e.Data.GetData(typeof(AudioListItemViewModel)) as IAudioListItemViewModel).Item.Audio;

            var expr = BindingOperations.GetMultiBindingExpression(CurrentAudioTextBlock, TextBlock.TextProperty);
            if (expr != null) expr.UpdateTarget();
        }
    }
}
