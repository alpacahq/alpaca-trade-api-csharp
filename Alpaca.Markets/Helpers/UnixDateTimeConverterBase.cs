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
            switch (value)
            {
                case DateTime dateTimeValue:
                    writer.WriteValue(IntoUnixTime(dateTimeValue));
                    break;

                case null:
                    writer.WriteNull();
                    break;
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
                : null;

        protected abstract Int64 IntoUnixTime(in DateTime value);

        protected abstract DateTime FromUnixTime(in Int64 value);
    }
}
