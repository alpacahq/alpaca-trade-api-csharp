[![Contributors](https://img.shields.io/github/all-contributors/alpacahq/alpaca-trade-api-csharp?logo=github)](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CONTRIBUTORS.md)
[![Codacy](https://img.shields.io/codacy/grade/7659cd4379964ef190a1088aa879350a?logo=codacy)](https://app.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Coverage](https://app.codacy.com/project/badge/Coverage/7659cd4379964ef190a1088aa879350a)](https://app.codacy.com/gh/OlegRa/Alpaca.Markets/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_coverage)

# .NET SDK for Alpaca Markets API

This package contains C#/.NET SDK for [Alpaca Trade API](https://docs.alpaca.markets/). See complete online documentation [here](https://alpacahq.github.io/alpaca-trade-api-csharp/api/Alpaca.Markets.html).

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
        public static async Task Main()
        {
            var keyId = Environment.GetEnvironmentVariable("APCA_API_KEY_ID")
                ?? throw new InvalidOperationException("APCA_API_KEY_ID is required.");
            var secretKey = Environment.GetEnvironmentVariable("APCA_API_SECRET_KEY")
                ?? throw new InvalidOperationException("APCA_API_SECRET_KEY is required.");

            var client = Environments.Paper
                .GetAlpacaTradingClient(new SecretKey(keyId, secretKey));

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
4.  Set `APCA_API_KEY_ID` and `APCA_API_SECRET_KEY` to your paper-account credentials from the Alpaca dashboard. Never commit credentials to source control.
5.  Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and when the market will open and close next.

See the [UsageExamples](https://github.com/alpacahq/alpaca-trade-api-csharp/tree/main/UsageExamples) project for near-to-real-world strategy implementations and the [Alpaca.Markets.Tests](https://github.com/alpacahq/alpaca-trade-api-csharp/tree/main/Alpaca.Markets.Tests) project for SDK usage examples. The [Wiki](https://github.com/alpacahq/alpaca-trade-api-csharp/wiki) contains additional information about environment handling, authentication types, order placement approaches, streaming subscriptions, and other SDK features.

## Support and contributing

Use [GitHub Issues](https://github.com/alpacahq/alpaca-trade-api-csharp/issues/new/choose) for SDK-specific bugs, feature requests, and questions. Do not report security vulnerabilities publicly; follow the [Security Policy](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/SECURITY.md).

Contributions are welcome. See the [contribution guide](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CONTRIBUTING.md) for setup, scope, API compatibility requirements, and the pull-request workflow. Participation is governed by our [Code of Conduct](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CODE_OF_CONDUCT.md).

## Contributors

Thanks a lot for all the contributors. See the complete list of project supporters in the [CONTRIBUTORS](https://github.com/alpacahq/alpaca-trade-api-csharp/blob/main/CONTRIBUTORS.md) file.
