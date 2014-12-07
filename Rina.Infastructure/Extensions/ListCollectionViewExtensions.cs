using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.ComponentModel;

namespace Rina.Infastructure.Extensions
{
    public static class ListCollectionViewExtensions
    {
        public static void Clear(this ListCollectionView view)
        {
            if (view.IsAddingNew)
                view.CommitNew();
            if (view.IsEditingItem)
                view.CommitEdit();

            if (view.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None)
                view.NewItemPlaceholderPosition = NewItemPlaceholderPosition.None;

            while (view.Count > 0)
            {
                var theItem = view.GetItemAt(0);
                view.Remove(theItem);
            }

            view.Refresh();
        }
    }
}
