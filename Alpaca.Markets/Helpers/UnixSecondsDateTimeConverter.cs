using System.Globalization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class UnixSecondsDateTimeConverter : DateTimeConverterBase
{
    public override void WriteJson(
        JsonWriter writer,
        Object? value,
        JsonSerializer serializer)
    {
        if (value is DateTime dateTimeValue)
        {
            writer.WriteValue(dateTimeValue.IntoUnixTimeSeconds());
        }
    }

    public override Object? ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        Int64.TryParse(reader.Value?.ToString(),
            NumberStyles.Integer, CultureInfo.InvariantCulture, out var unixTimeStamp)
            ? unixTimeStamp.FromUnixTimeSeconds()
            : null;
}
