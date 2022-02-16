namespace Alpaca.Markets.Tests;

public sealed class ConvertersTest
{
    private record struct EnumWrapper<T>
    {
        public T Item { get; set; }
    }

    [Fact]
    public void OrderSideEnumConverterWorks()
    {
        var original = new EnumWrapper<OrderSide> { Item = (OrderSide)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"SellShort\"");
        var converted = JsonConvert.DeserializeObject<EnumWrapper<OrderSide>>(json);

        Assert.NotEqual(original.Item, converted.Item);
        Assert.Equal(OrderSide.Sell, converted.Item);
    }

    [Fact]
    public void ExchangeEnumConverterWorks()
    {
        var original = new EnumWrapper<Exchange> { Item = (Exchange)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"MarsExchange\"");
        var converted = JsonConvert.DeserializeObject<EnumWrapper<Exchange>>(json);

        Assert.NotEqual(original.Item, converted.Item);
        Assert.Equal(Exchange.Unknown, converted.Item);
    }
}
