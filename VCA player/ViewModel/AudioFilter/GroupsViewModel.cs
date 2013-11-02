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
    public class GroupsViewModel : ViewModelBase, IAudioFilterViewModel<VKGroup>/*, IAudioFilterViewModel*/
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
        public GetPlayListFunc GetPlayList { get { return AudioGetPlayLists.GetGroupPlayList; } }
        public string SearchFilter
        {
            get { return FilterModel.Filter; }
            set { FilterModel.Filter = value; OnPropertyChanged(() => SearchFilter); }
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

            this.RefreshCommand = new DelegateCommand(() =>
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
        void groupModel_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKGroup> e)
        {
            OnPropertyChanged(() => SelectedItem);

            if (OnSelectedChanged != null)
            {
                OnSelectedChanged(this, e);
            }
        }

        void groupModel_OnFinishRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = false;
        }

        void groupModel_OnStartRefreshPlayList(object sender, EventArgs e)
        {
            IsLoading = true;
        }
        #endregion
    }
}
