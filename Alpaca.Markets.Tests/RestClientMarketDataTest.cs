using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientMarketDataTest
    {
        private readonly RestClient _restClient = RestClientFactory.GetRestClient();

        [Fact]
        public async void GetQuotesWorks()
        {
            var clock = await _restClient.GetClockAsync();

            if (!clock.IsOpen)
            {
                return;
            }

            var quotes = await _restClient.GetQuotesAsync(
                new [] { "AAPL", "GOOG", "MSFT" });

            Assert.NotNull(quotes);
            Assert.NotEmpty(quotes);
        }

        [Fact]
        public async void GetQuoteWorks()
        {
            var clock = await _restClient.GetClockAsync();

            if (!clock.IsOpen)
            {
                return;
            }

            var quote = await _restClient.GetQuoteAsync("AAPL");

            Assert.NotNull(quote);
            Assert.Equal("AAPL", quote.Symbol);
        }

        [Fact]
        public async void GetFundamentalsWorks()
        {
            var fundamentals = await _restClient.GetFundamentalsAsync(
                new[] { "AAPL", "GOOG", "MSFT" });

            Assert.NotNull(fundamentals);
            Assert.NotEmpty(fundamentals);
        }

        [Fact]
        public async void GetFundamentalWorks()
        {
            var fundamental = await _restClient.GetFundamentalAsync("AAPL");

            Assert.NotNull(fundamental);
            Assert.Equal("AAPL", fundamental.Symbol);
        }
    }
}
