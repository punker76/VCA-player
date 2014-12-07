using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Rina.Infastructure.Interfaces;

namespace Rina.Infastructure.Models
{
    public class AudioSelectedEvent : CompositePresentationEvent<IAudioListItemViewModel>
    {
    }
}
