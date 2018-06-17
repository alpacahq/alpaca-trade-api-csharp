using System;
using System.Linq;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class RestClientTest
    {
        private readonly RestClient _restClient = new RestClient(
            "AKEW7ZBQUSNUHOJNQ5MS",
            "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn",
            new Uri("https://staging-api.tradetalk.us"));

        [Fact]
        public async void GetAccountsWorks()
        {
            var account = await _restClient.GetAccountAsync();

            Assert.NotNull(account);
            Assert.Equal("USD", account.Currency);
        }

        [Fact]
        public async void GetOrderWorks()
        {
            var order = await _restClient.GetOrderAsync(Guid.NewGuid());

            Assert.NotNull(order);
            // Assert.NotEmpty(orders);
        }

        [Fact]
        public async void GetPositionsWorks()
        {
            var positions = await _restClient.GetPositionsAsync();

            Assert.NotNull(positions);
            // Assert.NotEmpty(positions);
        }

        [Fact(Skip = "Do not have position now")]
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
            Assert.True(clock.NextClose > clock.NextOpen);
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
