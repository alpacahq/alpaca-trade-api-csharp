using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static class EnumExtensions
    {
        private static readonly Char[] _doubleQuotes = { '"' };

        public static String ToEnumString<T>(
            this T enumValue) =>
            JsonConvert.SerializeObject(enumValue).Trim(_doubleQuotes);
    }
}
