using System;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientPolygonTest
    {
        private readonly RestClient _restClient = RestClientFactory.GetRestClient();

        [Fact(Skip = "Invalid API key")]
        public async void GetExchangesWorks()
        {
            var exchanges = await _restClient.GetExchangesAsync();

            Assert.NotNull(exchanges);
            Assert.NotEmpty(exchanges);
        }

        [Fact(Skip = "Invalid API key")]
        public async void GetSymbolTypeMapWorks()
        {
            var symbolTypeMap = await _restClient.GetSymbolTypeMapAsync();

            Assert.NotNull(symbolTypeMap);
            Assert.NotEmpty(symbolTypeMap);
        }

        [Fact(Skip = "Invalid API key and response format")]
        public async void GetHistoricalTradesWorks()
        {
            var historicalItems = await _restClient
                .GetHistoricalTradesAsync("AAPL", DateTime.Today);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact(Skip = "Invalid API key and response format")]
        public async void GetHistoricalQuotesWorks()
        {
            var historicalItems = await _restClient
                .GetHistoricalQuotesAsync("AAPL", DateTime.Today);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }
    }
}