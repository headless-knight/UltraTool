using UltraTool.Compares;

namespace UltraTool.Tests.Compares;

/// <summary>
/// CompareSymbolExtensions 单元测试
/// </summary>
public class CompareSymbolExtensionsTests
{
    #region ToOpposite 测试

    [Theory]
    [InlineData(CompareSymbol.Less, CompareSymbol.GreaterEquals)]
    [InlineData(CompareSymbol.Greater, CompareSymbol.LessEquals)]
    [InlineData(CompareSymbol.Equals, CompareSymbol.NotEquals)]
    [InlineData(CompareSymbol.LessEquals, CompareSymbol.Greater)]
    [InlineData(CompareSymbol.GreaterEquals, CompareSymbol.Less)]
    [InlineData(CompareSymbol.NotEquals, CompareSymbol.Equals)]
    public void ToOpposite_VariousSymbols_ReturnsCorrectOpposite(CompareSymbol input, CompareSymbol expected)
    {
        Assert.Equal(expected, input.ToOpposite());
    }

    #endregion

    #region ToSymbolString 测试

    [Theory]
    [InlineData(CompareSymbol.Less, "<")]
    [InlineData(CompareSymbol.Greater, ">")]
    [InlineData(CompareSymbol.Equals, "=")]
    [InlineData(CompareSymbol.LessEquals, "<=")]
    [InlineData(CompareSymbol.GreaterEquals, ">=")]
    [InlineData(CompareSymbol.NotEquals, "!=")]
    public void ToSymbolString_VariousSymbols_ReturnsCorrectString(CompareSymbol symbol, string expected)
    {
        Assert.Equal(expected, symbol.ToSymbolString());
    }

    #endregion

    #region ToOppositeString 测试

    [Theory]
    [InlineData(CompareSymbol.Less, ">=")]
    [InlineData(CompareSymbol.Greater, "<=")]
    [InlineData(CompareSymbol.Equals, "!=")]
    public void ToOppositeString_VariousSymbols_ReturnsCorrectOppositeString(CompareSymbol symbol, string expected)
    {
        Assert.Equal(expected, symbol.ToOppositeString());
    }

    #endregion
}
