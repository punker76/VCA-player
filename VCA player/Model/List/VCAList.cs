using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows;

namespace VCA_player.Model
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
            if (OnSelectedChanged != null) { OnSelectedChanged(this, new SelectedChangedEventArgs<T>(item)); }
        }
        public void RaiseStartRefreshList()
        {
            if (OnStartRefreshList != null) { OnStartRefreshList(this, new EventArgs()); }
        }
        public void RaiseFinishRefreshList()
        {
            if (OnFinishRefreshList != null) { OnFinishRefreshList(this, new EventArgs()); }
        }
        #endregion

        #region Private Fields
        protected abstract FilterLogicBase<T> FilterLogic { get; }
        private VCAListItem<T> _lastSelected;
        private VCAListItem<T> _selected;
        private object lock_items = new object();
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
                        lock (lock_items)
                        {
                            for (int i = 0; i < Items.Count; i++) { Items[i].IsShow = false; }

                            foreach (var item in Items.Where(FilterLogic.Filter))
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
            get
            {
                return _selected;
            }
            set
            {
                if (value != null) { SelectedT = value.Item; }
                if (_lastSelected != null) { _lastSelected.IsSelected = false; }
                _lastSelected = value;

                _selected = value;
                if (_selected != null) { _selected.IsSelected = true; }

                RaiseSelectedChanged(_selected);
            }
        }
        #endregion

        #region Constructor
        public VCAList()
        {
            Items = new ObservableCollection<VCAListItem<T>>();
        }
        #endregion

        #region Methods
        protected abstract Task refreshList();
        public void Refresh()
        {
            lock (lock_items)
            {
                refreshList();
            }
        }
        #endregion
    }
}
