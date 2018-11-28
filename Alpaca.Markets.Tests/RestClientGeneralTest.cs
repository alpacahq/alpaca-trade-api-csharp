using System;
using System.Linq;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientGeneralTest
    {
        private const String SYMBOL = "AAPL";

        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async void GetAccountWorks()
        {
            var account = await _restClient.GetAccountAsync();

            Assert.NotNull(account);
            Assert.Equal("USD", account.Currency);
        }

        [Fact]
        public async void ListOrdersWorks()
        {
            var orders = await _restClient.ListOrdersAsync();

            Assert.NotNull(orders);
            // Assert.NotEmpty(orders);
        }

        [Fact]
        public async void GetOrderWorks()
        {
            var orders = await _restClient.ListOrdersAsync(OrderStatusFilter.All);

            Assert.NotNull(orders);

            var ordersList = orders.ToList();
            Assert.NotEmpty(ordersList);
            var first = ordersList.First();

            var orderById = await _restClient.GetOrderAsync(first.OrderId);
            var orderByClientId = await _restClient.GetOrderAsync(first.ClientOrderId);

            Assert.NotNull(orderById);
            Assert.NotNull(orderByClientId);

            Assert.Equal(orderById.OrderId, orderByClientId.OrderId);
            Assert.Equal(orderById.ClientOrderId, orderByClientId.ClientOrderId);
        }

        [Fact]
        public async void ListPositionsWorks()
        {
            var positions = await _restClient.ListPositionsAsync();

            Assert.NotNull(positions);
            Assert.NotEmpty(positions);
        }

        [Fact]
        public async void GetPositionWorks()
        {
            var position = await _restClient.GetPositionAsync(SYMBOL);

            Assert.NotNull(position);
            Assert.Equal(SYMBOL, position.Symbol);
        }

        [Fact]
        public async void ListAssetsWorks()
        {
            var assets = await _restClient.ListAssetsAsync();

            Assert.NotNull(assets);
            Assert.NotEmpty(assets);
        }

        [Fact]
        public async void GetAssetWorks()
        {
            var asset = await _restClient.GetAssetAsync(SYMBOL);

            Assert.NotNull(asset);
            Assert.Equal(SYMBOL, asset.Symbol);
        }

        [Fact]
        public async void GetClockWorks()
        {
            var clock = await _restClient.GetClockAsync();

            Assert.NotNull(clock);
            Assert.True(clock.NextOpen > clock.Timestamp);
            Assert.True(clock.NextClose > clock.Timestamp);
        }

        [Fact]
        public async void ListCalendarWorks()
        {
            var calendars = await _restClient.ListCalendarAsync(
                DateTime.Today.AddDays(-14),
                DateTime.Today.AddDays(14));

            Assert.NotNull(calendars);

            var calendarsList = calendars.ToList();

            Assert.NotEmpty(calendarsList);

            var first = calendarsList.First();
            var last = calendarsList.Last();

            Assert.True(first.TradingDate <= last.TradingDate);
            Assert.True(first.TradingOpenTime < first.TradingCloseTime);
            Assert.True(last.TradingOpenTime < last.TradingCloseTime);
        }

        [Fact]
        public async void ListBarsWorks()
        {
            var barsLookup = await _restClient.ListBarsAsync(
                new []{ SYMBOL }, TimeFrame.Day);

            Assert.NotNull(barsLookup);
            Assert.Equal(1, barsLookup.Count);

            Assert.True(barsLookup.Contains(SYMBOL));
            Assert.NotNull(barsLookup[SYMBOL]);
            Assert.NotEmpty(barsLookup[SYMBOL]);
        }
    }
}
