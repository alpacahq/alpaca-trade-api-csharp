using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    internal sealed class DateConverter : IsoDateTimeConverter
    {
        public DateConverter()
        {
            DateTimeStyles = DateTimeStyles.AssumeLocal;
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}