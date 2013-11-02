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
using VKapi;
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;
using VCA_player.View;
using VCA_player.Model;
using System.Globalization;
using System.Windows;

namespace VCA_player.ViewModel
{
    public class MyEnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ParameterString = parameter as string;
            if (ParameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object paramvalue = Enum.Parse(value.GetType(), ParameterString);
            return paramvalue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ParameterString = parameter as string;
            var valueAsBool = (bool)value;

            if (ParameterString == null || !valueAsBool)
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
            return Enum.Parse(targetType, ParameterString);
        }
    }

    public enum AudioFilters
    {
        My,
        Group,
        Album
    }

    public sealed class MainViewModel: ViewModelBase
    {
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

        private AudioFilters _selectedFilter;
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

        private static MainViewModel instance;
        public static MainViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainViewModel();
                    instance.Init();
                }
                return instance;
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
            this.ShowMyAudiosCommand = new DelegateCommand(() =>
                {
                    AudioPlayerVM.ShowMyAudios();
                });
            SelectedFilter = AudioFilters.My;
            AudioPlayerVM.OnLoadingStateChanged += audioPlayerVM_OnLoadingStateChanged;
        }
        #endregion

        #region Event Handlers
        void audioPlayerVM_OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            OnPropertyChanged(() => IsLoading);
        }

        void audioFilterVM_OnSelectedChanged(object sender, SelectedChangedEventArgs<Object> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }

        void friendsFilter_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKapi.Friends.VKFriend> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }

        void groupsFilter_OnSelectedChanged(object sender, SelectedChangedEventArgs<VKapi.Groups.VKGroup> e)
        {
            AudioPlayerVM.SearchFilter = String.Empty;
            AudioPlayerVM.RefreshCommand.Execute();
        }
        #endregion
    }
}
