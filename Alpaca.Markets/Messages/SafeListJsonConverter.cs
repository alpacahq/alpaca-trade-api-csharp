using Newtonsoft.Json.Linq;

namespace Alpaca.Markets;

internal class SafeListJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) { return true; }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        return token.Type == JTokenType.Array ? token.ToObject<List<String>>() : [token.ToObject<String>()];
    }
}
