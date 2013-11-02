using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Groups;
using VCA_player.Kernel;
using VKapi;

namespace VCA_player.Model
{
    class VCAGroupList : VCAList<VKGroup>
    {
        private GroupFilterLogic _groupFilter = new GroupFilterLogic();
        protected override FilterLogicBase<VKGroup> FilterLogic { get { return _groupFilter; } }


        protected async override Task refreshList()
        {
            try
            {
                //IsLoading = true;
                RaiseStartRefreshList();

                Items.Clear();
                var list = await GroupRequest.GetExtendedAsync(VKSession.Instance.UserId);
                if (list == null) return;

                var listFilter = list.Items.Where(x => x.Type == VKGroup.TypeEnum.Page || x.Type == VKGroup.TypeEnum.Group);
                foreach (var item in listFilter)
                {
                    Items.Add(new VCAListItem<VKGroup>(item));
                }
            }
            catch (NullReferenceException)
            {
            }
            finally
            {/*
                IsLoading = false;
                */
                RaiseFinishRefreshList();
            }
        }
    }
}
