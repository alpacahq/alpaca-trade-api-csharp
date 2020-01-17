using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// OAuth key for Alpaca/Polygon APIs authentication.
    /// </summary>
    public sealed class OAuthKey : SecurityKey
    {
        /// <summary>
        /// Creates new instance of <see cref="OAuthKey"/> object.
        /// </summary>
        /// <param name="value">OAuth key.</param>
        public OAuthKey(String value) : base(value) {}

        internal override void AddAuthenticationHeader(
            HttpClient httpClient,
            String keyId)
        {
            httpClient.DefaultRequestHeaders.Add(
                "Authorization", "Bearer " + Value);
        }
    }
}
