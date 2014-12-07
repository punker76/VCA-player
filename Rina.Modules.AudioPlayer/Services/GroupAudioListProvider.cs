using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using Rina.Modules.AudioPlayer.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VKapi;
using VKapi.Groups;
using VKapi.Models;
using VKapi.Wall;

namespace Rina.Modules.AudioPlayer.Services
{
    public sealed class GroupAudioListProvider : IAudioListProviderParameters<NotificationWrapper<VKGroup>>
    {
        private VKGroup group;

        public Boolean IsStateSet { get; private set; }
        public NotificationWrapper<VKGroup> State { get; set; }

        public async Task GetListAsync(AddAudioItem addItem, CancellationToken token)
        {
            Debug.Assert(this.group != null);

            switch (group.Type)
            {
                case VKGroupTypeEnum.Page:
                    await LoadAudiosFromWall(addItem, -group.Id, token);
                    break;
                case VKGroupTypeEnum.Group:
                    await LoadAudios(this.group.Id, addItem, token);
                    break;
                case VKGroupTypeEnum.Event:
                    await LoadAudios(this.group.Id, addItem, token);
                    break;
            }
        }

        public Boolean Update()
        {
            if (State.Content == null || this.group == State.Content) return false;

            this.group = State.Content;
            IsStateSet = true;

            return true;
        }

        private static async Task LoadAudios(Int64 groupId, AddAudioItem addItem, CancellationToken token)
        {
            Int32 count = await VKApi.Audio.GetCountAsync(-groupId, token);
            await AudioProviderHelper.PartialLoading((o, c, t) =>
                VKApi.Audio.GetAsync(-groupId, token: token),
                addItem, count, token);
        }

        private async Task LoadAudiosFromWall(AddAudioItem addItem, long ownerId, CancellationToken token)
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
            if (lHead == null) return;
            foreach (var item in audioFilter(lHead))
            {
                token.ThrowIfCancellationRequested();
                await addItem(item);
            }

            curCount += maxCount;
            allCount = Convert.ToInt32(lHead.Count);

            while (curCount < allCount)
            {
                offset = curCount;
                var lCurr = await WallAPI.GetAsync(ownerId, count: maxCount, offset: offset, token: token);
                if (lCurr == null) break;

                foreach (var item in audioFilter(lCurr))
                {
                    token.ThrowIfCancellationRequested();
                    await addItem(item);
                }

                curCount += maxCount;
                token.ThrowIfCancellationRequested();
            }
        }

    }
}
