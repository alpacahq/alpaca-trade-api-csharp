using System;
using System.Threading;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class NatsClientTest
    {
        [Fact]
        public void NatsTradesSubscriptionWorks()
        {
            using (var client = GetClient())
            {
                client.Open();

                var waitObject = new AutoResetEvent(false);
                client.TradeReceived += (trade) =>
                {
                    Assert.Equal("AAPL", trade.Symbol);
                    waitObject.Set();
                };

                client.SubscribeTrade("AAPL");

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(10)));

                client.Close();
            }
        }

        [Fact]
        public void NatsQuotesSubscriptionWorks()
        {
            using (var client = GetClient())
            {
                client.Open();

                var waitObject = new AutoResetEvent(false);
                client.QuoteReceived += (quote) =>
                {
                    Assert.Equal("AAPL", quote.Symbol);
                    waitObject.Set();
                };

                client.SubscribeQuote("AAPL");

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(10)));

                client.Close();
            }
        }

        [Fact]
        public void NatsSecondBarSubscriptionWorks()
        {
            using (var client = GetClient())
            {
                client.Open();

                var waitObject = new AutoResetEvent(false);
                client.BarReceived += (bar) =>
                {
                    Assert.Equal("AAPL", bar.Symbol);
                    waitObject.Set();
                };

                client.SubscribeSecondBar("AAPL");

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(10)));

                client.Close();
            }
        }

        [Fact(Skip="Too long running")]
        public void NatsMinuteBarSubscriptionWorks()
        {
            using (var client = GetClient())
            {
                client.Open();

                var waitObject = new AutoResetEvent(false);
                client.BarReceived += (bar) =>
                {
                    Assert.Equal("AAPL", bar.Symbol);
                    waitObject.Set();
                };

                client.SubscribeMinuteBar("AAPL");

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(120)));

                client.Close();
            }
        }

        [Fact]
        public void NatsSeveralSubscriptionWorks()
        {
            using (var client = GetClient())
            {
                client.Open();

                var waitObjects = new []
                {
                    new AutoResetEvent(false),
                    new AutoResetEvent(false)
                };

                client.TradeReceived += (trade) =>
                {
                    Assert.Equal("AAPL", trade.Symbol);
                    waitObjects[0].Set();
                };

                client.QuoteReceived += (quote) =>
                {
                    Assert.Equal("AAPL", quote.Symbol);
                    waitObjects[1].Set();
                };

                client.SubscribeTrade("AAPL");
                client.SubscribeQuote("AAPL");

                Assert.True(WaitHandle.WaitAll(
                    waitObjects, TimeSpan.FromSeconds(10)));

                client.Close();
            }
        }

        private NatsClient GetClient() => new NatsClient("AKEW7ZBQUSNUHOJNQ5MS-staging");
    }
}
