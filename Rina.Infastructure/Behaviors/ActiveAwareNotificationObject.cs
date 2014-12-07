using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.ViewModel;

namespace Rina.Infastructure.Behaviors
{
    public class ActiveAwareNotificationObject : NotificationObject, IActiveAware
    {
        private Boolean isActive;

        public Boolean IsActive
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
