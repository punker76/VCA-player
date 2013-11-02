using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi;
using VKapi.Friends;
using VCA_player.Kernel;

namespace VCA_player.Model
{
    class VCAFriendList : VCAList<VKFriend>
    {
        private FriendFilterLogic _friendFilter = new FriendFilterLogic();
        protected override FilterLogicBase<VKFriend> FilterLogic { get { return _friendFilter; } }

        protected async override Task refreshList()
        {
            try
            {
                RaiseStartRefreshList();

                Items.Clear();
                var list = await FriendsRequest.GetExtendedAsync(
                    new[] { FriendsRequest.FieldsEnum.photo_50, FriendsRequest.FieldsEnum.online });
                if (list == null) return;

                foreach (var item in list.Items)
                {
                    Items.Add(new VCAListItem<VKFriend>(item));
                }
            }
            catch (NullReferenceException)
            {
            }
            finally
            {
                RaiseFinishRefreshList();
            }
        }
    }
}
