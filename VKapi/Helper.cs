using System;
using System.Reflection;

namespace VKapi
{
    public class EnumValue : Attribute
    {
        private String _value;

        public EnumValue(String value)
        {
            _value = value;
        }

        public String Value
        {
            get { return _value; }
        }
    }

    public static class EnumString
    {
        public static String GetStringValue(Enum value)
        {
            String output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            EnumValue[] attrs = fi.GetCustomAttributes(typeof (EnumValue), false) as EnumValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}