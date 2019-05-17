using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientPolygonTest : IDisposable
    {
        private const String SYMBOL = "AAPL";

        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async void ListExchangesWorks()
        {
            var exchanges = await _restClient.ListExchangesAsync();

            Assert.NotNull(exchanges);
            Assert.NotEmpty(exchanges);
        }

        [Fact]
        public async void GetSymbolTypeMapWorks()
        {
            var symbolTypeMap = await _restClient.GetSymbolTypeMapAsync();

            Assert.NotNull(symbolTypeMap);
            Assert.NotEmpty(symbolTypeMap);
        }

        [Fact]
        public async void ListHistoricalTradesWorks()
        {
            var historicalItems = await _restClient
                .ListHistoricalTradesAsync(
                    SYMBOL, await getLastTradingDay());

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact]
        public async void ListHistoricalQuotesWorks()
        {
            var historicalItems = await _restClient
                .ListHistoricalQuotesAsync(
                    SYMBOL, await getLastTradingDay());

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact]
        public async void ListMinuteAggregatesForDateRangeWorks()
        {
            var dateInto = await getLastTradingDay();
            var dateFrom = dateInto.AddHours(-20);

            var historicalItems = await _restClient
                .ListMinuteAggregatesAsync(SYMBOL, 1, dateFrom, dateInto);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact]
        public async void GetLastTradeWorks()
        {
            var lastTrade = await _restClient
                .GetLastTradeAsync(SYMBOL);

            Assert.NotNull(lastTrade);
            Assert.True(lastTrade.Time.Kind == DateTimeKind.Utc);
        }

        [Fact]
        public async void GetLastQuoteWorks()
        {
            var lastQuote = await _restClient
                .GetLastQuoteAsync(SYMBOL);

            Assert.NotNull(lastQuote);
            Assert.True(lastQuote.Time.Kind == DateTimeKind.Utc);
        }

        [Theory]
        [InlineData(TickType.Trades)]
        [InlineData(TickType.Quotes)]
        public async void GetConditionMapForQuotesWorks(
            TickType tickType)
        {
            var conditionMap = await _restClient
                .GetConditionMapAsync(tickType);

            Assert.NotNull(conditionMap);
            Assert.NotEmpty(conditionMap);
        }

        private async Task<DateTime> getLastTradingDay()
        {
            var calendars = await _restClient
                .ListCalendarAsync(
                    DateTime.UtcNow.Date.AddDays(-14),
                    DateTime.UtcNow.Date.AddDays(-1));

            Assert.NotNull(calendars);

            return calendars.Last().TradingCloseTime;
        }

        public void Dispose()
        {
            _restClient?.Dispose();
        }
    }
}