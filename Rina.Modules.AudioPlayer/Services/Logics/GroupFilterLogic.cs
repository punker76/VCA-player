using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Models;
using Rina.Infastructure.Interfaces;
using System.Diagnostics;
using System.Globalization;

namespace Rina.Modules.AudioPlayer.Services.Logics
{
    public class GroupFilterLogic : IFilterLogic<VKGroup>
    {
        public Boolean Predicate(VKGroup item, String filter)
        {
            if (item == null) return false;
            String filterUpper = filter.ToUpper(CultureInfo.CurrentCulture);

            return item.Name.ToUpper(CultureInfo.CurrentCulture).Contains(filterUpper);
        }
    }
}
