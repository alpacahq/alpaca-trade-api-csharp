using System.Globalization;

namespace Alpaca.Markets;

internal static class HttpResponseMethodExtensions
{
    public static async Task<TApi> DeserializeAsync<TApi, TJson>(
        this HttpResponseMessage response)
        where TJson : TApi
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        await using var _ = stream.ConfigureAwait(false);
#else
        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#endif
        // ReSharper disable once UseAwaitUsing
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

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        await using var _ = stream.ConfigureAwait(false);
#else
        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#endif
        // ReSharper disable once UseAwaitUsing
        using var reader = new JsonTextReader(new StreamReader(stream));

        throw getException(response, reader);
    }

    [SuppressMessage(
        "Design", "CA1031:Do not catch general exception types",
        Justification = "We wrap all exceptions into the single exception type.")]
    private static RestClientErrorException getException(
        HttpResponseMessage response,
        JsonReader reader)
    {
        try
        {
            var jsonError = new JsonSerializer()
                .Deserialize<JsonError>(reader) ?? new JsonError();
            jsonError.Code ??= (Int32)response.StatusCode;

            return String.IsNullOrEmpty(jsonError.Message)
                ? new RestClientErrorException(response)
                : new RestClientErrorException(response, jsonError);
        }
        catch (JsonReaderException)
        {
            return new RestClientErrorException(response);
        }
        catch (Exception exception)
        {
            return new RestClientErrorException(response, exception);
        }
    }
}
