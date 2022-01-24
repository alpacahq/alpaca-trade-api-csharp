using System.Globalization;

namespace Alpaca.Markets;

internal sealed class TimeOnlyConverter : JsonConverter
{
    private static readonly String[] _timeFormats = { "HH:mm", "HHmm" };

    public override void WriteJson(
        JsonWriter writer,
        Object? value,
        JsonSerializer serializer)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (value is TimeOnly timeOnlyValue)
        {
            writer.WriteValue(timeOnlyValue.ToString(_timeFormats[0], CultureInfo.InvariantCulture));
        }
        else if (value is null)
        {
            writer.WriteNull();
        }
    }

    public override Object? ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        TimeOnly.TryParseExact(reader.Value?.ToString(), _timeFormats, out var timeOnlyValue)
            ? timeOnlyValue
            : null;

    public override bool CanConvert(Type objectType) =>
        objectType == typeof(TimeOnly) || objectType == typeof(TimeOnly?);
}
