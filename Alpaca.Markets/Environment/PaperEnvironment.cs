using System;

namespace Alpaca.Markets
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Globalization", "CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    internal sealed class PaperEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new Uri("https://paper-api.alpaca.markets");

        public Uri AlpacaDataApi => Environments.Live.AlpacaDataApi;

        public Uri PolygonDataApi => throw new InvalidOperationException(
            "Polygon.io REST API does not available on this environment.");

        public Uri AlpacaStreamingApi { get; } = new Uri("wws://paper-api.alpaca.markets/stream");

        public Uri PolygonStreamingApi => throw new InvalidOperationException(
            "Polygon.io streaming API does not available on this environment.");
    }
}
