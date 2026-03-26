using UltraTool.Helpers;

namespace UltraTool.Tests.Helpers;

/// <summary>
/// MathHelper 单元测试
/// </summary>
public class MathHelperTests
{
    #region Min 测试

    [Theory]
    [InlineData(1, 2, 1)]
    [InlineData(5, 3, 3)]
    [InlineData(-1, 1, -1)]
    [InlineData(0, 0, 0)]
    public void Min_TwoIntegers_ReturnsSmallerValue(int a, int b, int expected)
    {
        Assert.Equal(expected, MathHelper.Min(a, b));
    }

    #endregion

    #region Max 测试

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(5, 3, 5)]
    [InlineData(-1, 1, 1)]
    [InlineData(0, 0, 0)]
    public void Max_TwoIntegers_ReturnsLargerValue(int a, int b, int expected)
    {
        Assert.Equal(expected, MathHelper.Max(a, b));
    }

    #endregion

    #region Middle 测试

    [Theory]
    [InlineData(1, 2, 3, 2)]
    [InlineData(3, 1, 2, 2)]
    [InlineData(2, 3, 1, 2)]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 1, 2, 1)]
    [InlineData(5, 3, 7, 5)]
    public void Middle_ThreeIntegers_ReturnsMiddleValue(int a, int b, int c, int expected)
    {
        Assert.Equal(expected, MathHelper.Middle(a, b, c));
    }

    [Fact]
    public void Middle_WithComparer_ReturnsMiddleValue()
    {
        var comparer = Comparer<int>.Default;
        Assert.Equal(2, MathHelper.Middle(1, 2, 3, comparer));
        Assert.Equal(5, MathHelper.Middle(5, 3, 7, comparer));
    }

    #endregion

    #region MinMax 测试

    [Fact]
    public void MinMax_NormalValues_ReturnsCorrectTuple()
    {
        var (min, max) = MathHelper.MinMax(5, 3);
        Assert.Equal(3, min);
        Assert.Equal(5, max);
    }

    [Fact]
    public void MinMax_EqualValues_ReturnsSameTuple()
    {
        var (min, max) = MathHelper.MinMax(4, 4);
        Assert.Equal(4, min);
        Assert.Equal(4, max);
    }

    [Fact]
    public void MinMax_SortedValues_ReturnsCorrectTuple()
    {
        var (min, max) = MathHelper.MinMax(1, 10);
        Assert.Equal(1, min);
        Assert.Equal(10, max);
    }

    #endregion

    #region Factorial 测试

    [Theory]
    [InlineData(0, 1L)]
    [InlineData(1, 1L)]
    [InlineData(2, 2L)]
    [InlineData(5, 120L)]
    [InlineData(10, 3628800L)]
    [InlineData(20, 2432902008176640000L)]
    public void Factorial_ValidValue_ReturnsCorrectFactorial(int n, long expected)
    {
        Assert.Equal(expected, MathHelper.Factorial(n));
    }

    [Fact]
    public void Factorial_NegativeNumber_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.Factorial(-1));
    }

    [Fact]
    public void Factorial_OutOfRange_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => MathHelper.Factorial(200));
    }

    #endregion

    #region Gcd 测试

    [Theory]
    [InlineData(12, 8, 4)]
    [InlineData(15, 10, 5)]
    [InlineData(7, 3, 1)]
    [InlineData(100, 25, 25)]
    [InlineData(6, 6, 6)]
    public void Gcd_TwoPositiveIntegers_ReturnsGcd(int a, int b, int expected)
    {
        Assert.Equal(expected, MathHelper.Gcd(a, b));
    }

    #endregion

    #region Multiple 测试

    [Theory]
    [InlineData(4, 6, 12)]
    [InlineData(3, 5, 15)]
    [InlineData(7, 7, 7)]
    [InlineData(12, 8, 24)]
    public void Multiple_TwoPositiveIntegers_ReturnsLcm(int a, int b, int expected)
    {
        Assert.Equal(expected, MathHelper.Multiple(a, b));
    }

    #endregion
}
