namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class AssetAttributesEnumConverter : StringEnumConverter
{
    public override Object ReadJson(
        JsonReader reader,
        Type objectType,
        Object? existingValue,
        JsonSerializer serializer)
    {
        try
        {
            return AssetAttributes.Unknown.FromEnumString(reader);
        }
        catch (JsonSerializationException)
        {
            return AssetAttributes.Unknown;
        }
    }
}
