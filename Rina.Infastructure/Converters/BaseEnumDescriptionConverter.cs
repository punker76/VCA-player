using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Reflection;
using VkontakteAPI.Converters;

namespace Rina.Infastructure.Converters
{
    public abstract class BaseEnumDescriptionConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public IEnumerable<string> GetEnumDescription(Type destinationType)
        {
            var enumType = destinationType;

            var values = RetrieveEnumDescriptionValues(enumType);

            return new List<string>(values);
        }

        public object GetEnumFromDescription(string descToDecipher, Type destinationType)
        {
            var type = destinationType;
            if (!type.IsEnum) throw new InvalidOperationException();
            var staticFields = type.GetFields().Where(fld => fld.IsStatic);
            foreach (var field in staticFields)
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(VKDescriptionAttribute)) as VKDescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == descToDecipher)
                    {
                        return (Enum.Parse(type, field.Name, true));
                    }
                }
                else
                {
                    if (field.Name == descToDecipher)
                        return field.GetValue(null);
                }
            }
            throw new ArgumentException("Description is not found in enum list.");
        }

        public static string[] RetrieveEnumDescriptionValues(Type typeOfEnum)
        {
            var values = Enum.GetValues(typeOfEnum);

            return (from object fieldInfo in values select DescriptionAttr(fieldInfo)).ToArray();
        }

        public static string DescriptionAttr(object enumToQuery)
        {
            FieldInfo fi = enumToQuery.GetType().GetField(enumToQuery.ToString());

            VKDescriptionAttribute[] attributes = (VKDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(VKDescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : enumToQuery.ToString();
        }
    }
}
