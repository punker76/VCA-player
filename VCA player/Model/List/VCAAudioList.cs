using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;
using VCA_player.Kernel;

namespace VCA_player.Model
{
    public delegate Task<IEnumerable<VKAudio>> GetPlayListFunc();
    public class SelectedIndexChangedEventArgs : EventArgs
    {
        public int Index { get; set; }

        public SelectedIndexChangedEventArgs(int index)
        {
            Index = index;
        }
    }

    public class SelectedChangedEventArgs<T> : EventArgs
        where T : class
    {
        public VCAListItem<T> Item { get; set; }

        public SelectedChangedEventArgs(VCAListItem<T> item)
        {
            Item = item;
        }
    }
    class VCAAudioList : VCAList<VKAudio>
    {
        public GetPlayListFunc GetPlayList;

        private AudioFilterLogic audioFilter = new AudioFilterLogic();
        protected override FilterLogicBase<VKAudio> FilterLogic { get { return audioFilter; } }
        protected async override Task refreshList()
        {
            try
            {
                RaiseStartRefreshList();

                Items.Clear();

                var list = await GetPlayList();
                if (list == null) return;

                foreach (var item in list)
                {
                    if (SelectedT != null && SelectedT.Id == item.Id)
                    {
                        var newItem = new VCAListItem<VKAudio>(item, true);
                        Items.Add(newItem);
                        SelectedItem = newItem;
                    }
                    else
                    {
                        Items.Add(new VCAListItem<VKAudio>(item));
                    }
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

        public VCAAudioList()
            : base()
        {
            GetPlayList = AudioGetPlayLists.GetFriendPlayList;
        }
    }
}
