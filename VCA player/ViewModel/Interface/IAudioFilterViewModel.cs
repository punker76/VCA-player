using System;
using System.Windows.Input;
using VCA_player.Kernel;
using VCA_player.Model.List;

namespace VCA_player.ViewModel.Interface
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