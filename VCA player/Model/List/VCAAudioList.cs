using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VCA_player.Kernel.FilterLogic;
using VKapi.Audio;

namespace VCA_player.Model.List
{
    public delegate Task<IEnumerable<VKAudio>> GetPlayListFunc();

    public class SelectedIndexChangedEventArgs : EventArgs
    {
        public SelectedIndexChangedEventArgs(int index)
        {
            Index = index;
        }

        public int Index { get; set; }
    }

    public class SelectedChangedEventArgs<T> : EventArgs
        where T : class
    {
        public SelectedChangedEventArgs(VCAListItem<T> item)
        {
            Item = item;
        }

        public VCAListItem<T> Item { get; set; }
    }

    internal class VCAAudioList : VCAList<VKAudio>
    {
        private readonly AudioFilterLogic _audioFilter = new AudioFilterLogic();
        public GetPlayListFunc GetPlayList;

        public VCAAudioList()
            : base()
        {
            GetPlayList = AudioGetPlayLists.GetFriendPlayList;
        }

        protected override FilterLogicBase<VKAudio> FilterLogic
        {
            get { return _audioFilter; }
        }

        protected override async Task RefreshList()
        {
            try
            {
                RaiseStartRefreshList();

                Items.Clear();

                var list = await GetPlayList();
                if (list == null) return;
                int curNum = 0;
                foreach (var item in list)
                {
                    if (SelectedT != null && SelectedT.Id == item.Id)
                    {
                        var newItem = new VCAListItem<VKAudio>(item, true, num: curNum);
                        Items.Add(newItem);
                        SelectedItem = newItem;
                    }
                    else
                    {
                        Items.Add(new VCAListItem<VKAudio>(item, num: curNum));
                    }

                    curNum++;
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

        public void ShuffleItems()
        {
            Random random = new Random();
            int currentIndex = -1;
            for (int i = Items.Count; i > 0;)
            {
                int j = Convert.ToInt32(random.NextDouble()*i);
                var x = Items[--i];
                Items[i] = Items[j];
                if (Items[i].IsSelected)
                {
                    currentIndex = i;
                }
                Items[j] = x;
            }
            if (currentIndex != -1)
            {
                var x = Items[currentIndex];
                Items[currentIndex] = Items[0];
                Items[0] = x;
            }
        }

        public void RestoreItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var num = Items[i].Num;
                if (num == i)
                {
                    continue;
                }
                var x = Items[num];
                Items[num] = Items[i];
                Items[i] = x;
            }
        }
    }
}