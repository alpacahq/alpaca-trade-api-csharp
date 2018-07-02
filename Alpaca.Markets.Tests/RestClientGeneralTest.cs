using System;
using System.Linq;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientGeneralTest
    {
        private readonly RestClient _restClient = RestClientFactory.GetRestClient();

        [Fact]
        public async void GetAccountWorks()
        {
            var account = await _restClient.GetAccountAsync();

            Assert.NotNull(account);
            Assert.Equal("USD", account.Currency);
        }

        [Fact]
        public async void GetOrdersWorks()
        {
            var orders = await _restClient.GetOrdersAsync();

            Assert.NotNull(orders);
            // Assert.NotEmpty(orders);
        }

        [Fact]
        public async void GetOrderWorks()
        {
            var orders = await _restClient.GetOrdersAsync(OrderStatusFilter.All);

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
        public async void GetPositionsWorks()
        {
            var positions = await _restClient.GetPositionsAsync();

            Assert.NotNull(positions);
            Assert.NotEmpty(positions);
        }

        [Fact]
        public async void GetPositionWorks()
        {
            var position = await _restClient.GetPositionAsync("AAPL");

            Assert.NotNull(position);
            Assert.Equal("AAPL", position.Symbol);
        }

        [Fact]
        public async void GetAssetsWorks()
        {
            var assets = await _restClient.GetAssetsAsync();

            Assert.NotNull(assets);
            Assert.NotEmpty(assets);
        }

        [Fact]
        public async void GetAssetWorks()
        {
            var asset = await _restClient.GetAssetAsync("AAPL");

            Assert.NotNull(asset);
            Assert.Equal("AAPL", asset.Symbol);
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
        public async void GetCalendarWorks()
        {
            var calendars = await _restClient.GetCalendarAsync(
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
    }
}
