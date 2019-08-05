using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class TimeConverter : IsoDateTimeConverter
    {
        public TimeConverter()
        {
            DateTimeStyles = DateTimeStyles.AssumeLocal;
            DateTimeFormat = "HH:mm";
        }
    }
}
