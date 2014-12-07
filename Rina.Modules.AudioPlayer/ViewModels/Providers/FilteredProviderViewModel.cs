using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKapi.Audio;
using Rina.Infastructure.Interfaces;

namespace Rina.Modules.AudioPlayer.ViewModels.Providers
{
    public abstract class FilteredProviderViewModel<TItem, TParam> :
        AudioListProviderItemsViewModelBase<TItem, TParam>
        where TItem : class
        where TParam : IContentWrapper<TItem>, INotifyPropertyChanged, new()
    {
        private String filterText;

        protected abstract IFilterLogic<TItem> FilterLogic { get; }

        public String FilterText
        {
            get { return this.filterText; }
            set
            {
                this.filterText = value;
                RaisePropertyChanged(() => FilterText);

                UiDispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            ItemsView.MoveCurrentToPosition(-1);
                            ItemsView.Filter = (obj) => FilterLogic.Predicate(obj as TItem, FilterText);
                            ItemsView.MoveCurrentTo(AudioListProviderParameters.State.Content);
                        }),
                    System.Windows.Threading.DispatcherPriority.Background);
            }
        }
        
        public FilteredProviderViewModel(IAudioService audioService, IAudioController audioController)
            : base(audioService, audioController)
        {
            
        }
    }
}
