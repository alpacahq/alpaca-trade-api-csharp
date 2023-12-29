namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaCryptoStreamingClient"/> instance.
/// </summary>
public sealed class AlpacaCryptoStreamingClientConfiguration : StreamingClientConfiguration
{
    private readonly HashSet<CryptoExchange> _exchanges = [];

    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaCryptoStreamingClientConfiguration()
        : base(Environments.Live.AlpacaCryptoStreamingApi)
    {
    }

    private AlpacaCryptoStreamingClientConfiguration(
        AlpacaCryptoStreamingClientConfiguration configuration,
        IEnumerable<CryptoExchange> exchanges)
        : base(configuration.ApiEndpoint)
    {
        SecurityId = configuration.SecurityId;
        _exchanges.UnionWith(exchanges);
    }

    /// <summary>
    /// Gets crypto exchanges list for data subscription (empty list means 'all exchanges').
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<CryptoExchange> Exchanges => _exchanges;
        
    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoStreamingClientConfiguration"/> object
    /// with the updated <see cref="AlpacaCryptoStreamingClientConfiguration.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new instance of the <see cref="AlpacaCryptoStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public AlpacaCryptoStreamingClientConfiguration WithExchanges(
        IEnumerable<CryptoExchange> exchanges) =>
        new(this, exchanges.EnsureNotNull());

    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoStreamingClientConfiguration"/> object
    /// with the updated <see cref="Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new instance of the <see cref="AlpacaCryptoStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public AlpacaCryptoStreamingClientConfiguration WithExchanges(
        params CryptoExchange[] exchanges) =>
        new(this, exchanges.EnsureNotNull());

    internal override Uri GetApiEndpoint() =>
        new UriBuilder(base.GetApiEndpoint())
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
                Query = new QueryBuilder()
                .AddParameter("exchanges", _exchanges)
                .AsStringAsync().Result
#pragma warning restore CA2012 // Use ValueTasks correctly
            }.Uri;
}
