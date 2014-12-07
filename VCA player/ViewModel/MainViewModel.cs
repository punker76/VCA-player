using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using VCA_player.Kernel;
using VCA_player.Model.List;
using VCA_player.ViewModel.AudioFilter;
using VKapi.Friends;
using VKapi.Groups;

namespace VCA_player.ViewModel
{
    public class MyEnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object paramvalue = Enum.Parse(value.GetType(), parameterString);
            return paramvalue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameterString = parameter as string;
            var valueAsBool = (bool) value;

            if (parameterString == null || !valueAsBool)
            {
                try
                {
                    return Enum.Parse(targetType, "0");
                }
                catch (Exception)
                {
                    return DependencyProperty.UnsetValue;
                }
            }
            return Enum.Parse(targetType, parameterString);
        }
    }

    public enum AudioFilters
    {
        My,
        Group,
        Album
    }

    public sealed class MainViewModel : ViewModelBase
    {
        private static MainViewModel _instance;
        private AudioFilters _selectedFilter;
        public FriendsViewModel FriendsFilter { get; private set; }
        public GroupsViewModel GroupsFilter { get; private set; }

        public AudioPlayerViewModel AudioPlayerVM { get; private set; }

        public bool IsLoading
        {
            get
            {
                bool res = AudioPlayerVM.IsLoading;

                switch (SelectedFilter)
                {
                    case AudioFilters.My:
                        res |= FriendsFilter.IsLoading;
                        break;
                    case AudioFilters.Group:
                        res |= GroupsFilter.IsLoading;
                        break;
                }

                return res;
            }
        }

        public AudioFilters SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                AudioPlayerVM.SearchFilter = String.Empty;
                FriendsFilter.OnSelectedChanged -= friendsFilter_OnSelectedChanged;
                FriendsFilter.OnLoadingStateChanged -= audioPlayerVM_OnLoadingStateChanged;
                GroupsFilter.OnSelectedChanged -= groupsFilter_OnSelectedChanged;
                GroupsFilter.OnLoadingStateChanged -= audioPlayerVM_OnLoadingStateChanged;

                switch (value)
                {
                    case AudioFilters.My:
                        FriendsFilter.OnSelectedChanged += friendsFilter_OnSelectedChanged;
                        FriendsFilter.OnLoadingStateChanged += audioPlayerVM_OnLoadingStateChanged;
                        AudioPlayerVM.SetPlayListGetter(FriendsFilter.GetPlayList);
                        FriendsFilter.RefreshCommand.Execute(null);
                        AudioPlayerVM.RefreshCommand.Execute();
                        break;
                    case AudioFilters.Group:
                        GroupsFilter.OnSelectedChanged += groupsFilter_OnSelectedChanged;
                        GroupsFilter.OnLoadingStateChanged += audioPlayerVM_OnLoadingStateChanged;
                        AudioPlayerVM.SetPlayListGetter(GroupsFilter.GetPlayList);
                        GroupsFilter.RefreshCommand.Execute(null);
                        AudioPlayerVM.RefreshCommand.Execute();
                        break;
                }

                OnPropertyChanged<AudioFilters>(() => SelectedFilter);
            }
        }

        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                    _instance.Init();
                }
                return _instance;
            }
        }

        #region Commands

        public ICommand ShowMyAudiosCommand { get; private set; }

        #endregion

        #region Constructor

        private void Init()
        {
            FriendsFilter = new FriendsViewModel();
            GroupsFilter = new GroupsViewModel();
            AudioPlayerVM = new AudioPlayerViewModel();
            ShowMyAudiosCommand = new DelegateCommand(() => AudioPlayerVM.ShowMyAudios());
            SelectedFilter = AudioFilters.My;
            AudioPlayerVM.OnLoadingStateChanged += audioPlayerVM_OnLoadingStateChanged;
        }

        #endregion

        #region Event Handlers

        private void audioPlayerVM_OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            OnPropertyChanged(() => IsLoading);
        }

        private void audioFilterVM_OnSelectedChanged(object sender, SelectedChangedEventArgs<Object> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }

        private void friendsFilter_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKFriend> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }

        private void groupsFilter_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKGroup> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }

        #endregion
    }
}