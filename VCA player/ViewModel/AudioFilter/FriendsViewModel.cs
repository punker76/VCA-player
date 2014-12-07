using System;
using System.Windows.Input;
using VCA_player.Kernel;
using VCA_player.Model;
using VCA_player.Model.List;
using VCA_player.ViewModel.Interface;
using VKapi.Friends;

namespace VCA_player.ViewModel.AudioFilter
{
    public class FriendsViewModel : ViewModelBase, IAudioFilterViewModel<VKFriend> /*, IAudioFilterViewModel*/
    {
        #region Private Fields

        private bool _isLoading = false;

        #endregion

        public FriendsViewModel()
        {
            FilterModel = new VCAFriendList();

            FilterModel.OnSelectedChanged += FilterModel_OnSelectedChanged;
            FilterModel.OnStartRefreshList += FilterModel_OnStartRefreshList;
            FilterModel.OnFinishRefreshList += FilterModel_OnFinishRefreshList;
            RefreshCommand = new DelegateCommand(() =>
            {
                SearchFilter = String.Empty;
                FilterModel.Refresh();
            });
        }

        public VCAList<VKFriend> FilterModel { get; private set; }

        public VCAListItem<VKFriend> SelectedItem
        {
            get { return FilterModel.SelectedItem; }
            set { FilterModel.SelectedItem = value; }
        }

        public string SearchFilter
        {
            get { return FilterModel.Filter; }
            set
            {
                FilterModel.Filter = value;
                OnPropertyChanged(() => SearchFilter);
            }
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

        public GetPlayListFunc GetPlayList
        {
            get { return AudioGetPlayLists.GetFriendPlayList; }
        }

        public ICommand RefreshCommand { get; private set; }

        #region Events

        public event EventHandler<SelectedChangedEventArgs<VKFriend>> OnSelectedChanged;
        public event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;

        #endregion

        private void FilterModel_OnFinishRefreshList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        private void FilterModel_OnStartRefreshList(object sender, EventArgs e)
        {
            IsLoading = true;
        }

        private void FilterModel_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKFriend> e)
        {
            OnPropertyChanged(() => SelectedItem);

            if (OnSelectedChanged != null)
            {
                OnSelectedChanged(this, e);
            }
        }
    }
}