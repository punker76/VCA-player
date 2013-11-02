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
using VKapi.Friends;

namespace VCA_player.ViewModel
{
    public class FriendsViewModel : ViewModelBase, IAudioFilterViewModel<VKFriend>/*, IAudioFilterViewModel*/
    {
        #region Private Fields
        private bool _isLoading = false;
        #endregion

        public VCAList<VKFriend> FilterModel { get; private set; }

        public VCAListItem<VKFriend> SelectedItem
        {
            get { return FilterModel.SelectedItem; }
            set { FilterModel.SelectedItem = value; }
        }
        //Object IAudioFilterViewModel.SelectedItem { get { return SelectedItem; } }
        public string SearchFilter
        {
            get { return FilterModel.Filter; }
            set { FilterModel.Filter = value; OnPropertyChanged(() => SearchFilter); }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;

                OnPropertyChanged(() => IsLoading);

                if (OnLoadingStateChanged != null)
                {
                    OnLoadingStateChanged(this, new LoadingStateChangedEventArgs(IsLoading));
                }
            }
        }
        public GetPlayListFunc GetPlayList { get { return AudioGetPlayLists.GetFriendPlayList; } }

        public ICommand RefreshCommand { get; private set; }

        #region Events
        public event EventHandler<SelectedChangedEventArgs<VKFriend>> OnSelectedChanged;
        public event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;
        #endregion

        public FriendsViewModel()
        {
            FilterModel = new VCAFriendList();

            FilterModel.OnSelectedChanged += FilterModel_OnSelectedChanged;
            FilterModel.OnStartRefreshList += FilterModel_OnStartRefreshList;
            FilterModel.OnFinishRefreshList += FilterModel_OnFinishRefreshList;
            this.RefreshCommand = new DelegateCommand(() =>
                {
                    SearchFilter = String.Empty;
                    FilterModel.Refresh();
                });
        }

        void FilterModel_OnFinishRefreshList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        void FilterModel_OnStartRefreshList(object sender, EventArgs e)
        {
            IsLoading = true;
        }

        void FilterModel_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKFriend> e)
        {
            OnPropertyChanged(() => SelectedItem);

            if (OnSelectedChanged != null)
            {
                OnSelectedChanged(this, e);
            }
        }
    }
}
