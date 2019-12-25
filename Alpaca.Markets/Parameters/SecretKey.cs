using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SecretKey : SecurityKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
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
