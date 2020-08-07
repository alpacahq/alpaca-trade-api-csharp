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

See the [UsageExamples](../../tree/develop/UsageExamples) project for near-to-real-world strategy implementation using this SDK. The [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) pages contains a lot of additonal information about different aspects of this SDK (environments handling, authentication types, different order placement approaches, streaming client subscriptions handling, etc.).

## Mapping between branches and SDK versions

| Branch                                       | Version | Description                    |
| -------------------------------------------- | ------- | ------------------------------ |
| [develop](../../tree/develop)                | 3.8.*   | New features, breaking changes |
| [master](../../tree/master)                  | 3.7.*   | All new features and hot-fixes |
| [support/v3.6.x](../../tree/support/v3.6.x)  | 3.6.*   | Hot fixes only, some features  |
