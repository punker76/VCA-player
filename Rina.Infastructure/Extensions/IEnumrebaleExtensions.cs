using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rina.Infastructure.Extensions
{
    public static class IEnumrebaleExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> handler)
        {
            foreach (var item in collection)
            {
                handler(item);
            }
        }
    }
}
