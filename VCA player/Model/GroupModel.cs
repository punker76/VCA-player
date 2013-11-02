using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Groups;
using System.ComponentModel;
using System.Collections.ObjectModel;
using VCA_player.Kernel;
using VKapi;

namespace VCA_player.Model
{
    class GroupModel
    {
        #region Events
        public event EventHandler<SelectedIndexChangedEventArgs> OnSelectedIndexChanged;
        public event EventHandler OnStartRefreshPlayList;
        public event EventHandler OnFinishRefreshPlayList;
        public event EventHandler OnLoadingStateChanged;
        #endregion

        #region Private Fields
        private int _selectedIndex;
        private bool _isLoading;
        #endregion

        #region Properties
        public ObservableCollection<VKGroup> Items { get; private set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value < 0 || value >= Items.Count) return;

                _selectedIndex = value;
                if (OnSelectedIndexChanged != null)
                {
                    OnSelectedIndexChanged(this, new SelectedIndexChangedEventArgs(_selectedIndex));
                }
            }
        }

        public VKGroup SelectedItem
        {
            get
            {
                if (SelectedIndex < 0 || SelectedIndex >= Items.Count) return null;

                return Items[SelectedIndex];
            }
            set
            {
                int idx = Items.IndexOf(value);
                if (idx < 0) return;
                SelectedIndex = idx;
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                if (OnLoadingStateChanged != null)
                {
                    OnLoadingStateChanged(this, new EventArgs());
                }
            }
        }
        #endregion

        #region Constructor
        public GroupModel()
        {
            Items = new ObservableCollection<VKGroup>();
            SelectedIndex = 0;
        }
        #endregion

        #region Public Methods
        public async void RefreshList()
        {
            try
            {
                IsLoading = true;
                if (OnStartRefreshPlayList != null)
                {
                    OnStartRefreshPlayList(this, new EventArgs());
                }

                Items.Clear();
                var list = await GroupRequest.GetExtendedAsync(VKSession.Instance.UserId);
                if (list == null) return;

                var listFilter = list.Items.Where(x => x.Type == VKGroup.TypeEnum.Page || x.Type == VKGroup.TypeEnum.Group);
                foreach (var item in listFilter)
                {
                    Items.Add(item);
                }
            }
            catch (NullReferenceException)
            {
            }
            finally
            {
                IsLoading = false;

                if (OnFinishRefreshPlayList != null)
                {
                    OnFinishRefreshPlayList(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
