using System.Globalization;

namespace Alpaca.Markets;

internal sealed class DateOnlyConverter : JsonConverter
{
    private const String DateFormat = "O";

    public override void WriteJson(
        JsonWriter writer,
        Object? value,
        JsonSerializer serializer)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (value is DateOnly dateOnlyValue)
        {
            writer.WriteValue(dateOnlyValue.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }

    public override Object? ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        DateOnly.TryParseExact(reader.Value?.ToString(), DateFormat, out var dateOnlyValue)
            ? dateOnlyValue
            : null;

    [ExcludeFromCodeCoverage]
    public override Boolean CanConvert(Type objectType) =>
        objectType == typeof(DateOnly);
}
