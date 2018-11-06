using System;
using System.Threading.Tasks;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientExtendedTest
    {
        private const String SYMBOL = "AAPL";

        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async void ListHistoricalQuotesReturnsEmptyListForSunday()
        {
            var historicalItems = await _restClient
                .ListHistoricalQuotesAsync(SYMBOL, new DateTime(2018, 8, 5));

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.Empty(historicalItems.Items);
        }

        [Fact]
        public async void ListDayAggregatesForSpecificDatesWorks()
        {
            var dateInto = DateTime.Today;
            var dateFrom = dateInto.AddDays(-7);

            var historicalItems = await _restClient
                .ListDayAggregatesAsync(SYMBOL, dateFrom, dateInto);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact]
        public async void ListMinuteAggregatesForSpecificDatesWorks()
        {
            var dateInto = DateTime.Today;
            var dateFrom = dateInto.AddDays(-7);

            var historicalItems = await _restClient
                .ListMinuteAggregatesAsync(SYMBOL, dateFrom, dateInto, 100);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
        }

        [Fact]
        public async void ListMinuteAggregatesWithLimitWorks()
        {
            var historicalItems = await _restClient
                .ListMinuteAggregatesAsync(SYMBOL, limit: 100);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);

            Assert.Equal(100, historicalItems.Items.Count);
        }

        [Fact]
        public void AlpacaRestApiThrottlingWorks()
        {
            var tasks = new Task[300];
            for (var i = 0; i < tasks.Length; ++i)
            {
                tasks[i] = _restClient.GetClockAsync();
            }

            Task.WaitAll(tasks);
            Assert.DoesNotContain(tasks, task => task.IsFaulted);
        }
    }
}
