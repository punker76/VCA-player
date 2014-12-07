using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Rina.Infastructure.Extensions
{
    public static class VisualTreeExtensions
    {
        public static T FindParent<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            return (parent as T) ?? FindParent<T>(parent);
        }

    }
}
