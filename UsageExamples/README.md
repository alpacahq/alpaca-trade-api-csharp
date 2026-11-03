# Examples

This directory contains examples that use the SDK to connect to Alpaca APIs and data streams. The project is a console application. To run a different example, change the class selected in `Program.cs`.

## Credentials

Create paper-account credentials in the [Alpaca dashboard](https://app.alpaca.markets/) and provide them through environment variables. Never place real credentials in source files or commit them to the repository.

On macOS or Linux:

```bash
export APCA_API_KEY_ID="<paper-key-id>"
export APCA_API_SECRET_KEY="<paper-secret-key>"
```

On Windows PowerShell:

```powershell
$env:APCA_API_KEY_ID="<paper-key-id>"
$env:APCA_API_SECRET_KEY="<paper-secret-key>"
```

## Financial safety

Every example that submits, cancels, or closes orders constructs its trading and trade-update clients from `Environments.Paper`. The brokerage stock example uses `Environments.Live` only for the paid SIP market data stream. Paper and live environments currently share the same crypto market data stream, so the crypto example's data-stream selection does not change its paper order routing.

These examples demonstrate realistic SDK usage, but their performance is not guaranteed and the strategies may not be suitable for any brokerage account. Review the selected example before running it, even with paper credentials.

## Mean Reversion

The mean reversion example is a day trading algorithm that comes in three flavors: paper-only, regular, and power. These distinctions refer to the Alpaca account which will be using the algorithm. A paper-only Alpaca account - one that does not have an attached, funded brokerage account - will not have access to the premium Alpaca data feed that is provided with the creation of a brokerage account. The paper-only version of the algorithm, therefore, relies on the REST API to determine current prices. The regular and power versions of the algorithm improve on this by instead listening to the Alpaca data stream, reducing the latency somewhat versus sending a REST call. The power version of the script goes another step further and shorts when it thinks the price will drop. Not all Alpaca accounts can short securities yet, though. Please see [this page](https://docs.alpaca.markets/margin-and-shorting/) for more information about upgrading to an account with access to shorting and margin.
