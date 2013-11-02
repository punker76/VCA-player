using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.ComponentModel;
using VCA_player;
using VCA_player.Kernel;
using System.ComponentModel.Design;
using System.Windows.Input;
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;
using VCA_player.Model;
using VKapi.Groups;

namespace VCA_player.ViewModel
{
    /*public interface IAudioFilterViewModel
    {
        bool IsLoading { get; }
        Object SelectedItem { get; set; }
    }*/
    
    public interface IAudioFilterViewModel<T>
        where T : class
    {
        VCAList<T> FilterModel { get; }
        VCAListItem<T> SelectedItem { get; set; }
        string SearchFilter { get; set; }
        bool IsLoading { get; }
        GetPlayListFunc GetPlayList { get; }

        ICommand RefreshCommand { get; }

        event EventHandler<SelectedChangedEventArgs<T>> OnSelectedChanged;
        event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;
    }
}
