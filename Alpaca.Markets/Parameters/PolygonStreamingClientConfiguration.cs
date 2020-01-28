namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="PolygonStreamingClient"/> class.
    /// </summary>
    public sealed class PolygonStreamingClientConfiguration : StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClientConfiguration"/> class.
        /// </summary>
        public PolygonStreamingClientConfiguration()
            : base(Environments.Live.PolygonStreamingApi)
        {
        }
    }
}
