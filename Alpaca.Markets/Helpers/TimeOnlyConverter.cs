using System.Globalization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class TimeOnlyConverter : JsonConverter
{
    private static readonly String[] _timeFormats = [ "HH:mm", "HHmm" ];

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
    }

    public override Object? ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        TimeOnly.TryParseExact(reader.Value?.ToString(), _timeFormats, out var timeOnlyValue)
            ? timeOnlyValue
            : null;

    [ExcludeFromCodeCoverage]
    public override Boolean CanConvert(Type objectType) =>
        objectType == typeof(TimeOnly);
}
