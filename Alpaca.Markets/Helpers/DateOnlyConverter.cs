using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class DateOnlyConverter : JsonConverter
    {
        private const String DateFormat = "O";

        public override void WriteJson(
            JsonWriter writer,
            Object? value,
            JsonSerializer serializer)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (value is DateTime dateTime)
            {
                writer.WriteValue(dateTime.ToString(DateFormat, CultureInfo.InvariantCulture));
            }
        }

        public override Object? ReadJson(
            JsonReader reader,
            Type objectType,
            Object? existingValue,
            JsonSerializer serializer) =>
            DateTime.TryParseExact(reader.Value?.ToString(), DateFormat, 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime)
                ? dateTime
                : null;

        public override Boolean CanConvert(
            Type objectType) =>
            objectType == typeof(DateTime) ||
            objectType == typeof(DateTime?);
    }
}
