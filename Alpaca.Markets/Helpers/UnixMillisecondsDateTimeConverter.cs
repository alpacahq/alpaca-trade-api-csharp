using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class UnixMillisecondsDateTimeConverter : UnixDateTimeConverterBase
    {
        protected override Int64 IntoUnixTime(in DateTime value) => value.IntoUnixTimeMilliseconds();

        protected override DateTime FromUnixTime(in Int64 value) => value.FromUnixTimeMilliseconds();
    }
}
