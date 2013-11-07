using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCA_player.ViewModel;

namespace VCA_player.Model
{
    public class VCAListItem<T>: ViewModelBase
        where T : class
    {
        #region Private Fields
        private bool _isSelected = false;
        private bool _isShow = true;
        #endregion

        #region Properties
        public T Item { get; set; }
        public int Num { get; set; }
        public bool IsShow
        {
            get { return _isShow; }
            set { _isShow = value; OnPropertyChanged(() => IsShow); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(() => IsSelected); }
        }
        #endregion

        public VCAListItem(T item, bool isSelected = false, bool isShow = true, int num = 0)
        {
            Item = item;
            IsSelected = isSelected;
            IsShow = isShow;
            Num = num;
        }
    }
}
