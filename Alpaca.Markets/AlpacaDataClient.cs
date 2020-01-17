﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
    /// </summary>
    public sealed class AlpacaDataClient : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaDataClient(
            AlpacaDataClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint
                .AddApiVersionNumberSafe(ApiVersion.V1);
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">>Asset names for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="areTimesInclusive">
        /// If <c>true</c> - both <paramref name="timeFrom"/> and <paramref name="timeInto"/> parameters are treated as inclusive.
        /// </param>
        /// <param name="timeFrom">Start time for filtering.</param>
        /// <param name="timeInto">End time for filtering.</param>
        /// <param name="limit">Maximal number of daily bars in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public async Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            IEnumerable<String> symbols,
            TimeFrame timeFrame,
            Int32? limit = 100,
            Boolean areTimesInclusive = true,
            DateTime? timeFrom = null,
            DateTime? timeInto = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"bars/{timeFrame.ToEnumString()}",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", symbols))
                    .AddParameter((areTimesInclusive ? "start" : "after"), timeFrom, "O")
                    .AddParameter((areTimesInclusive ? "end" : "until"), timeInto, "O")
                    .AddParameter("limit", limit)
            };

            var response = await _httpClient
                .GetSingleObjectAsync<IReadOnlyDictionary<String, List<JsonBarAgg>>, Dictionary<String, List<JsonBarAgg>>>(
                    FakeThrottler.Instance, builder, cancellationToken)
                .ConfigureAwait(false);

            return response.ToDictionary(
                kvp => kvp.Key, 
                kvp => (IReadOnlyList<IAgg>)kvp.Value.AsReadOnly());
        }
    }
}
