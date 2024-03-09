using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class ServiceCollectionTest
{
    [Fact]
    public void AddAlpacaCryptoDataClientWorks()
    {
        var collection = new ServiceCollection();

        Assert.NotNull(collection.AddAlpacaCryptoDataClient(
            Environments.Live, new SecretKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())));

        using var client = collection.BuildServiceProvider().GetService<IAlpacaCryptoDataClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAlpacaDataClientWorks()
    {
        var collection = new ServiceCollection();

        Assert.NotNull(collection.AddAlpacaDataClient(
            Environments.Live, new SecretKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())));

        using var client = collection.BuildServiceProvider().GetService<IAlpacaDataClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAlpacaOptionsDataClientWorks()
    {
        var collection = new ServiceCollection();

        Assert.NotNull(collection.AddAlpacaOptionsDataClient(
            Environments.Live, new SecretKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())));

        using var client = collection.BuildServiceProvider().GetService<IAlpacaOptionsDataClient>();
        Assert.NotNull(client);
    }

    [Fact]
    public void AddAlpacaTradingClientWorks()
    {
        var collection = new ServiceCollection();

        Assert.NotNull(collection.AddAlpacaTradingClient(
            Environments.Live, new SecretKey(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())));

        using var client = collection.BuildServiceProvider().GetService<IAlpacaTradingClient>();
        Assert.NotNull(client);
    }
}
