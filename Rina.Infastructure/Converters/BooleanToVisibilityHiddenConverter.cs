using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Rina.Infastructure.Converters
{
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public sealed class BooleanToVisibilityHiddenConverter : BooleanToVisibilityConverter
    {
        public BooleanToVisibilityHiddenConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
        }
    }
}
