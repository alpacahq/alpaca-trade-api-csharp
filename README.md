# .NET SDK for Alpaca Markets API
## .NET Core Usage Example
1. Create new console application using command `dotnet new console` in new empty folder.
2. Add reference for Alpaca .NET SDK using `dotnet add package Alpaca.Markets --version 1.0.0-alpha` command.
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
