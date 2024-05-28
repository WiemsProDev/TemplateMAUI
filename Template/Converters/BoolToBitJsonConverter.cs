using System;
using Newtonsoft.Json;

namespace Template.Converters
{
    public class BoolToBitJsonConverter : JsonConverter
    {
        readonly string[] TrueStrings = { "true", "yes", "1" };
        public override bool CanConvert(Type objectType)
        {
            return typeof(bool) == objectType;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
                return reader.Value.ToString().Equals("1", StringComparison.InvariantCultureIgnoreCase) || reader.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase);
            else
                return false;

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            int bitVal = 0;
            if (value != null)
                bitVal = Convert.ToBoolean(value) ? 1 : 0; writer.WriteValue(bitVal);
        }
    }
}
