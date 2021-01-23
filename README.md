[![Build](https://github.com/alpacahq/alpaca-trade-api-csharp/workflows/Build%20and%20Release/badge.svg?branch=master)](https://github.com/alpacahq/alpaca-trade-api-csharp/actions)
[![Codacy](https://img.shields.io/codacy/grade/7659cd4379964ef190a1088aa879350a?logo=codacy)](https://www.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=github.com)
[![Nuget](https://img.shields.io/nuget/v/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets)
[![Nuget](https://img.shields.io/nuget/vpre/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets)
[![Nuget](https://img.shields.io/nuget/dt/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/stats/packages/Alpaca.Markets?groupby=Version)

# .NET SDK for Alpaca Markets API

## .NET Core Usage Example

1.  Create a new console application in a new, empty folder by running `dotnet new console`.
2.  Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3.  Replace content of the auto-generated `Programm.cs` file with this code snippet:
```cs
using System;
using Alpaca.Markets;
using System.Threading.Tasks;

namespace AlpacaExample
{
    internal static class Program
    {
        private const String KEY_ID = "";

        private const String SECRET_KEY = "";

        public static async Task Main()
        {
            var client = Environments.Paper
                .GetAlpacaTradingClient(new SecretKey(KEY_ID, SECRET_KEY));

            var clock = await client.GetClockAsync();

            if (clock != null)
            {
                Console.WriteLine(
                    "Timestamp: {0}, NextOpen: {1}, NextClose: {2}",
                    clock.TimestampUtc, clock.NextOpenUtc, clock.NextCloseUtc);
            }
        }
    }
}
```
4.  Replace `KEY_ID` and `SECRET_KEY` values with your own data from the Alpaca dashboard.
5.  Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and the times that the market will open and close next.

See the [UsageExamples](../../tree/develop/UsageExamples) project for near-to-real-world strategy implementation using this SDK. The [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) pages contains a lot of additonal information about different aspects of this SDK (environments handling, authentication types, different order placement approaches, streaming client subscriptions handling, etc.).

## Mapping between branches and SDK versions

| Branch                                       | Version | Description                    |
| -------------------------------------------- | ------- | ------------------------------ |
| [develop](../../tree/develop)                | 5.0.*   | New features, breaking changes |
| [master](../../tree/master)                  | 4.0.*   | All new features and hot-fixes |
| [support/v3.9.x](../../tree/support/v3.9.x)  | 3.9.*   | Hot fixes only, some features  |
