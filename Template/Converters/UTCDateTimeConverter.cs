using System;
using Newtonsoft.Json;
namespace Template.Converters
{
    public class UTCDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }

            var tz = TimeZoneInfo.Local.BaseUtcOffset;
            DateTime d = _epoch.AddMilliseconds((long)reader.Value);
            if (d.IsDaylightSavingTime())
                return _epoch.AddMilliseconds((long)reader.Value).AddHours(tz.Hours + 1);
            else
                return _epoch.AddMilliseconds((long)reader.Value).AddHours(tz.Hours);

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            string time = ((DateTime)value - _epoch).TotalMilliseconds.ToString();
            time = time.Remove(time.Length - 1);
            writer.WriteRawValue(time);
        }
    }
}
