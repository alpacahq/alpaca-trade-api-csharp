using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca REST API and Polygon REST API endpoints.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed partial class RestClient : IDisposable
    {
        private readonly AlpacaTradingClient _alpacaTradingClient;

        private readonly PolygonDataClient _polygonDataClient;

        private readonly AlpacaDataClient _alpacaDataClient;

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public RestClient(
            RestClientConfiguration configuration)
        {
            configuration.EnsureNotNull(nameof(configuration));

            _alpacaTradingClient = new AlpacaTradingClient(configuration.AlpacaTradingClientConfiguration);
            _polygonDataClient = new PolygonDataClient(configuration.PolygonDataClientConfiguration);
            _alpacaDataClient = new AlpacaDataClient(configuration.AlpacaDataClientConfiguration);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _alpacaTradingClient?.Dispose();
            _alpacaDataClient?.Dispose();
            _polygonDataClient?.Dispose();
        }
    }
}
