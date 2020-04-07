![](https://github.com/alpacahq/alpaca-trade-api-csharp/workflows/Build%20and%20Release/badge.svg?branch=master)
![Nuget](https://img.shields.io/nuget/v/Alpaca.Markets?logo=NuGet)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Alpaca.Markets?logo=NuGet)
![Nuget](https://img.shields.io/nuget/dt/Alpaca.Markets?logo=NuGet)

# .NET SDK for Alpaca Markets API

## .NET Core Usage Example

1. Create a new console application in a new, empty folder by running `dotnet new console`.
2. Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3. Change `Main` method in auto-generated `Programm.cs` file to this code snippet:
```cs
var client = Alpaca.Markets.Environments.Paper
    .GetAlpacaTradingClient(new SecretKey(KEY_ID, SECRET_KEY));

var clock = client.GetClockAsync().Result;

if (clock != null)
{
    Console.WriteLine(
        "Timestamp: {0}, NextOpen: {1}, NextClose: {2}",
        clock.Timestamp, clock.NextOpen, clock.NextClose);
}
```
4. Replace `KEY_ID` and `SECRET_KEY` values with your own data from the Alpaca dashboard.
5. Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and the times that the market will open and close next.

## Mapping between branches and SDK versions

| Branch                                       | Version | Description                    |
| -------------------------------------------- | ------- | ------------------------------ |
| [develop](../../tree/develop)                | 3.7.*   | New features, breaking changes |
| [master](../../tree/master)                  | 3.6.*   | All new features and hot-fixes |
| [support/v3.5.x](../../tree/support/v3.5.x)  | 3.5.*   | Hot fixes only, some features  |

# Release-specific changes in SDK

## The `IConfiguration` support deprecation

The [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) interface support completely removed from the SDK starting from version [3.6.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.6.0) because it's hard to maintain and can be easy implemented on client side.

## Polygon NATS client deprecation

Polygon is deprecating their NATS streaming in favor of Websockets, and they have finally made it possible for Alpaca users to switch to the websocket endpoint. Starting from version [3.5.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.5.0) support for NATS client streaming completely removed from SDK.
