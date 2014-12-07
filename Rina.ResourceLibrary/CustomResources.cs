using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rina.ResourceLibrary
{
    public class CustomResources
    {
        public static ComponentResourceKey OriginalButtonKey
        {
            get
            {
                return new ComponentResourceKey(typeof (CustomResources), "OriginalButton");
            }
        }
    }
}
