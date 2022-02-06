using Xunit;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaCryptoDataClientTest
{
    private const String Crypto = "BTCUSD";

    private const String Other = "ETHUSD";

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    public AlpacaCryptoDataClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;
}