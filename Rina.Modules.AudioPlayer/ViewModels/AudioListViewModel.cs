using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Interfaces;
using System.ComponentModel.Composition;
using Rina.Infastructure.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.Diagnostics.Contracts;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Data;
using System.ComponentModel;
using Microsoft.Practices.Prism.Events;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Rina.Modules.AudioPlayer.ViewModels
{
    [Export(typeof(AudioListViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AudioListViewModel : NotificationObject
    {
        public IAudioController Audio { get; private set; }

        [ImportingConstructor]
        public AudioListViewModel(IAudioController audioController)
        {
            Audio = audioController;
        }
    }
}