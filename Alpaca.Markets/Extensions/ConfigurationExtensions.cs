namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extensions methods for creating the strongly-typed Alpaca REST API clients.
/// </summary>
[TypeForwardedFrom("Alpaca.Markets.Extensions, PublicKey=002400000480000094000000060200000024000052534131000400000100010075944e93a9e5981863853b33c605d0e67449ad9a0d90bbca7229ee052bb8d6f8136322bc7ca252ab472172a4a12e50d532c19fb0fe258c07a60820eff7c61753ff12993a0a0ce8cabe9a793c98be1f794e741ba776ce4b8df3873a6e7d6774e0c577eb3198b1930bc41ef82a829bf20b5edef2563a65213a04b49e7ae17e52b9")]
public static class ConfigurationExtensions
{
    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaCryptoDataClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaCryptoDataClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoDataClient GetClient(
        this AlpacaCryptoDataClientConfiguration configuration) =>
        new AlpacaCryptoDataClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaCryptoStreamingClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaCryptoStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoStreamingClient GetClient(
        this AlpacaCryptoStreamingClientConfiguration configuration) =>
        new AlpacaCryptoStreamingClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaNewsStreamingClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaNewsStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaNewsStreamingClient GetClient(
        this AlpacaNewsStreamingClientConfiguration configuration) =>
        new AlpacaNewsStreamingClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaDataClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaDataClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataClient GetClient(
        this AlpacaDataClientConfiguration configuration) =>
        new AlpacaDataClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaDataStreamingClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaDataStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataStreamingClient GetClient(
        this AlpacaDataStreamingClientConfiguration configuration) =>
        new AlpacaDataStreamingClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaStreamingClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaStreamingClient GetClient(
        this AlpacaStreamingClientConfiguration configuration) =>
        new AlpacaStreamingClient(configuration);

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaTradingClient"/> interface
    /// implementation using the <paramref name="configuration"/> argument.
    /// </summary>
    /// <param name="configuration">Client configuration parameters.</param>
    /// <returns>The new instance of <see cref="IAlpacaTradingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaTradingClient GetClient(
        this AlpacaTradingClientConfiguration configuration) =>
        new AlpacaTradingClient(configuration);
}
