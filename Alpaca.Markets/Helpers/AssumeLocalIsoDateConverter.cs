using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class AssumeLocalIsoDateConverter : IsoDateTimeConverter
{
    private const String DateFormat = "yyyy-MM-dd";

    public AssumeLocalIsoDateConverter()
    {
        DateTimeStyles = DateTimeStyles.AssumeLocal;
        DateTimeFormat = DateFormat;
    }
}
