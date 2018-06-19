using System;
using System.Linq;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientMarketDataTest
    {
        private readonly RestClient _restClient = RestClientFactory.GetRestClient();

        [Theory]
        [InlineData(BarDuration.OneMinute, 4)]
        [InlineData(BarDuration.FiveMinutes, 5)]
        [InlineData(BarDuration.FifteenMinutes, 7)]
        [InlineData(BarDuration.OneHour, 7)]
        [InlineData(BarDuration.OneDay, 14)]
        public async void GetMultiAssetBarsWorks(
            BarDuration barDuration,
            Int32 daysLookback)
        {
            var endTimeInclusive = DateTime.UtcNow;
            var startTimeInclusive = endTimeInclusive
                .Subtract(TimeSpan.FromDays(daysLookback));

            var collection = await _restClient.GetBarsAsync(
                new [] { "AAPL", "GOOG", "MSFT" },
                barDuration, startTimeInclusive, endTimeInclusive);

            Assert.NotNull(collection);

            var list = collection.ToList();

            Assert.NotEmpty(list);
            Assert.Equal(3, list.Count);
        }

        [Theory]
        [InlineData(BarDuration.OneMinute, 4)]
        [InlineData(BarDuration.FiveMinutes, 5)]
        [InlineData(BarDuration.FifteenMinutes, 7)]
        [InlineData(BarDuration.OneHour, 7)]
        [InlineData(BarDuration.OneDay, 14)]
        public async void GetSingleAssetBarsWorks(
            BarDuration barDuration,
            Int32 daysLookback)
        {
            var endTimeInclusive = DateTime.UtcNow;
            var startTimeInclusive = endTimeInclusive
                .Subtract(TimeSpan.FromDays(daysLookback));

            var bars = await _restClient.GetBarsAsync(
                "AAPL", barDuration,
                startTimeInclusive, endTimeInclusive);

            Assert.NotNull(bars);
            Assert.Equal("AAPL", bars.Symbol);

            Assert.NotNull(bars.Items);
            Assert.NotEmpty(bars.Items);

            var first = bars.Items.First();
            var last = bars.Items.Last();

            Assert.True(first.Time < last.Time);
        }

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
