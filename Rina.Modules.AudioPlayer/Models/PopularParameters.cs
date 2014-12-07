using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel;
using VKapi;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.Models
{
    public sealed class PopularParameters : NotificationObject
    {
        private Boolean onlyEng = false;
        private VKGenreEnum genre = VKGenreEnum.All;

        public Boolean OnlyEng
        {
            get { return this.onlyEng; }
            set
            {
                this.onlyEng = value;
                RaisePropertyChanged(() => OnlyEng);
            }
        }

        public VKGenreEnum Genre
        {
            get { return this.genre; }
            set
            {
                this.genre = value;
                RaisePropertyChanged(() => Genre);
            }
        }

        public VKGenreEnum ListOfGenreVariants
        {
            get { return new VKGenreEnum(); }
        }
    }
}
