namespace Alpaca.Markets.Tests;

public sealed class ConvertersTest
{
    private readonly record struct EnumWrapper<T>
    {
        public T Item { get; init; }
    }

    [Fact]
    public void OrderSideEnumConverterWorks()
    {
        var original = new EnumWrapper<OrderSide> { Item = (OrderSide)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"SellShort\"");
        var converted1 = JsonConvert.DeserializeObject<EnumWrapper<OrderSide>>(json);

        Assert.NotEqual(original.Item, converted1.Item);
        Assert.Equal(OrderSide.Sell, converted1.Item);

        var converted2 = JsonConvert.DeserializeObject<EnumWrapper<OrderSide>>(
            json.Replace("42", "\"SellShort\""));

        Assert.NotEqual(original.Item, converted2.Item);
        Assert.Equal(OrderSide.Sell, converted2.Item);

        Assert.Equal(converted1.Item, converted2.Item);
    }

    [Fact]
    public void ExchangeEnumConverterWorks()
    {
        var original = new EnumWrapper<Exchange> { Item = (Exchange)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"MarsExchange\"");
        var converted1 = JsonConvert.DeserializeObject<EnumWrapper<Exchange>>(json);

        Assert.NotEqual(original.Item, converted1.Item);
        Assert.Equal(Exchange.Unknown, converted1.Item);

        var converted2 = JsonConvert.DeserializeObject<EnumWrapper<Exchange>>(
            json.Replace("42", "\"MarsExchange\""));

        Assert.NotEqual(original.Item, converted2.Item);
        Assert.Equal(Exchange.Unknown, converted2.Item);

        Assert.Equal(converted1.Item, converted2.Item);
    }

    [Fact]
    public void CryptoExchangeEnumConverterWorks()
    {
        var original = new EnumWrapper<CryptoExchange> { Item = (CryptoExchange)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"MarsExchange\"");
        var converted1 = JsonConvert.DeserializeObject<EnumWrapper<CryptoExchange>>(json);

        Assert.NotEqual(original.Item, converted1.Item);
        Assert.Equal(CryptoExchange.Unknown, converted1.Item);

        var converted2 = JsonConvert.DeserializeObject<EnumWrapper<CryptoExchange>>(
            json.Replace("42", "\"MarsExchange\""));

        Assert.NotEqual(original.Item, converted2.Item);
        Assert.Equal(CryptoExchange.Unknown, converted2.Item);

        Assert.Equal(converted1.Item, converted2.Item);
    }

    [Fact]
    public void AssetAttributesEnumConverterWorks()
    {
        var original = new EnumWrapper<AssetAttributes> { Item = (AssetAttributes)42 };
        var json = JsonConvert.SerializeObject(original);
        var converted1 = JsonConvert.DeserializeObject<EnumWrapper<AssetAttributes>>(json);

        Assert.NotEqual(original.Item, converted1.Item);
        Assert.Equal(AssetAttributes.Unknown, converted1.Item);

        var converted2 = JsonConvert.DeserializeObject<EnumWrapper<AssetAttributes>>(
            json.Replace("42", "\"VerySpecificAsset\""));

        Assert.NotEqual(original.Item, converted2.Item);
        Assert.Equal(AssetAttributes.Unknown, converted2.Item);

        Assert.Equal(converted1.Item, converted2.Item);
    }
}
