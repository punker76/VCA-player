using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using VkontakteAPI.Converters;

namespace Rina.Infastructure.Converters
{
    public class DescriptionToEnumConverter : BaseEnumDescriptionConverter, IValueConverter
    {
        public DescriptionToEnumConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DescriptionAttr(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = value;

            if (enumValue != null)
            {
                enumValue = GetEnumFromDescription(value.ToString(), targetType);
            }

            return enumValue;
        }
    }
}
