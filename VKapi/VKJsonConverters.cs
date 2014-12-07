using System;
using Newtonsoft.Json;

namespace VKapi
{
    public class JsonUnixTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            Int64 timestamp = serializer.Deserialize<Int64>(reader);

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
            return (objectType == typeof (string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            Int16 flag = serializer.Deserialize<Int16>(reader);

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
            return objectType == typeof (T);
        }

        public override void WriteJson(JsonWriter writer, object
            value, JsonSerializer serializer)
        {
            writer.WriteValue(((T) value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type
            objectType, object existingValue, JsonSerializer serializer)
        {
            foreach (var item in Enum.GetValues(typeof (T)))
            {
                string val = EnumString.GetStringValue(item as Enum);
                if (val == reader.Value.ToString())
                {
                    return item;
                }
            }

            return Enum.Parse(typeof (T), reader.Value.ToString());
        }
    }
}