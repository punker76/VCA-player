using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VCA_player.Kernel.FilterLogic;

namespace VCA_player.Model.List
{
    public abstract class VCAList<T>
        where T : class
    {
        #region Events

        public event EventHandler<SelectedChangedEventArgs<T>> OnSelectedChanged;
        public event EventHandler OnStartRefreshList;
        public event EventHandler OnFinishRefreshList;

        #endregion

        #region EventsRaise

        public void RaiseSelectedChanged(VCAListItem<T> item)
        {
            if (OnSelectedChanged != null)
            {
                OnSelectedChanged(this, new SelectedChangedEventArgs<T>(item));
            }
        }

        public void RaiseStartRefreshList()
        {
            if (OnStartRefreshList != null)
            {
                OnStartRefreshList(this, new EventArgs());
            }
        }

        public void RaiseFinishRefreshList()
        {
            if (OnFinishRefreshList != null)
            {
                OnFinishRefreshList(this, new EventArgs());
            }
        }

        #endregion

        #region Private Fields

        private readonly object _lockItems = new object();
        protected abstract FilterLogicBase<T> FilterLogic { get; }
        private VCAListItem<T> LastSelected { get; set; }
        private VCAListItem<T> Selected { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<VCAListItem<T>> Items { get; protected set; }

        public string Filter
        {
            get { return FilterLogic.SearchFilter; }
            set
            {
                FilterLogic.SearchFilter = value;
                new Thread(() =>
                {
                    lock (_lockItems)
                    {
                        foreach (VCAListItem<T> t in Items)
                        {
                            t.IsShow = false;
                        }

                        var items = Items.Where(FilterLogic.Filter);

                        foreach (var item in items)
                        {
                            item.IsShow = true;
                        }
                    }
                }).Start();
            }
        }

        public T SelectedT { get; set; }

        public VCAListItem<T> SelectedItem
        {
            get { return Selected; }
            set
            {
                if (value != null)
                {
                    SelectedT = value.Item;
                }
                if (LastSelected != null)
                {
                    LastSelected.IsSelected = false;
                }
                LastSelected = value;

                Selected = value;
                if (Selected != null)
                {
                    Selected.IsSelected = true;
                }

                RaiseSelectedChanged(Selected);
            }
        }

        #endregion

        #region Constructor

        protected VCAList()
        {
            Items = new ObservableCollection<VCAListItem<T>>();
        }

        #endregion

        #region Methods

        protected abstract Task RefreshList();

        public void Refresh()
        {
            lock (_lockItems)
            {
                RefreshList();
            }
        }

        #endregion
    }
}