# Options Streaming Example

This example demonstrates how to use the new `IAlpacaOptionsStreamingClient` to stream real-time options data.

```csharp
using Alpaca.Markets;

// Create options streaming client with free Indicative feed
var client = Environments.Paper.GetAlpacaOptionsStreamingClient(
    new SecretKey("your-api-key", "your-secret-key"),
    OptionsFeed.Indicative);

// Or use premium OPRA feed
var opraClient = Environments.Live.GetAlpacaOptionsStreamingClient(
    new SecretKey("your-api-key", "your-secret-key"),
    OptionsFeed.Opra);

// Connect to the streaming service
await client.ConnectAndAuthenticateAsync();

// Subscribe to option trades for a specific option symbol
var optionSymbol = "AAPL250117C00150000"; // Apple call option
var tradeSubscription = client.GetTradeSubscription(optionSymbol);
tradeSubscription.Received += trade =>
{
    Console.WriteLine($"Option Trade: {trade.Symbol} - Price: {trade.Price}, Size: {trade.Size}");
};

// Subscribe to option quotes for real-time bid/ask
var quoteSubscription = client.GetQuoteSubscription(optionSymbol);
quoteSubscription.Received += quote =>
{
    Console.WriteLine($"Option Quote: {quote.Symbol} - Bid: {quote.BidPrice} x {quote.BidSize}, Ask: {quote.AskPrice} x {quote.AskSize}");
};

// Subscribe to option bars (if available)
var barSubscription = client.GetMinuteBarSubscription(optionSymbol);
barSubscription.Received += bar =>
{
    Console.WriteLine($"Option Bar: {bar.Symbol} - OHLC: {bar.Open}/{bar.High}/{bar.Low}/{bar.Close}, Volume: {bar.Volume}");
};

// Start receiving data
await client.SubscribeAsync(tradeSubscription);
await client.SubscribeAsync(quoteSubscription);
await client.SubscribeAsync(barSubscription);

// Keep the application running
Console.WriteLine("Streaming options data. Press any key to stop...");
Console.ReadKey();

// Clean up
await client.DisconnectAsync();
client.Dispose();
```

## Feed Options

- **Indicative Feed**: Free, 15-minute delayed options data
- **OPRA Feed**: Real-time options data (requires subscription)

## Key Features

- ✅ Real-time options trades
- ✅ Real-time options quotes (bid/ask)
- ✅ Options bars (minute, daily, updated)
- ✅ MessagePack protocol support for efficient data transfer
- ✅ Consistent API with other streaming clients
- ✅ Support for both Paper and Live environments

## Note

This example provides streaming capabilities for options market data. For options trading (placing orders, managing positions), use the existing `IAlpacaTradingClient` which already supports options contracts.