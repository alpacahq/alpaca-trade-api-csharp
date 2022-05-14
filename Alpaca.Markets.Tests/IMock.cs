namespace Alpaca.Markets.Tests;

internal interface IMock
{
    void AddGet(
        String request,
        JToken response);
}
