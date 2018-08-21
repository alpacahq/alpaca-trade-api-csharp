using System;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientExtendedTest
    {
        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async void ListHistoricalQuotesReturnsEmptyListForSunday()
        {
            var historicalItems = await _restClient
                .ListHistoricalQuotesAsync("AAPL", new DateTime(2018, 8, 5));

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.Empty(historicalItems.Items);
        }
    }
}
