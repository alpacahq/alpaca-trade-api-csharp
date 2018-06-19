using System;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class OrderActionsTest
    {
        private readonly RestClient _restClient = new RestClient(
            "AKEW7ZBQUSNUHOJNQ5MS",
            "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn",
            new Uri("https://staging-api.tradetalk.us"));

        [Fact]
        public async void OrderPlaceCheckCancelWorks()
        {
            var clientOrderId = Guid.NewGuid().ToString("N");

            var clock = await _restClient.GetClockAsync();

            var order = await _restClient.PostOrderAsync(
                "AAPL", 1, OrderSide.Buy, OrderType.Market,
                clock.IsOpen ? TimeInForce.Day : TimeInForce.Opg,
                clientOrderId: clientOrderId);

            Assert.NotNull(order);
            Assert.Equal("AAPL", order.Symbol);
            Assert.Equal(clientOrderId, order.ClientOrderId);

            var orderById = await _restClient.GetOrderAsync(order.OrderId);
            var orderByClientId = await _restClient.GetOrderAsync(clientOrderId);

            Assert.NotNull(orderById);
            Assert.NotNull(orderByClientId);

            var result = await _restClient.DeleteOrderAsync(order.OrderId);

            Assert.True(result);
        }
    }
}
