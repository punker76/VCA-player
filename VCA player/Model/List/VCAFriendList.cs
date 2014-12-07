using System;
using System.Threading.Tasks;
using VCA_player.Kernel.FilterLogic;
using VKapi.Friends;

namespace VCA_player.Model.List
{
    internal class VCAFriendList : VCAList<VKFriend>
    {
        private readonly FriendFilterLogic _friendFilter = new FriendFilterLogic();

        protected override FilterLogicBase<VKFriend> FilterLogic
        {
            get { return _friendFilter; }
        }

        protected override async Task RefreshList()
        {
            try
            {
                RaiseStartRefreshList();

                Items.Clear();
                var list = await FriendsAPI.GetExtendedAsync(
                    new[] {FriendsAPI.FieldsEnum.photo_50, FriendsAPI.FieldsEnum.online});
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