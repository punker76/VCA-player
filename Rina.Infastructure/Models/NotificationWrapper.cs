using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using Rina.Infastructure.Interfaces;

namespace Rina.Infastructure.Models
{
    public sealed class NotificationWrapper<TContent> : NotificationObject, IContentWrapper<TContent>
    {
        private TContent content;

        public TContent Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                RaisePropertyChanged(() => Content);
            }
        }
    }
}
