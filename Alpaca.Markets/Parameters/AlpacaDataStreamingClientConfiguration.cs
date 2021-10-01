namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaDataStreamingClient"/> class.
    /// </summary>
    public sealed class AlpacaDataStreamingClientConfiguration : StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataStreamingClientConfiguration"/> class.
        /// </summary>
        public AlpacaDataStreamingClientConfiguration()
            : base(Environments.Live.AlpacaDataStreamingApi)
        {
        }
    }
}
