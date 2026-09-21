namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class OrderClassEnumConverter : StringEnumConverter
{
    public override Object ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer) =>
        OrderClass.Simple.FromEnumString(reader);
}
