[![Master build status](https://ci.appveyor.com/api/projects/status/github/alpacahq/alpaca-trade-api-csharp?svg=true&branch=master&passingText=master%20-%20OK&failedText=master%20-%20FAIL&pendingText=master%20-%20Pending)](https://ci.appveyor.com/project/alpacahq-bot/alpaca-trade-api-csharp) [![Develop build status](https://ci.appveyor.com/api/projects/status/github/alpacahq/alpaca-trade-api-csharp?svg=true&branch=develop&passingText=develop%20-%20OK&failedText=develop%20-%20FAIL&pendingText=develop%20-%20Pending)](https://ci.appveyor.com/project/alpacahq-bot/alpaca-trade-api-csharp)

# .NET SDK for Alpaca Markets API
## .NET Core Usage Example
1. Create a new console application in a new, empty folder by running `dotnet new console`.
2. Add a reference for Alpaca .NET SDK with `dotnet add package Alpaca.Markets`.
3. Change `Main` method in auto-generated `Programm.cs` file to this code snippet:
```cs
var client = new Alpaca.Markets.RestClient(
    KEY_ID, SECRET_KEY, API_URL);

var clock = client.GetClockAsync().Result;

if (clock != null)
{
    Console.WriteLine(
        "Timestamp: {0}, NextOpen: {1}, NextClose: {2}",
        clock.Timestamp, clock.NextOpen, clock.NextClose);
}
```
4. Replace `KEY_ID`, `SECRET_KEY` and `API_URL` values with your own data from the Alpaca dashboard.
5. Run the sample application using `dotnet run` command and check the output. You should see information about the current market timestamp and the times that the market will open and close next.

## Use .NET Core configuration

Starting from version [1.1.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v1.1.0) you can use the [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) interface for providing configuration data to constructors. You can read more about the new .NET Core configuration approach in [this](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1) article. Please also read the appropriate Wiki pages about expected configuration parameters for each constructor.

## Assembly signing in NuGet package

Starting from version [1.2.3](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v1.1.0) and [2.0.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v2.0.0) NuGet packages contains strongly signed assemblies only for .NET Standard 1.6 and 2.0 targets. .NET Framework 4.5 version of `Alpaca.Markets.dll` is packaged unsigned because a dependent assembly, `NATS.Client.dll`, also shipped unsigned.

## Polygon NATS client deprecation

Polygon is deprecating their NATS streaming in favor of Websockets, and they have finally made it possible for Alpaca users to switch to the websocket endpoint. Starting from version [3.2.0-beta2](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v3.2.0-beta2) we mark `NatsClient` class with `Obsolete` attribute and any usage of this class will generate warning for now. Recommended way for obtaining real-time data now is `PolygonSockClient` usage (it provides very similar interface so transition should be easy). In upcoming minor release (most probably 3.3.0) `NatsClient` will be marked as `Obsolete` with error generation and in upcoming major release (4.0.0) it will be removed from SDK completely.
