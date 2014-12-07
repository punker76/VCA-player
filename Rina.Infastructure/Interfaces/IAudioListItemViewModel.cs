using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rina.Infastructure.Models;

namespace Rina.Infastructure.Interfaces
{
    public interface IAudioListItemViewModel
    {
        ICommand AddCommand { get; }
        ICommand PlayCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand RestoreCommand { get; }
        ICommand DownloadCommand { get; }
        ICommand ChangeStateCommand { get; }
        ICommand SwitchLyricsStateCommand { get; }

        PlayState State { get; }
        Boolean IsAdded { get; }
        Boolean IsDeleted { get; }
        Boolean ShowLyrics { get; }
        Boolean IsOwnAudio { get; }
        TimeSpan PlayProgress { get; }
        Double DownloadProgress { get; }
        AudioListItemModel Item { get; set; }
    }
}
