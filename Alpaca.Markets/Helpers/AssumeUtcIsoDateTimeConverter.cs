using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class AssumeUtcIsoDateTimeConverter : IsoDateTimeConverter
    {
        public AssumeUtcIsoDateTimeConverter()
        {
            DateTimeStyles = DateTimeStyles.AdjustToUniversal;
            Culture = CultureInfo.InvariantCulture;
        }

        public override Object? ReadJson(
            JsonReader reader,
            Type objectType,
            Object? existingValue,
            JsonSerializer serializer)
        {
            var value = base.ReadJson(reader, objectType, existingValue, serializer);
            return value is DateTime dateTimeValue
                ? DateTime.SpecifyKind(dateTimeValue, DateTimeKind.Utc) 
                : value;
        }
    }
}
