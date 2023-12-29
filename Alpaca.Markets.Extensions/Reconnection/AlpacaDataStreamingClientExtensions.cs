namespace Alpaca.Markets.Extensions;

/// <summary>
/// Helper extension method for creating special version of the <see cref="IAlpacaDataStreamingClient"/>
/// implementation with automatic reconnection (with configurable delay and number of attempts) support.
/// </summary>
public static partial class AlpacaDataStreamingClientExtensions
{
    [method: ExcludeFromCodeCoverage]
    private sealed class ClientWithReconnection(
        IAlpacaDataStreamingClient client,
        ReconnectionParameters reconnectionParameters) :
        ClientWithSubscriptionReconnectBase<IAlpacaDataStreamingClient>(
            client, reconnectionParameters),
        IAlpacaDataStreamingClient
    {
        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ITrade> GetTradeSubscription() =>
            Client.GetTradeSubscription();

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ITrade> GetTradeSubscription(String symbol) =>
            Client.GetTradeSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription() =>
            Client.GetQuoteSubscription();

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(String symbol) =>
            Client.GetQuoteSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(String symbol) =>
            Client.GetMinuteBarSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
            Client.GetMinuteBarSubscription();

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(String symbol) =>
            Client.GetDailyBarSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IStatus> GetStatusSubscription(String symbol) =>
            Client.GetStatusSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ITrade> GetCancellationSubscription(String symbol) =>
            Client.GetCancellationSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(String symbol) =>
            Client.GetCorrectionSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(String symbol) =>
            Client.GetLimitUpLimitDownSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetUpdatedBarSubscription(String symbol) =>
            Client.GetUpdatedBarSubscription(symbol);
    }

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
    /// with automatic reconnection support and provide optional reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    public static IAlpacaDataStreamingClient WithReconnect(
        this IAlpacaDataStreamingClient client) =>
        WithReconnect(client.EnsureNotNull(), ReconnectionParameters.Default);

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
    /// with automatic reconnection support with the default reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <param name="parameters">Reconnection parameters.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="parameters"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    public static IAlpacaDataStreamingClient WithReconnect(
        this IAlpacaDataStreamingClient client,
        ReconnectionParameters parameters) =>
        new ClientWithReconnection(client.EnsureNotNull(), parameters.EnsureNotNull());
}
