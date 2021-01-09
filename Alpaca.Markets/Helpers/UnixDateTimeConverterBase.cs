using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    internal abstract class UnixDateTimeConverterBase : DateTimeConverterBase
    {
        public sealed override void WriteJson(
            JsonWriter writer, 
            Object? value, 
            JsonSerializer serializer)
        {
            if (value is DateTime dateTimeValue)
            {
                writer.WriteValue(IntoUnixTime(dateTimeValue));
            }
            else if (value is null)
            {
                writer.WriteNull();
            }
        }

        public sealed override Object? ReadJson(
            JsonReader reader, 
            Type objectType,
            Object? existingValue, 
            JsonSerializer serializer) =>
            Int64.TryParse(reader.Value?.ToString(), 
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var unixTimeStamp)
                ? FromUnixTime(unixTimeStamp)
                : (Object?) null;

        protected abstract Int64 IntoUnixTime(in DateTime value);

        protected abstract DateTime FromUnixTime(in Int64 value);
    }
}
