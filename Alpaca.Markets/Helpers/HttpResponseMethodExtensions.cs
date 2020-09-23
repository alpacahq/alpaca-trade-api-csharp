using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static class HttpResponseMethodExtensions
    {
        public static async Task<TApi> DeserializeAsync<TApi, TJson>(
            this HttpResponseMessage response)
            where TJson : TApi
        {
#if NETSTANDARD2_1
            await using var stream = await response.Content.ReadAsStreamAsync()
#else
            using var stream = await response.Content.ReadAsStreamAsync()
#endif
                .ConfigureAwait(false);
            using var reader = new JsonTextReader(new StreamReader(stream));

            if (response.IsSuccessStatusCode)
            {
                return new JsonSerializer
                           {
                               DateParseHandling = DateParseHandling.None,
                               Culture = CultureInfo.InvariantCulture
                           }
                           .Deserialize<TJson>(reader) ??
                       throw new RestClientErrorException(
                           "Unable to deserialize JSON response message.");
            }

            throw getException(response, reader);
        }

        public static async Task<Boolean> IsSuccessStatusCodeAsync(
            this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

#if NETSTANDARD2_1
            await using var stream = await response.Content.ReadAsStreamAsync()
#else
            using var stream = await response.Content.ReadAsStreamAsync()
#endif
                .ConfigureAwait(false);
            using var reader = new JsonTextReader(new StreamReader(stream));

            throw getException(response, reader);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "We wrap all exceptions into the single exception type.")]
        private static RestClientErrorException getException(
            HttpResponseMessage response,
            JsonReader reader)
        {
            try
            {
                // ReSharper disable once ConstantNullCoalescingCondition
                var jsonError = new JsonSerializer()
                    .Deserialize<JsonError>(reader) ?? new JsonError();

                return jsonError.Code == 0 ||
                       String.IsNullOrEmpty(jsonError.Message)
                    ? new RestClientErrorException(response)
                    : new RestClientErrorException(jsonError);
            }
            catch (Exception exception)
            {
                return new RestClientErrorException(response, exception);
            }
        }
    }
}