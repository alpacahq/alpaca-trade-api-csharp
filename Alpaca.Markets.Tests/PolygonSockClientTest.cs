using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class PolygonSockClientTest : IDisposable
    {
        private const String SYMBOL = "AAPL";

        private readonly RestClient _restClient = ClientsFactory.GetRestClient();

        [Fact]
        public async Task TradesSubscriptionWorks()
        {
            using (var client = ClientsFactory.GetPolygonSockClient())
            {
                await connectAndAuthenticate(client);

                var waitObject = new AutoResetEvent(false);

                client.TradeReceived += (trade) =>
                {
                    Assert.Equal(SYMBOL, trade.Symbol);
                    waitObject.Set();
                };

                waitObject.Reset();
                client.SubscribeTrade(SYMBOL);

                if (_restClient.GetClockAsync().Result.IsOpen)
                {
                    Assert.True(waitObject.WaitOne(
                        TimeSpan.FromSeconds(10)));
                }

                await client.DisconnectAsync();
            }
        }

        [Fact]
        public async Task QuotesSubscriptionWorks()
        {
            using (var client = ClientsFactory.GetPolygonSockClient())
            {
                await connectAndAuthenticate(client);

                var waitObject = new AutoResetEvent(false);
                client.QuoteReceived += (quote) =>
                {
                    Assert.Equal(SYMBOL, quote.Symbol);
                    waitObject.Set();
                };

                client.SubscribeQuote(SYMBOL);

                if (_restClient.GetClockAsync().Result.IsOpen)
                {
                    Assert.True(waitObject.WaitOne(
                        TimeSpan.FromSeconds(10)));
                }

                await client.DisconnectAsync();
            }
        }

        [Fact]
        public async Task SecondAggSubscriptionWorks()
        {
            using (var client = ClientsFactory.GetPolygonSockClient())
            {
                await connectAndAuthenticate(client);

                var waitObject = new AutoResetEvent(false);
                client.SecondAggReceived += (agg) =>
                {
                    Assert.Equal(SYMBOL, agg.Symbol);
                    waitObject.Set();
                };

                client.SubscribeSecondAgg(SYMBOL);

                if (_restClient.GetClockAsync().Result.IsOpen)
                {
                    Assert.True(waitObject.WaitOne(
                        TimeSpan.FromSeconds(10)));
                }

                await client.DisconnectAsync();
            }
        }

        [Fact(Skip="Too long running")]
        public async Task MinuteAggSubscriptionWorks()
        {
            using (var client = ClientsFactory.GetPolygonSockClient())
            {
                await connectAndAuthenticate(client);

                var waitObject = new AutoResetEvent(false);
                client.MinuteAggReceived += (agg) =>
                {
                    Assert.Equal(SYMBOL, agg.Symbol);
                    waitObject.Set();
                };

                client.SubscribeMinuteAgg(SYMBOL);

                if (_restClient.GetClockAsync().Result.IsOpen)
                {
                    Assert.True(waitObject.WaitOne(
                        TimeSpan.FromSeconds(120)));
                }

                await client.DisconnectAsync();
            }
        }

        [Fact]
        public async Task SeveralSubscriptionWorks()
        {
            using (var client = ClientsFactory.GetPolygonSockClient())
            {
                await connectAndAuthenticate(client);

                var waitObjects = new []
                {
                    new AutoResetEvent(false),
                    new AutoResetEvent(false)
                };

                client.TradeReceived += (trade) =>
                {
                    Assert.Equal(SYMBOL, trade.Symbol);
                    waitObjects[0].Set();
                };

                client.QuoteReceived += (quote) =>
                {
                    Assert.Equal(SYMBOL, quote.Symbol);
                    waitObjects[1].Set();
                };

                client.SubscribeTrade(SYMBOL);
                client.SubscribeQuote(SYMBOL);

                if (_restClient.GetClockAsync().Result.IsOpen)
                {
                    // ReSharper disable once CoVariantArrayConversion
                    Assert.True(WaitHandle.WaitAll(
                        waitObjects, TimeSpan.FromSeconds(10)));
                }

                await client.DisconnectAsync();
            }
        }

        public void Dispose()
        {
            _restClient?.Dispose();
        }

        private static async Task connectAndAuthenticate(
            PolygonSockClient client)
        {
            var waitObject = new AutoResetEvent(false);

            client.Connected += (status) =>
            {
                Assert.Equal(AuthStatus.Authorized, status);
                waitObject.Set();
            };

            await client.ConnectAsync();
            Assert.True(waitObject.WaitOne(
                TimeSpan.FromSeconds(10)));
        }
    }
}
