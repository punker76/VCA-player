using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Rina.Infastructure.Converters
{
    [ValueConversion(typeof(DateTime), typeof(String))]
    public sealed class TimeSpanFromatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan)) return null;

            TimeSpan dt = (TimeSpan)value;

            return ((dt < TimeSpan.Zero) ? "-" : "") +
                   ((dt.Hours == 0)
                       ? dt.ToString(@"mm\:ss", CultureInfo.InvariantCulture)
                       : dt.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
