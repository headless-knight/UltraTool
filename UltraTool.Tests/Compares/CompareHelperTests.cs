using UltraTool.Compares;

namespace UltraTool.Tests.Compares;

/// <summary>
/// CompareHelper 单元测试
/// </summary>
public class CompareHelperTests
{
    #region Compare 测试

    [Theory]
    [InlineData(1, 2, CompareSymbol.Less, true)]
    [InlineData(2, 1, CompareSymbol.Less, false)]
    [InlineData(2, 1, CompareSymbol.Greater, true)]
    [InlineData(1, 2, CompareSymbol.Greater, false)]
    [InlineData(1, 1, CompareSymbol.Equals, true)]
    [InlineData(1, 2, CompareSymbol.Equals, false)]
    [InlineData(1, 2, CompareSymbol.LessEquals, true)]
    [InlineData(1, 1, CompareSymbol.LessEquals, true)]
    [InlineData(2, 1, CompareSymbol.LessEquals, false)]
    [InlineData(2, 1, CompareSymbol.GreaterEquals, true)]
    [InlineData(1, 1, CompareSymbol.GreaterEquals, true)]
    [InlineData(1, 2, CompareSymbol.GreaterEquals, false)]
    [InlineData(1, 2, CompareSymbol.NotEquals, true)]
    [InlineData(1, 1, CompareSymbol.NotEquals, false)]
    public void Compare_VariousSymbols_ReturnsCorrectResult(int a, int b, CompareSymbol symbol, bool expected)
    {
        Assert.Equal(expected, CompareHelper.Compare(a, b, symbol));
    }

    #endregion

    #region AllCompare 测试

    [Fact]
    public void AllCompare_AllElementsMeetCondition_ReturnsTrue()
    {
        var range = new[] { 1, 2, 3 };
        Assert.True(CompareHelper.AllCompare(range, 5, CompareSymbol.Less));
    }

    [Fact]
    public void AllCompare_SomeElementsDoNotMeet_ReturnsFalse()
    {
        var range = new[] { 1, 2, 6 };
        Assert.False(CompareHelper.AllCompare(range, 5, CompareSymbol.Less));
    }

    #endregion

    #region InRange 测试

    [Fact]
    public void InRange_InsideClosedRange_ReturnsTrue()
    {
        Assert.True(CompareHelper.InRange(5, 1, 10, RangeMode.Close));
    }

    [Fact]
    public void InRange_ClosedRangeBoundary_ReturnsTrue()
    {
        Assert.True(CompareHelper.InRange(1, 1, 10, RangeMode.Close));
        Assert.True(CompareHelper.InRange(10, 1, 10, RangeMode.Close));
    }

    [Fact]
    public void InRange_OpenRangeBoundary_ReturnsFalse()
    {
        Assert.False(CompareHelper.InRange(1, 1, 10, RangeMode.Open));
        Assert.False(CompareHelper.InRange(10, 1, 10, RangeMode.Open));
    }

    [Fact]
    public void InRange_InsideOpenRange_ReturnsTrue()
    {
        Assert.True(CompareHelper.InRange(5, 1, 10, RangeMode.Open));
    }

    [Fact]
    public void InRange_OpenCloseBoundaryTest()
    {
        Assert.False(CompareHelper.InRange(1, 1, 10, RangeMode.OpenClose));
        Assert.True(CompareHelper.InRange(10, 1, 10, RangeMode.OpenClose));
    }

    [Fact]
    public void InRange_CloseOpenBoundaryTest()
    {
        Assert.True(CompareHelper.InRange(1, 1, 10, RangeMode.CloseOpen));
        Assert.False(CompareHelper.InRange(10, 1, 10, RangeMode.CloseOpen));
    }

    [Fact]
    public void InRange_StartGreaterThanEnd_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => CompareHelper.InRange(5, 10, 1));
    }

    #endregion

    #region OutRange 测试

    [Fact]
    public void OutRange_OutsideClosedRange_ReturnsTrue()
    {
        Assert.True(CompareHelper.OutRange(0, 1, 10, RangeMode.Close));
        Assert.True(CompareHelper.OutRange(11, 1, 10, RangeMode.Close));
    }

    [Fact]
    public void OutRange_ClosedRangeBoundary_SinglePointReturnsFalse()
    {
        // Close mode OutRange: start < value || end > value
        // Only returns false when value == start == end
        Assert.False(CompareHelper.OutRange(5, 5, 5, RangeMode.Close));
        // Boundary value in non-single-point range, OutRange returns true
        Assert.True(CompareHelper.OutRange(1, 1, 10, RangeMode.Close));
        Assert.True(CompareHelper.OutRange(10, 1, 10, RangeMode.Close));
    }

    [Fact]
    public void OutRange_StartGreaterThanEnd_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => CompareHelper.OutRange(5, 10, 1));
    }

    #endregion

    #region ParseSymbol 测试

    [Theory]
    [InlineData("<", CompareSymbol.Less)]
    [InlineData(">", CompareSymbol.Greater)]
    [InlineData("=", CompareSymbol.Equals)]
    [InlineData("<=", CompareSymbol.LessEquals)]
    [InlineData(">=", CompareSymbol.GreaterEquals)]
    [InlineData("!=", CompareSymbol.NotEquals)]
    public void ParseSymbol_ValidSymbol_ReturnsCorrectEnum(string symbol, CompareSymbol expected)
    {
        Assert.Equal(expected, CompareHelper.ParseSymbol(symbol));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid")]
    public void ParseSymbol_InvalidSymbol_ThrowsException(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => CompareHelper.ParseSymbol(symbol));
    }

    #endregion

    #region TryParseSymbol 测试

    [Fact]
    public void TryParseSymbol_ValidSymbol_ReturnsTrue()
    {
        Assert.True(CompareHelper.TryParseSymbol("<", out var symbol));
        Assert.Equal(CompareSymbol.Less, symbol);
    }

    [Fact]
    public void TryParseSymbol_InvalidSymbol_ReturnsFalse()
    {
        Assert.False(CompareHelper.TryParseSymbol("invalid", out _));
    }

    [Fact]
    public void TryParseSymbol_Null_ReturnsFalse()
    {
        Assert.False(CompareHelper.TryParseSymbol(null, out _));
    }

    #endregion
}
