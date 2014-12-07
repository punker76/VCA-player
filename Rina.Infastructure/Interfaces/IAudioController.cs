using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;

namespace Rina.Infastructure.Interfaces
{
    public interface IAudioController
    {
        TimeSpan PlayPosition               { get; }
        Double DownloadProgress             { get; }
        ICollectionView ItemsView           { get; }
        DelegateCommand MoveNextCommand     { get; }
        DelegateCommand MovePrevCommand     { get; }
        IAudioListItemViewModel CurrentItem { get; }
        Double Volume                       { get; set; }
        Boolean IsShuffle                   { get; set; }
        Double PlayProgress                 { get; set; }
        String TextFilter                   { get; set; }
        Boolean IsRepeat                    { get; set; }

        void DownloadAll();
        Task RefreshAsync();
        Task<Boolean> MoveItem(IAudioListItemViewModel from, IAudioListItemViewModel to);
    }
}
