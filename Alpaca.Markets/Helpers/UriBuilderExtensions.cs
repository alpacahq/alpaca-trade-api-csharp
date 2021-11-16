namespace Alpaca.Markets;

internal static class UriBuilderExtensions
{
    public static UriBuilder AppendPath(
        this UriBuilder uriBuilder,
        String pathSegment)
    {
        uriBuilder.Path += pathSegment;
        return uriBuilder;
    }
}
