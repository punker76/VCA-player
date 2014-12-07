using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Data;
using Rina.Infastructure.Interfaces;

namespace Rina.Infastructure.Behaviors
{
    public class ActiveAwareUserControl : UserControl, IActiveAware
    {
        private bool isActive;

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;

                    var viewModelActiveAware = this.DataContext as IActiveAware;
                    if (viewModelActiveAware != null)
                        viewModelActiveAware.IsActive = value;

                    this.OnIsActiveChanged();
                }
            }
        }

        public event EventHandler IsActiveChanged;

        protected virtual void OnIsActiveChanged()
        {
            var handler = this.IsActiveChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
