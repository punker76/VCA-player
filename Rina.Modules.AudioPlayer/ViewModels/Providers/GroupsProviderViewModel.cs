using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Rina.Infastructure.Interfaces;
using Rina.Modules.AudioPlayer.Services;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics.Contracts;
using VKapi.Friends;
using VKapi.Groups;
using VKapi.Audio;
using VKapi;
using System.Collections.ObjectModel;
using System.Windows;
using Rina.Modules.AudioPlayer.Services.Logics;
using Rina.Infastructure.Models;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class GroupsProviderViewModel :  FilteredProviderViewModel<VKGroup, NotificationWrapper<VKGroup>>
    {
        private readonly IAudioListProviderParameters<NotificationWrapper<VKGroup>> audioListProviderParameters = new GroupAudioListProvider();
        private readonly IFilterLogic<VKGroup> filterLogic = new GroupFilterLogic();

        protected override IFilterLogic<VKGroup> FilterLogic
        {
            get { return this.filterLogic; }
        }

        public override IAudioListProviderParameters<NotificationWrapper<VKGroup>> AudioListProviderParameters
        {
            get { return this.audioListProviderParameters; }
        }

        public override string HeaderInfo
        {
            get { return "Группы"; }
        }
        
        [ImportingConstructor]
        public GroupsProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            
        }

        protected override async Task<IEnumerable<VKGroup>> RefreshCore()
        {
            var list = await GroupAPI.GetExtendedAsync(VKSession.Instance.UserId);

            return list.Items;
        }
    }
}
