using System;

namespace Alpaca.Markets
{
    internal static class NullableHelper
    {
        public static T EnsureNotNull<T>(this T value, String name) where T : class => value ?? throw new ArgumentNullException(name);
    }
}
