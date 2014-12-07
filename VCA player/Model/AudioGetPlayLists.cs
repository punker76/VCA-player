using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VCA_player.ViewModel;
using VKapi;
using VKapi.Audio;
using VKapi.Groups;
using VKapi.Models;
using VKapi.Wall;

namespace VCA_player.Model
{
    public static class AudioGetPlayLists
    {
        private static CancellationTokenSource _currentCts;

        public static async Task<IEnumerable<VKAudio>> GetFriendPlayList()
        {
            long friendId = MainViewModel.Instance.FriendsFilter.SelectedItem != null
                ? MainViewModel.Instance.FriendsFilter.SelectedItem.Item.Id
                : VKSession.Instance.UserId;

            CheckCurrentCTS();
            VKList<VKAudio> list = await AudioAPI.GetAsync((long) friendId, token: _currentCts.Token);
            return list.Items;
        }

        private static async Task<IEnumerable<VKAudio>> GetAllAudioFromPost(long ownerId, CancellationToken token)
        {
            int offset = 0;
            const int maxCount = 100;
            int allCount;
            int curCount = 0;

            Func<VKList<VKPost>, IEnumerable<VKAudio>> audioFilter = list => list.Items
                .Where(x => x.Attachments != null)
                .SelectMany(x => x.Attachments)
                .Where(x => x.Type == "audio")
                .Select(x => x.Audio)
                .Where(x => !String.IsNullOrWhiteSpace(x.Url));

            var lHead = await WallAPI.GetAsync(ownerId, count: maxCount, offset: offset, token: token);
            if (lHead == null) return null;

            curCount += maxCount;
            allCount = Convert.ToInt32(lHead.Count);

            while (curCount < allCount)
            {
                offset = curCount;
                var lCurr = await WallAPI.GetAsync(ownerId, count: maxCount, offset: offset, token: token);
                if (lCurr == null) break;

                lHead.Items = lHead.Items.Union(lCurr.Items);
                curCount += maxCount;
            }

            return audioFilter(lHead);
        }

        private static void CheckCurrentCTS()
        {
            if (_currentCts != null)
            {
                _currentCts.Cancel();
                _currentCts = null;
            }

            if (_currentCts == null)
            {
                _currentCts = new CancellationTokenSource();
            }
        }

        public static async Task<IEnumerable<VKAudio>> GetGroupPlayList()
        {
            VKGroup group = MainViewModel.Instance.GroupsFilter.SelectedItem.Item;
            if (group == null) return null;

            if (group.Type == VKGroup.TypeEnum.Page)
            {
                CheckCurrentCTS();

                try
                {
                    return (await GetAllAudioFromPost(-(long) group.Id, _currentCts.Token));
                }
                catch (OperationCanceledException)
                {
                }
            }
            if (group.Type == VKGroup.TypeEnum.Group)
            {
                CheckCurrentCTS();

                try
                {
                    VKList<VKAudio> list = (await AudioAPI.GetAsync(-(long) group.Id, token: _currentCts.Token));
                    return list.Items;
                }
                catch (OperationCanceledException)
                {
                }
            }

            return null;
        }
    }
}