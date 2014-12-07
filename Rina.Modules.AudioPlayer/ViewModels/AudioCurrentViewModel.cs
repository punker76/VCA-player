using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel.Composition;
using Rina.Infastructure.Interfaces;
using Rina.Infastructure.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Rina.Modules.AudioPlayer.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AudioCurrentViewModel : NotificationObject
    {
        public IAudioController Audio { get; private set; }
        public ICommand DownloadAllCommand { get; private set; }

        [ImportingConstructor]
        public AudioCurrentViewModel(IAudioController audioController)
        {
            Audio = audioController;
            DownloadAllCommand = new DelegateCommand(Audio.DownloadAll);
        }
    }
}
