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
using VKapi.Audio;
using VKapi.Wall;
using System.Windows.Data;
using VCA_player.Model;
using VKapi.Groups;
using VCA_player.Model;

namespace VCA_player.Kernel
{
    public abstract class FilterLogicBase<T>
        where T : class
    {
        public string SearchFilter { get; set; }

        private class ComparerSplit : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return y.Contains(x);
            }
            public int GetHashCode(string obj)
            {
                return 1;
            }
        }

        public abstract bool Filter(VCAListItem<T> item);

        protected bool checkContains(string strContains, string strCheck)
        {
            var splitContains = strContains.ToLower().Split(',', ' ', '&', '-').Where(x => !String.IsNullOrWhiteSpace(x));
            var splitCheck = strCheck.ToLower().Split(',', ' ', '&', '-').Where(x => !String.IsNullOrWhiteSpace(x));

            int count = splitContains.Intersect(splitCheck, new ComparerSplit()).Count<string>();

            return (count >= splitCheck.Count<string>());
        }
    }
}
