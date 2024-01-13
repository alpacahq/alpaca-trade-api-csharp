[![Contributors](https://img.shields.io/github/all-contributors/alpacahq/alpaca-trade-api-csharp?logo=github)](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/develop/CONTRIBUTORS.md)
[![Build](https://github.com/alpacahq/alpaca-trade-api-csharp/workflows/Build%20and%20Release/badge.svg?branch=master)](https://github.com/alpacahq/alpaca-trade-api-csharp/actions)
[![Codacy](https://img.shields.io/codacy/grade/7659cd4379964ef190a1088aa879350a?logo=codacy)](https://www.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=github.com)
[![Coverage](https://app.codacy.com/project/badge/Coverage/7659cd4379964ef190a1088aa879350a)](https://www.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=github.com)
[![Nuget](https://img.shields.io/nuget/dt/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/stats/packages/Alpaca.Markets?groupby=Version)
[![PVS-Studio](https://img.shields.io/badge/PVS--Studio-0-blue?logo=opensourceinitiative&logoColor=white&logoWidth=16)](https://pvs-studio.com/pvs-studio/?utm_source=website&utm_medium=github&utm_campaign=open_source)

# .NET SDK for Alpaca Markets API

| Package | Stable | Pre-release |
| ------- | ------ | ----------- |
| [Alpaca.Markets](https://olegra.github.io/Alpaca.Markets/api/Alpaca.Markets.html) | [![Nuget](https://img.shields.io/nuget/v/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets) | [![Nuget](https://img.shields.io/nuget/vpre/Alpaca.Markets?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets/absoluteLatest) |
| [Alpaca.Markets.Extensions](https://olegra.github.io/Alpaca.Markets/api/Alpaca.Markets.Extensions.html) | [![Nuget](https://img.shields.io/nuget/v/Alpaca.Markets.Extensions?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets.Extensions) | [![Nuget](https://img.shields.io/nuget/vpre/Alpaca.Markets.Extensions?logo=NuGet)](https://www.nuget.org/packages/Alpaca.Markets.Extensions/absoluteLatest) |

## .NET Core Usage Example

1.  Create a new console application in a new, empty folder by running `dotnet new console`.
2.  Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3.  Replace the content of the auto-generated `Programm.cs` file with this code snippet:
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
4.  Replace `KEY_ID` and `SECRET_KEY` values with your data from the Alpaca dashboard.
5.  Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and the times that the market will open and close next.

See the [UsageExamples](../../tree/develop/UsageExamples) project for near-to-real-world strategy implementation using this SDK and the [Alpaca.Markets.Tests](https://github.com/OlegRa/Alpaca.Markets.Tests) repository for SDK usage examples. The [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) pages contain a lot of additional information about different aspects of this SDK (environments handling, authentication types, different order placement approaches, streaming client subscriptions handling, etc.).

## Alpaca Data API subscription plans

Alpaca provides 3 different subscription plans for the Data API v2 real-time streaming data: Free, Unlimited, and Business. The first one provides only IEX data and has some subscription limits. Other plans provide full SIP data without data subscription limits. The `IAlpacaDataStreamingClient` interface and its implementation from SDK provide unified access for both streams.

Use the `Environments.Paper.GetAlpacaDataStreamingClient(...)` factory method for creating a client connected to the Free IEX data stream. For the Unlimited and Business SIP data stream use the `Environments.Live.GetAlpacaDataStreamingClient(...)` code. So _Paper_ environment for free data tier and _Live_ for paid subscriptions.

## Mapping between branches and SDK versions

| Branch                                       | Version | Description                                  | Milestone                |
| -------------------------------------------- | ------- | -------------------------------------------- |--------------------------|
| [develop](../../tree/develop)                | 8.x     | Unstable - experimental, can contain bugs    | [SDK 8.x WIP](https://github.com/alpacahq/alpaca-trade-api-csharp/milestone/18) |
| [master](../../tree/develop)                 | 7.x     | LTS - good choice for the new development    | [SDK 7.x LTS](https://github.com/alpacahq/alpaca-trade-api-csharp/milestone/17) |

### Build instructions

1.  Install the latest version of the [.NET 8.0 SDK](https://dotnet.microsoft.com/download) for your OS.
2.  Clone the local version of this repository or your fork (if you want to make changes).
3.  Build the packages using the `dotnet build` command running in the root directory of the cloned repo.

## Contributors

Thanks a lot for all the contributors. See the full list of project supporters in the [CONTRIBUTORS](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/develop/CONTRIBUTORS.md) file.
