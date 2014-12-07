using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using VkontakteAPI.Converters;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.Models
{
    public sealed class SearchParameters : NotificationObject
    {
        private String query = String.Empty;
        private Boolean autoComplete = true;
        private Boolean onlyWithLyrics = false;
        private Boolean onlyByArtist = false;
        private VKSearchSortEnum chosenSort = VKSearchSortEnum.ByRating;

        public VKSearchSortEnum ListOfSortVariants
        {
            get { return new VKSearchSortEnum(); }
        }

        public Boolean AutoComplete
        {
            get { return this.autoComplete; }
            set
            {
                this.autoComplete = value;
                RaisePropertyChanged(() => AutoComplete);
            }
        }

        public Boolean OnlyByArtist
        {
            get { return this.onlyByArtist; }
            set
            {
                this.onlyByArtist = value;
                RaisePropertyChanged(() => OnlyByArtist);
            }
        }

        public Boolean OnlyWithLyrics
        {
            get { return this.onlyWithLyrics; }
            set
            {
                this.onlyWithLyrics = value;
                RaisePropertyChanged(() => OnlyWithLyrics);
            }
        }

        public VKSearchSortEnum ChosenSort
        {
            get { return this.chosenSort; }
            set
            {
                this.chosenSort = value;
                RaisePropertyChanged(() => ChosenSort);
            }
        }

        public String Query
        {
            get { return this.query; }
            set
            {
                this.query = value;
                RaisePropertyChanged(() => Query);
            }
        }
    }
}
