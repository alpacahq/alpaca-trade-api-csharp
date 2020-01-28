![](https://github.com/alpacahq/alpaca-trade-api-csharp/workflows/Build%20and%20Release/badge.svg?branch=master)

# .NET SDK for Alpaca Markets API

## .NET Core Usage Example

1. Create a new console application in a new, empty folder by running `dotnet new console`.
2. Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3. Change `Main` method in auto-generated `Programm.cs` file to this code snippet:
```cs
var client = new Alpaca.Markets.Environments.Paper
    .GetAlpacaTradingClient(KEY_ID, new SecretKey(SECRET_KEY));

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
| [develop](../../tree/develop)                | 3.6.*   | New features, breaking changes |
| [master](../../tree/master)                  | 3.5.*   | All new features and hot-fixes |
| [support/v3.4.x](../../tree/support/v3.4.x)  | 3.4.*   | Hot fixes only, some features  |

# Release-specific changes in SDK

## Use .NET Core `IConfiguration`

The [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) interface support marked as obsolete in SDK starting from version [3.5.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.5.0) because it's hard to maintain and can be easy implemented on client side. This support will be completely removed from SDK in upboming major release.

## Assembly signing in NuGet package

Starting from version [3.5.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.5.0) NuGet packages contains strongly signed assemblies only all target frameworks.

## Polygon NATS client deprecation

Polygon is deprecating their NATS streaming in favor of Websockets, and they have finally made it possible for Alpaca users to switch to the websocket endpoint. Starting from version [3.5.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.5.0) support for NATS client streaming completely removed from SDK.
