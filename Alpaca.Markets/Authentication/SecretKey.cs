namespace Alpaca.Markets;

/// <summary>
/// Secret API key for Alpaca API authentication.
/// </summary>
public sealed class SecretKey : SecurityKey
{
    /// <summary>
    /// Creates a new instance of <see cref="SecretKey"/> object.
    /// </summary>
    /// <param name="keyId">Secret API key identifier.</param>
    /// <param name="value">Secret API key value.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="keyId"/> or <paramref name="value"/> argument is <c>null</c>.
    /// </exception>
    public SecretKey(
        String keyId,
        String value)
        : base(value) =>
        KeyId = keyId.EnsureNotNull();

    private String KeyId { get; }

    internal override IEnumerable<KeyValuePair<String, String>> GetAuthenticationHeaders()
    {
        // ReSharper disable StringLiteralTypo
        yield return new KeyValuePair<String, String>(
            "APCA-API-KEY-ID", KeyId);
        yield return new KeyValuePair<String, String>(
            "APCA-API-SECRET-KEY", Value);
        // ReSharper restore StringLiteralTypo
    }

    internal override JsonAuthRequest.JsonData GetAuthenticationData() =>
        new()
        {
            KeyId = KeyId,
            SecretKey = Value
        };

    internal override JsonAuthentication GetAuthentication() =>
        new()
        {
            Action = JsonAction.StreamingAuthenticate,
            SecretKey = Value,
            KeyId = KeyId
        };
}
