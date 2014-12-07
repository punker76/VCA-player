using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using VKapi.Audio;
using VKapi.Models;
using System.Diagnostics;
using System.Globalization;

namespace Rina.Modules.AudioPlayer.Services.Logics
{
    public class FriendFilterLogic : IFilterLogic<VKFriend>
    {
        public Boolean Predicate(VKFriend item, String filter)
        {
            if (item == null) return false;

            String filterUpper = filter.ToUpper(CultureInfo.CurrentCulture);

            return item.FirstName.ToUpper(CultureInfo.CurrentCulture).Contains(filterUpper) ||
                   item.LastName.ToUpper(CultureInfo.CurrentCulture).Contains(filterUpper);
        }
    }
}
