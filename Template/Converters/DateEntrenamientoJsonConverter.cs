using System;
using Newtonsoft.Json;

namespace Template.Converters
{
    public class DateEntrenamientoJsonConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                try
                {
                    long millis = (long)reader.Value;
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(millis).ToLocalTime();



                    return date.AddMilliseconds(1);
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {

            if (value != null)
            {
                try
                {
                    DateTime date = (DateTime)value;
                    writer.WriteValue(date.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                catch
                {

                }
            }

        }
    }
}

