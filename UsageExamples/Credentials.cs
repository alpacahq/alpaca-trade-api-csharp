using Alpaca.Markets;

namespace UsageExamples;

internal static class Credentials
{
    public static SecretKey GetPaperSecretKey()
    {
        var apiKey = Environment.GetEnvironmentVariable("APCA_API_KEY_ID");
        var apiSecret = Environment.GetEnvironmentVariable("APCA_API_SECRET_KEY");

        if (String.IsNullOrWhiteSpace(apiKey) || String.IsNullOrWhiteSpace(apiSecret))
        {
            throw new InvalidOperationException(
                "Set APCA_API_KEY_ID and APCA_API_SECRET_KEY to paper-account credentials.");
        }

        return new SecretKey(apiKey, apiSecret);
    }
}
