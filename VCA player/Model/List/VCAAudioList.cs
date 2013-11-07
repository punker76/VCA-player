using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using VCA_player;
using VCA_player.Kernel;
using System.ComponentModel.Design;
using System.Windows.Input;
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;
using System.Windows;

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

        private void setOrder(IEnumerable<VCAListItem<VKAudio>> orderedCollection)
        {
            int curIndex = 0;

            foreach (var item in orderedCollection)
            {
                int idx = Items.IndexOf(item);
                Items.Insert(curIndex, item);
                if (curIndex <= idx) { idx++; }
                Items.RemoveAt(idx);
                curIndex++;
            }
        }

        public void ShuffleItems()
        {
            var res = Items.OrderBy((e => e.IsSelected ? Guid.Parse("{00000000-0000-0000-0000-000000000000}") : Guid.NewGuid()));
            setOrder(res);
        }

        public void RestoreItems()
        {
            var res = Items.OrderBy(e => e.Num);
            setOrder(res);
        }

        public VCAAudioList()
            : base()
        {
            GetPlayList = AudioGetPlayLists.GetFriendPlayList;
        }
    }
}
