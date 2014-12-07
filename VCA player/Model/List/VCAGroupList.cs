using System;
using System.Linq;
using System.Threading.Tasks;
using VCA_player.Kernel.FilterLogic;
using VKapi;
using VKapi.Groups;

namespace VCA_player.Model.List
{
    internal class VCAGroupList : VCAList<VKGroup>
    {
        private readonly GroupFilterLogic _groupFilter = new GroupFilterLogic();

        protected override FilterLogicBase<VKGroup> FilterLogic
        {
            get { return _groupFilter; }
        }


        protected override async Task RefreshList()
        {
            try
            {
                //IsLoading = true;
                RaiseStartRefreshList();

                Items.Clear();
                var list = await GroupAPI.GetExtendedAsync(VKSession.Instance.UserId);
                if (list == null) return;

                var listFilter =
                    list.Items.Where(x => x.Type == VKGroup.TypeEnum.Page || x.Type == VKGroup.TypeEnum.Group);
                foreach (var item in listFilter)
                {
                    Items.Add(new VCAListItem<VKGroup>(item));
                }
            }
            catch (NullReferenceException)
            {
            }
            finally
            {
/*
                IsLoading = false;
                */
                RaiseFinishRefreshList();
            }
        }
    }
}