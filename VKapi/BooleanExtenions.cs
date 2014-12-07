using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKapi
{
    internal static class BooleanExtenions
    {
        public static Int32 ToVKValue(this Boolean value)
        {
            return value ? 1 : 0;
        }
    }
}
