using System;
using System.Threading;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class OrderActionsTest : IDisposable
    {
        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async void OrderPlaceCheckCancelWorks()
        {
            using (var sockClient = ClientsFactory.GetSockClient())
            {
                sockClient.OnError += (ex) =>
                {
                    Assert.Null(ex.Message);
                };

                await sockClient.ConnectAsync();

                var waitObject = new AutoResetEvent(false);
                sockClient.OnTradeUpdate += (update) =>
                {
                    Assert.NotNull(update);
                    Assert.NotNull(update.Order);
                    Assert.Equal("AAPL", update.Order.Symbol);
                    waitObject.Set();
                };

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

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(10)));

                await sockClient.DisconnectAsync();
            }
        }

        public void Dispose()
        {
            _restClient?.Dispose();
        }
    }
}
