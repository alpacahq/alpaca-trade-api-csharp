using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Secret API key for Alpaca/Polygon APIs authentication.
    /// </summary>
    public sealed class SecretKey : SecurityKey
    {
        /// <summary>
        /// Creates new instance of <see cref="SecretKey"/> object.
        /// </summary>
        /// <param name="value">Secret API key value.</param>
        public SecretKey(String value) : base(value) {}

        internal override void AddAuthenticationHeader(
            HttpClient httpClient,
            String keyId)
        {
            httpClient.DefaultRequestHeaders.Add(
                "APCA-API-KEY-ID", keyId);
            httpClient.DefaultRequestHeaders.Add(
                "APCA-API-SECRET-KEY", Value);
        }
    }
}
