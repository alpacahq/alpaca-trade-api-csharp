﻿namespace Alpaca.Markets.Tests;

public sealed class ConvertersTest
{
    private record struct EnumWrapper<T>
    {
        public T Item { get; init; }
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

    [Fact]
    public void CryptoExchangeEnumConverterWorks()
    {
        var original = new EnumWrapper<CryptoExchange> { Item = (CryptoExchange)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"MarsExchange\"");
        var converted = JsonConvert.DeserializeObject<EnumWrapper<CryptoExchange>>(json);

        Assert.NotEqual(original.Item, converted.Item);
        Assert.Equal(CryptoExchange.Unknown, converted.Item);
    }

    [Fact]
    public void AssetAttributesEnumConverterWorks()
    {
        var original = new EnumWrapper<AssetAttributes> { Item = (AssetAttributes)42 };
        var json = JsonConvert.SerializeObject(original)
            .Replace("42", "\"VerySpecificAsset\"");
        var converted = JsonConvert.DeserializeObject<EnumWrapper<AssetAttributes>>(json);

        Assert.NotEqual(original.Item, converted.Item);
        Assert.Equal(AssetAttributes.Unknown, converted.Item);
    }
}
