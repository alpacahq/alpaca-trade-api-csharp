using System;

namespace Alpaca.Markets
{
    internal static class DecimalExtensions
    {
        public static Int64 AsInteger(this Decimal value) =>
            (Int64) Math.Round(value, MidpointRounding.ToEven);
        
        public static UInt64 AsUInteger(this Decimal value) =>
            (UInt64) Math.Round(value, MidpointRounding.ToEven);
    }
}
