using System.Threading.Tasks;

namespace VCA_player.Kernel
{
    public static class Empty<T>
    {
        private static readonly Task<T> Task = System.Threading.Tasks.Task.FromResult(default(T));
    }
}