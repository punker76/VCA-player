using System;
using System.Windows.Input;
using VCA_player.Kernel;
using VCA_player.Model;
using VCA_player.Model.List;
using VCA_player.ViewModel.Interface;
using VKapi.Groups;

namespace VCA_player.ViewModel.AudioFilter
{
    public class GroupsViewModel : ViewModelBase, IAudioFilterViewModel<VKGroup> /*, IAudioFilterViewModel*/
    {
        #region Private Fields

        private bool _isLoading = false;

        #endregion

        #region Events

        public event EventHandler<SelectedChangedEventArgs<VKGroup>> OnSelectedChanged;
        public event EventHandler<LoadingStateChangedEventArgs> OnLoadingStateChanged;

        #endregion

        #region Properties

        public VCAList<VKGroup> FilterModel { get; private set; }

        public VCAListItem<VKGroup> SelectedItem
        {
            get { return FilterModel.SelectedItem; }
            set { FilterModel.SelectedItem = value; }
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
            get { return AudioGetPlayLists.GetGroupPlayList; }
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

        #endregion

        #region Commands

        public ICommand RefreshCommand { get; private set; }

        #endregion

        #region Constructor

        public GroupsViewModel()
            : base()
        {
            FilterModel = new VCAGroupList();

            RefreshCommand = new DelegateCommand(() =>
            {
                SearchFilter = String.Empty;
                FilterModel.Refresh();
            }, () => !IsLoading);

            FilterModel.OnSelectedChanged += groupModel_OnSelectedChanged;
            FilterModel.OnStartRefreshList += groupModel_OnStartRefreshPlayList;
            FilterModel.OnFinishRefreshList += groupModel_OnFinishRefreshPlayList;
        }

        #endregion

        #region EventHandlers

        private void groupModel_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKGroup> e)
        {
            OnPropertyChanged(() => SelectedItem);

            if (OnSelectedChanged != null)
            {
                OnSelectedChanged(this, e);
            }
        }

        private void groupModel_OnFinishRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        private void groupModel_OnStartRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = true;
        }

        #endregion
    }
}