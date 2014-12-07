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
    [ViewSortHint("02")]
    public partial class FilterFriendView : ActiveAwareUserControl
    {
        public FilterFriendView()
        {
            InitializeComponent();
        }

        [Import]
        public FriendsProviderViewModel ViewModel
        {
            get { return DataContext as FriendsProviderViewModel; }
            set { DataContext = value; }
        }
    }
}
