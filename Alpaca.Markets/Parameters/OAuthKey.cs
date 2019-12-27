using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class OAuthKey : SecurityKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
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
