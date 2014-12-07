using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rina.Infastructure.Interfaces
{
    public interface IRegisterMenuService
    {
        void NavigateTo(String name);
        void NavigateTo(IMenuViewItem item);
        void RegisterItem(String name, Type viewType, Type informationViewType);
    }
}
