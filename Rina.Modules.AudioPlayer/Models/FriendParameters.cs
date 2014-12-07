using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using Microsoft.Practices.Prism.ViewModel;
using VKapi.Friends;
using VKapi.Audio;
using VkontakteAPI;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.Models
{
    public sealed class FriendParameters : NotificationObject, IContentWrapper<VKFriend>
    {
        private VKFriend content;
        private VKAlbum album;

        public VKFriend Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                RaisePropertyChanged(() => Content);
            }
        }

        public VKAlbum Album
        {
            get
            {
                return this.album;
            }
            set
            {
                this.album = value;
                RaisePropertyChanged(() => Album);
            }
        }
    }
}
