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
        public async void GetHistoricTradesWorks()
        {
            var historicTrades = await _restClient
                .GetHistoricTradesAsync("AAPL", DateTime.Today);

            Assert.NotNull(historicTrades);

            Assert.NotNull(historicTrades.Trades);
            Assert.NotEmpty(historicTrades.Trades);
        }
    }
}