using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;

namespace VKapi
{
    public static class VKApi
    {
        private static readonly AudioAPI audio = new AudioAPI();

        public static AudioAPI Audio { get { return audio; } }
    }
}
