using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCA_player.Kernel
{
    public static class Empty<T>
    {
        public static Task<T> Task { get { return _task; } }

        private static readonly Task<T> _task = System.Threading.Tasks.Task.FromResult(default(T));
    }
}
