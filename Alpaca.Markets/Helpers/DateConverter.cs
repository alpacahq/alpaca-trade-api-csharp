using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
            : this("yyyy-MM-dd")
        {
        }

        public DateConverter(String format)
        {
            DateTimeStyles = DateTimeStyles.AssumeLocal;
            DateTimeFormat = format;
        }
    }
}
