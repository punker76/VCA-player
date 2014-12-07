using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Rina.Infastructure.Converters
{
    [ValueConversion(typeof(Double), typeof(Double))]
    public sealed class DoubleAdditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            if (parameter == null) return value;

            return System.Convert.ToDouble(value, CultureInfo.CurrentCulture) + System.Convert.ToDouble(parameter, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
