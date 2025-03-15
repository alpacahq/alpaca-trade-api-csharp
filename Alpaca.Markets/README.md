[![Contributors](https://img.shields.io/github/all-contributors/alpacahq/alpaca-trade-api-csharp?logo=github)](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CONTRIBUTORS.md)
[![Codacy](https://img.shields.io/codacy/grade/7659cd4379964ef190a1088aa879350a?logo=codacy)](https://app.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Coverage](https://app.codacy.com/project/badge/Coverage/7659cd4379964ef190a1088aa879350a)](https://app.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)
[![Sponsors](https://img.shields.io/github/sponsors/OlegRa?logo=github)](https://github.com/sponsors/OlegRa)

# .NET SDK for Alpaca Markets API

This package contains C#/.NET SDK for [Alpaca Trade API](https://docs.alpaca.markets/). See complete online documentation [here](https://olegra.github.io/Alpaca.Markets/api/Alpaca.Markets.html).

## .NET Core Usage Example

1.  Create a new console application in a new, empty folder by running `dotnet new console`.
2.  Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3.  Replace content of the auto-generated `Program.cs` file with this code snippet:
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
4.  Replace the `KEY_ID` and `SECRET_KEY` values with your data from the Alpaca dashboard.
5.  Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and when the market will open and close next.

See the [UsageExamples](https://github.com/alpacahq/alpaca-trade-api-csharp/tree/main/UsageExamples) project for near-to-real-world strategy implementation using this SDK and the
[Alpaca.Markets.Tests](https://github.com/OlegRa/Alpaca.Markets.Tests) repository for SDK usage examples. The [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) pages contain
a lot of additional information about different aspects of this SDK (environments handling, authentication types, different order placement approaches, streaming client subscriptions handling, etc.).

## Contributors

Thanks a lot for all the contributors. See the complete list of project supporters in the [CONTRIBUTORS](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CONTRIBUTORS.md) file.
