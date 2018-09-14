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
            using (var client = ClientsFactory.GetSockClient())
            {
                client.OnError += (ex) =>
                {
                    Assert.Null(ex.Message);
                };

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
    }
}
