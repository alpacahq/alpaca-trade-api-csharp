# Crypto Perpetual Futures Support

## Current Status: **Preparatory Only**

The Alpaca.Markets .NET SDK includes a `CryptoPerpetual` asset class enum value in preparation for potential future support of crypto perpetual futures. However, **Alpaca Markets does not currently offer crypto perpetual futures trading**.

## What's Available

### ✅ **Spot Crypto Trading (Fully Supported)**
```csharp
// Current working crypto symbols (spot trading only)
var symbols = new[] { "BTC/USD", "ETH/USD", "AVAX/USD" };

// Historical data
var barsRequest = new HistoricalCryptoBarsRequest(symbols, yesterday, today);
var bars = await cryptoDataClient.ListHistoricalBarsAsync(barsRequest);

// Real-time streaming
var subscription = cryptoStreamingClient.GetTradeSubscription("BTC/USD");
```

### ❌ **Crypto Perpetual Futures (Not Currently Available)**
```csharp
// The following enum exists but is not functional:
AssetClass.CryptoPerpetual  // JSON: "crypto_perp"

// Hypothetical perpetual symbols (these would NOT work):
// "BTCUSD-PERP", "BTC-PERP", etc.
```

## Technical Implementation Status

| Component | Spot Crypto | Perpetual Futures |
|-----------|-------------|-------------------|
| **IAlpacaCryptoDataClient** | ✅ Full support | ❌ Not available |
| **IAlpacaCryptoStreamingClient** | ✅ Full support | ❌ Not available |
| **AssetClass enum** | ✅ `Crypto` | 🟡 `CryptoPerpetual` (preparatory) |
| **API endpoints** | ✅ `/v1beta3/crypto/us/` | ❌ No active endpoints |

## Current Crypto Capabilities

The existing crypto clients provide complete functionality for **spot crypto trading**:

### Data API Features:
- Historical bars, trades, and quotes
- Latest market data (bars, trades, quotes)
- Market snapshots
- Order book data
- Exchange filtering

### Streaming Features:
- Real-time trade updates
- Real-time quote updates
- Order book streaming
- Market status updates

## Future Considerations

The `CryptoPerpetual` asset class exists as **preparatory infrastructure** for potential future support. If/when Alpaca Markets adds crypto perpetual futures:

1. **No breaking changes** will be required in client code
2. **Additional methods** may be added to crypto clients
3. **New request/response models** may be introduced
4. **Symbol formats** will be determined by Alpaca's implementation

## Alternative Solutions

For crypto perpetual futures trading, consider platforms that currently offer derivatives trading:
- Other crypto exchanges with perpetual futures
- Multi-asset brokers with crypto derivatives
- Dedicated crypto derivatives platforms

## Questions?

If you need crypto perpetual futures functionality, please:
1. **Contact Alpaca Markets** directly about their roadmap
2. **Monitor Alpaca's announcements** for derivatives support
3. **Watch SDK releases** for perpetual futures implementation

---

*This documentation reflects the current state as of SDK 8.0.0. Future versions may include active perpetual futures support when Alpaca Markets launches this functionality.*