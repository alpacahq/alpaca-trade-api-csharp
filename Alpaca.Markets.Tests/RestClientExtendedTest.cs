using System;
using System.Threading.Tasks;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientExtendedTest : IDisposable
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
                .ListDayAggregatesAsync(SYMBOL, 1, dateFrom, dateInto);

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
                .ListMinuteAggregatesAsync(SYMBOL, 1, dateFrom, dateInto, true);

            Assert.NotNull(historicalItems);

            Assert.NotNull(historicalItems.Items);
            Assert.NotEmpty(historicalItems.Items);
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
        
        [Fact]
        public async void ListOrdersForDatesWorks()
        {
            var orders = await _restClient.ListOrdersAsync(
                untilDateTimeExclusive: DateTime.Today.AddDays(-5));

            Assert.NotNull(orders);
            // Assert.NotEmpty(orders);
        }

        public void Dispose()
        {
            _restClient?.Dispose();
        }
    }
}
