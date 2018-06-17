using System;
using Newtonsoft.Json;

namespace Alpaca.Markets.Helpers
{
    internal static class EnumExtensions
    {
        private static readonly Char[] _doubleQuotes = { '"' };

        public static String ToEnumString<T>(
            this T enumValue)
        {
            return JsonConvert.SerializeObject(enumValue).Trim(_doubleQuotes);
        }
    }
}
