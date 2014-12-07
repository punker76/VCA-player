using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rina.Infastructure.Interfaces
{
    public interface IMenuViewItem
    {
        String Name { get; set; }
        Type ViewType { get; set; }
        Boolean IsSelected { get; set; }
        Object InformationView { get; set; }
        ICommand OpenCommand { get; }

        event EventHandler NavigateRequest;
    }
}
