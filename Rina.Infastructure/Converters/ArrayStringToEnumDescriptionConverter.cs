using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace Rina.Infastructure.Converters
{
    public class ArrayStringToEnumDescriptionConverter : BaseEnumDescriptionConverter, IValueConverter
    {
        public ArrayStringToEnumDescriptionConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.GetType();
            return !type.IsEnum ? null : base.GetEnumDescription(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
