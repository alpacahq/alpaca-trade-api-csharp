using System;

namespace Alpaca.Markets
{
    internal static class StringExtensions
    {
        public static Boolean IsWatchListNameValid(this String name) => 
            !String.IsNullOrEmpty(name) && name.Length <= 64;
    }
}
