# .NET SDK for Alpaca Markets API
## .NET Core Usage Example
1. Create new console application using command `dotnet new console` in new empty folder.
2. Add reference for Alpaca .NET SDK using `dotnet add package Alpaca.Markets` command.
3. Change `Main` method in auto-generated `Programm.cs` file using next code snippet:
```cs
var client = new Alpaca.Markets.RestClient(
    KEY_ID, SECRET_KEY, new Uri(API_URL));

var clock = client.GetClockAsync().Result;

if (clock != null)
{
    Console.WriteLine(
        "Timestamp: {0}, NextOpen: {1}, NextClose: {2}",
        clock.Timestamp, clock.NextOpen, clock.NextClose);
}
```
4. Replace `KEY_ID`, `SECRET_KEY` and `API_URL` values with data from web console.
5. Run sample application using `dotnet run` command and check output

## Use .NET Core configuration

Starting form version [1.1.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v1.1.0) you can use [IConfiguration](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration) interface for providing configuration data into constructors. You can read more about new .NET Core configuration approach in [this](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.1) article. Read appropriate Wiki pages about expected configuration parameters for each constructor.

## Assembly signing in NuGet package

Starting from version [1.2.3](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v1.1.0) and [2.0.0](https://github.com/alpacahq/alpaca-trade-api-csharp/releases/tag/v2.0.0) NuGet packages contains strongly signed assemblies only for .NET Standard 1.6 and 2.0 targets, .NET Framework 4.5 version of `Alpaca.Markets.dll` packaged unsigned because dependent assembly `NATS.Client.dll` also shipped unsigned.
