using System;
using System.Threading;
using Xunit;

namespace Alpaca.Markets.Tests
{
    public sealed class SockClientTest
    {
        [Fact]
        public async void ConnectWorks()
        {
            using (var client = getClient())
            {
                await client.ConnectAsync();

                var waitObject = new AutoResetEvent(false);
                client.Connected += (status) =>
                {
                    Assert.Equal(AuthStatus.Authorized, status);
                    waitObject.Set();
                };

                Assert.True(waitObject.WaitOne(
                    TimeSpan.FromSeconds(10)));

                await client.DisconnectAsync();
            }
        }

        private SockClient getClient()
        {
            return new SockClient(
                "AKEW7ZBQUSNUHOJNQ5MS",
                "Yr2Tms89rQ6foRLNu4pz3w/yXOrxQGDmXctU1BCn",
                new Uri("https://staging-api.tradetalk.us"));
        }
    }
}
