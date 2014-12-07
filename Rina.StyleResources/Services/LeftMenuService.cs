using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using System.ComponentModel.Composition;
using Microsoft.Practices.ServiceLocation;
using Rina.StyleResources.Models;

namespace Rina.StyleResources.Services
{
    [Export(typeof(ILeftMenuService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LeftMenuService : ILeftMenuService
    {
        private readonly IServiceLocator serviceLocator;
        private readonly IApplicationCommands commands;

        private IMenuViewItem previousItem;

        public ObservableCollection<IMenuViewItem> MenuItems { get; private set; }

        [ImportingConstructor]
        public LeftMenuService(IServiceLocator serviceLocator, IApplicationCommands commands)
        {
            this.commands = commands;
            this.serviceLocator = serviceLocator;

            MenuItems = new ObservableCollection<IMenuViewItem>();
        }

        public void NavigateTo(String name)
        {
            var item = MenuItems.FirstOrDefault(v => v.Name == name);

            if (item == null) return;

            NavigateTo(item);
        }

        public void NavigateTo(IMenuViewItem item)
        {
            if (item == null) return;
            if (previousItem != null) previousItem.IsSelected = false;
            previousItem = item;
            item.IsSelected = true;

            this.commands.OpenView(item.ViewType);
        }

        public void RegisterItem(String name, Type viewType, Type informationViewType)
        {
            var item = this.serviceLocator.GetInstance<IMenuViewItem>();
            var infoView = informationViewType != null ? this.serviceLocator.GetInstance(informationViewType) : null;
            item.NavigateRequest += (s, e) => NavigateTo(s as IMenuViewItem);

            item.Name = name;
            item.ViewType = viewType;
            item.InformationView = infoView;

            MenuItems.Add(item);
        }
    }
}
