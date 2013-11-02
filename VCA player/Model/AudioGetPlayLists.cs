using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKapi.Audio;
using VKapi.Wall;
using VKapi.Groups;
using VCA_player.ViewModel;
using System.Windows;
using VKapi;

namespace VCA_player.Model
{

    static public class AudioGetPlayLists
    {
        public async static Task<IEnumerable<VKAudio>> GetFriendPlayList()
        {
            ulong friendId;

            if (MainViewModel.Instance.FriendsFilter.SelectedItem != null)
                friendId = MainViewModel.Instance.FriendsFilter.SelectedItem.Item.Id;
            else
                friendId = VKSession.Instance.UserId;

            VKList<VKAudio> list = await AudioRequest.GetAsync((long)friendId);
            return list.Items;
        }

        private async static Task<IEnumerable<VKAudio>> getAllAudioFromPost(long ownerId, CancellationToken token)
        {
            int offset = 0;
            int maxCount = 100;
            int allCount = 0;
            int curCount = 0;


            var lHead = await WallRequest.GetAsync(ownerId: ownerId, count: maxCount, offset: offset, token: token);
            if (lHead == null) return null;

            curCount += maxCount;
            allCount = Convert.ToInt32(lHead.Count);

            while (curCount < allCount)
            {
                offset = curCount;
                var lCurr = await WallRequest.GetAsync(ownerId: ownerId, count: maxCount, offset: offset, token: token);
                if (lCurr == null) break;

                lHead.Items = lHead.Items.Union(lCurr.Items);
                curCount += maxCount;
            }

            return lHead.Items
                    .Where(x => x.Attachments != null)
                    .SelectMany(x => x.Attachments)
                    .Where(x => x.Type == "audio")
                    .Select(x => x.Audio)
                    .Where(x => !String.IsNullOrWhiteSpace(x.Url)); ;
        }

        private static CancellationTokenSource currentCTS;
        private static void checkCurrentCTS()
        {
            if (currentCTS != null)
            {
                currentCTS.Cancel();
                currentCTS = null;
            }

            if (currentCTS == null)
            {
                currentCTS = new CancellationTokenSource();
            }
        }
        public async static Task<IEnumerable<VKAudio>> GetGroupPlayList()
        {
            VKGroup group = MainViewModel.Instance.GroupsFilter.SelectedItem.Item;
            if (group == null) return null;
            
            if (group.Type == VKGroup.TypeEnum.Page)
            {
                checkCurrentCTS();

                try
                {
                    return (await getAllAudioFromPost(-(long)group.Id, currentCTS.Token));
                }
                catch (OperationCanceledException) { }
            }
            if (group.Type == VKGroup.TypeEnum.Group)
            {
                checkCurrentCTS();

                try
                {
                    VKList<VKAudio> list = (await AudioRequest.GetAsync(-(long)group.Id, token: currentCTS.Token));
                    return list.Items;
                }
                catch (OperationCanceledException) { }
            }

            return null;
        }
    }
}
