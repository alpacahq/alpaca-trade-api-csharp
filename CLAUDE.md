# CLAUDE.md

## General Information and References

This .NET SDK covers Alpaca trading API surface and provides access for all features of this API. The Broker API should be explictitly excluded from any future analysis and processing.
The original underlying Alpaca REST API specification can be found here: https://docs.alpaca.markets/reference and here: https://docs.alpaca.markets/docs/getting-started

There are other SDKs covering the same API for another languages/platforms (in order of significancy for future analysis):
- **Python SDK** - https://github.com/alpacahq/alpaca-py and https://alpaca.markets/sdks/python/
- **Go SDK** - https://github.com/alpacahq/alpaca-trade-api-go/
- **Node.js SDK** - https://github.com/alpacahq/alpaca-trade-api-js/

This SDK has also GitHub WiKi which contains some useful information: https://github.com/alpacahq/alpaca-trade-api-csharp/wiki

## Development Notes and Code Quality

- Use dedicated Git branches for any piece of work related to code/documenation changes.
- Use this pattern for the Git branch for features or code changes: `/feature/{feature-name}`
- Use this pattern for the Git branch for bug or critical errors fixes: `/fix/{problem-description}`
- Treat all warnings as errors and use highest warning level for early problem diagnostics
- Use JetBrains.Annotations for enhanced static analysis using JetBrains ReSharper tools
- Documentation is built from in-code XML comments and published to GitHub Pages
- Use Polly library for HTTP resilience patterns (retry, circuit breaker)
- Use locked package restore mode with `packages.lock.json`
- Write code using SOLID, KISS, and DRY principles

## Code Duplication and Inheritance Guidelines

**Before implementing new classes that extend base classes:**
- ALWAYS analyze the base class inheritance hierarchy first
- Identify what methods are already provided by base classes
- Only implement methods that are genuinely missing or need different behavior
- Never use `new` keyword to hide base class methods unless absolutely necessary

**Data Model Reuse:**
- ALWAYS check for existing data models before creating new ones
- Reuse existing JSON message types if the data structure is identical
- Only create new data models when the structure genuinely differs
- Consider the difference between data source and data structure

**Configuration Best Practices:**
- ALWAYS verify protocol-specific settings (UseMessagePack, API endpoints, etc.)
- Ensure all constructors properly configure required settings
- Follow established patterns from similar client configurations
- Test configuration with actual protocol requirements

**C# Language Features:**
- Prefer composition over inheritance when appropriate
- Avoid unnecessary use of `new` keyword for method hiding
- Use `override` for virtual methods, `new` only for intentional hiding
- Leverage existing base class functionality rather than reimplementing

## Framework Support

- Keep in mind .NET targets list: .NET Standard 2.0, .NET Standard 2.1, .NET Framework 4.6.2, .NET 8.0
- Use common denominator types as much as possible and conditional compilation as a last resort
- Uses latest available C# features provided by the latest .NET SDK compatible with targets

## Common Development Commands

- `dotnet test` - Build debug binaries and test them
- `dotnet build` - Build the entire solution in debug mode
- `dotnet restore` - Restore NuGet packages (using lock files)
- `dotnet tool restore` - Restore .NET tools (dotCover, DocFX)

## Architecture Overview

### NuGet Packages

- **Alpaca.Markets** - https://www.nuget.org/packages/Alpaca.Markets - the main SDK package, provides acces for all covered Alpaca API functions
- **Alpaca.Markets.Extensions** - https://www.nuget.org/packages/Alpaca.Markets.Extensions - the optional package with extended functionality

### Core Projects

- **Alpaca.Markets** - Main SDK with REST and WebSocket clients for trading, market data, and streaming
- **Alpaca.Markets.Extensions** - Extension methods and helper utilities for enhanced functionality
- **Portable.Helpers** - Shared project with common utilities
- **UsageExamples** - Sample code demonstrating SDK usage

### Test Projects

- **Alpaca.Markets.Tests** - Unit tests for the main SDK using xUnit, Moq, and MockHttp
- **Alpaca.Markets.Extensions.Tests** - Unit tests for the extensions package

### Key Client Architecture

The SDK follows a client pattern with multiple specialized clients:

1. **Trading Clients** - Account managemen:
   - `IAlpacaTradingClient` - orders, positions, watchlists
2. **Data Clients** - Historical market data:
   - `IAlpacaDataClient` - Stock market data
   - `IAlpacaCryptoDataClient` - Cryptocurrency data
   - `IAlpacaOptionsDataClient` - Options data
3. **Streaming Clients** - Real-time data streams:
   - `IAlpacaDataStreamingClient` - Stock market data streaming
   - `IAlpacaCryptoStreamingClient` - Crypto data streaming
   - `IAlpacaNewsStreamingClient` - News streaming

### Environment Management

- **Environments** - Factory class providing Paper (sandbox) and Live environment configurations
- **IEnvironment** - Interface for environment-specific client creation (staging, testing, etc.)

### Authentication

- **SecretKey** - API key/secret pair authentication
- **OAuthKey** - OAuth token authentication

## Package Configuration

- Package versioning follows semantic versioning with beta pre-releases
- NuGet packages are auto-generated on build in Release mode
- Packages include symbols (snupkg) for debugging

## CI/CD Configuration

- GitHub Actions workflow handles build, test, and publishing of NuGet packages
- Automatic NuGet publishing on tagged releases (`sdk/*` and `ext/*` tags)
- Documentation auto-deployment to GitHub Pages using a dedicated workflow
