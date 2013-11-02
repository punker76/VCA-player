using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;

namespace VKapi
{
    public class JsonUnixTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long timestamp = serializer.Deserialize<long>(reader);

            return (new DateTime(1970, 1, 1, 0, 0, 0, 0))
                .AddSeconds(timestamp);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonBoolConvereter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            byte flag = serializer.Deserialize<byte>(reader);

            return (flag == 1);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonEnumConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override void WriteJson(JsonWriter writer, object
        value, JsonSerializer serializer)
        {
            writer.WriteValue(((T)value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type
        objectType, object existingValue, JsonSerializer serializer)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                string val = EnumString.GetStringValue(item as Enum);
                if (val == reader.Value.ToString())
                {
                    return item;
                }
            }

            return Enum.Parse(typeof(T), reader.Value.ToString());
        }
    }
}
