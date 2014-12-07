using System;
using System.Collections.Generic;
using System.Linq;
using VCA_player.Model.List;

namespace VCA_player.Kernel.FilterLogic
{
    public abstract class FilterLogicBase<T>
        where T : class
    {
        public string SearchFilter { get; set; }

        public abstract bool Filter(VCAListItem<T> item);

        protected bool CheckContains(string strContains, string strCheck)
        {
            var splitContains = strContains.ToLower()
                .Split(',', ' ', '&', '-')
                .Where(x => !String.IsNullOrWhiteSpace(x));
            var splitCheck = strCheck.ToLower().Split(',', ' ', '&', '-').Where(x => !String.IsNullOrWhiteSpace(x));

            IEnumerable<string> check = splitCheck as string[] ?? splitCheck.ToArray();
            int count = splitContains.Intersect(check, new ComparerSplit()).Count();

            return (count >= check.Count());
        }

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
    }
}