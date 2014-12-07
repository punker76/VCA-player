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
    [ViewSortHint("03")]
    public partial class FilterGroupView : ActiveAwareUserControl
    {
        public FilterGroupView()
        {
            InitializeComponent();
        }

        [Import]
        public GroupsProviderViewModel ViewModel
        {
            get { return DataContext as GroupsProviderViewModel; }
            set { DataContext = value; }
        }
    }
}
