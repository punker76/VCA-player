using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VKapi;
using Rina.Infastructure.Interfaces;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.Helpers
{
    public delegate Task<VKList<VKAudio>> LoadDelegate(Int32 offset, Int32 count, CancellationToken token);
    public static class AudioProviderHelper
    {
        public static async Task PartialLoading(LoadDelegate load, AddAudioItem addItem, Int32 loadCount, CancellationToken token)
        {
            Int32 currentCount = 20;
            Int32 currentOffset = 0;

            do
            {
                if (currentCount + currentOffset > loadCount) currentCount = loadCount - currentOffset + 1;

                VKList<VKAudio> list = await load(currentOffset, currentCount, token);

                if (list != null && list.Items != null)
                {
                    foreach (var item in list.Items)
                    {
                        token.ThrowIfCancellationRequested();
                        await addItem(item);
                    }
                }
                else
                {
                    break;
                }

                currentOffset += currentCount;
                currentCount *= 2;
            } while (currentOffset < loadCount);
        }
    }
}
