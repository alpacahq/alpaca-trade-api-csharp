using System.Globalization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class AssumeUtcIsoDateTimeConverter : DateTimeConverterBase
{
    public override void WriteJson(
        JsonWriter writer,
        Object? value,
        JsonSerializer serializer)
    {
        if (value is DateTime dateTimeValue)
        {
            writer.WriteValue(dateTimeValue.ToString("O"));
        }
    }

    public override Object? ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        DateTimeOffset.TryParse(reader.Value?.ToString(),
            CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
            out var dateTimeOffset)
            ? dateTimeOffset.UtcDateTime
            : null;
}
