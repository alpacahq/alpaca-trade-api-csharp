namespace Alpaca.Markets.Extensions.Tests;

public sealed class WithReconnectTest
{
    [Fact]
    public void AlpacaStreamingClientWithReconnectWorks()
    {
        var count = 0;
        var client = new Mock<IAlpacaStreamingClient>();

        client.SetupAdd(_ => _.OnTradeUpdate += null).Callback(() => ++count);
        client.SetupRemove(_ => _.OnTradeUpdate -= null).Callback(() => ++count);

        using (var wrapped = client.Object.WithReconnect())
        {
            wrapped.OnTradeUpdate += HandleTradeUpdate;
            wrapped.OnTradeUpdate -= HandleTradeUpdate;
        }

        Assert.Equal(0, count);
        client.Verify();

        void HandleTradeUpdate(ITradeUpdate _) => ++count;
    }
}
