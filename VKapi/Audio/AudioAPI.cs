using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi;
using VKapi.Models;

namespace VKapi.Audio
{
    public sealed partial class AudioAPI : APIBase
    {
        protected override string GroupName
        {
            get { return "audio"; }
        }

        internal AudioAPI() { }
    }
}
