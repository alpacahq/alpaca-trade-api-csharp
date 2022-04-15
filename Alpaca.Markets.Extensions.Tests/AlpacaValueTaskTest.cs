namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaValueTaskTest
{
    [Fact]
    public void DefaultValueWorks()
    {
        var valueTask = new AlpacaValueTask();

        Assert.True(valueTask.GetAwaiter().IsCompleted);
    }
}
