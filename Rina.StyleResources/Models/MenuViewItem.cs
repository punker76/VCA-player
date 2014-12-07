using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Rina.Infastructure.Interfaces;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel.Composition;
using System.Threading;

namespace Rina.StyleResources.Models
{
    [Export(typeof(IMenuViewItem))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class MenuViewItem : NotificationObject, IMenuViewItem
    {
        private String name;
        private Type viewType;
        private Object informationView;
        private Boolean isSelected;

        public String Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public Type ViewType
        {
            get { return this.viewType; }
            set
            {
                this.viewType = value;
                RaisePropertyChanged(() => ViewType);
            }
        }

        public Boolean IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        public Object InformationView
        {
            get { return this.informationView; }
            set
            {
                this.informationView = value;
                RaisePropertyChanged(() => InformationView);
            }
        }

        public ICommand OpenCommand { get; private set; }

        public event EventHandler NavigateRequest;

        [ImportingConstructor]
        public MenuViewItem()
        {
            OpenCommand = new DelegateCommand(() =>
            {
                RaiseEvent(ref NavigateRequest);
            });
        }

        private void RaiseEvent(ref EventHandler handler)
        {
            var handlerSafe = Volatile.Read(ref NavigateRequest);
            if (handlerSafe != null)
            {
                handlerSafe(this, EventArgs.Empty);
            }
        }
    }
}
