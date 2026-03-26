using UltraTool.Numerics;

namespace UltraTool.Tests.Numerics;

/// <summary>
/// IntegerExtensions 单元测试
/// </summary>
public class IntegerExtensionsTests
{
    #region IsOdd 测试

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(3, true)]
    [InlineData(0, false)]
    [InlineData(-1, true)]
    [InlineData(-2, false)]
    public void IsOdd_VariousIntegers_ReturnsCorrectResult(int number, bool expected)
    {
        Assert.Equal(expected, number.IsOdd());
    }

    #endregion

    #region IsEven 测试

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(4, true)]
    [InlineData(-2, true)]
    [InlineData(-3, false)]
    public void IsEven_VariousIntegers_ReturnsCorrectResult(int number, bool expected)
    {
        Assert.Equal(expected, number.IsEven());
    }

    #endregion

    #region GetBitOneCount 测试

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    [InlineData(7, 3)]
    [InlineData(255, 8)]
    public void GetBitOneCount_VariousIntegers_ReturnsCorrectCount(int number, int expected)
    {
        Assert.Equal(expected, number.GetBitOneCount());
    }

    #endregion

    #region IsBitOne 测试

    [Fact]
    public void IsBitOne_SpecifiedBitIsOne_ReturnsTrue()
    {
        // 5 = 0b101, 第0位和第2位为1
        Assert.True(5.IsBitOne(0));
        Assert.True(5.IsBitOne(2));
    }

    [Fact]
    public void IsBitOne_SpecifiedBitIsZero_ReturnsFalse()
    {
        // 5 = 0b101, 第1位为0
        Assert.False(5.IsBitOne(1));
    }

    [Fact]
    public void IsBitOne_NegativeIndex_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 5.IsBitOne(-1));
    }

    [Fact]
    public void IsBitOne_IndexOutOfRange_ThrowsException()
    {
        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => 5.IsBitOne(33));
    }

    #endregion

    #region CalcSetBitOne 测试

    [Fact]
    public void CalcSetBitOne_SetSpecifiedBitToOne_ReturnsCorrectValue()
    {
        // 0 的第0位设为1 -> 1
        Assert.Equal(1, 0.CalcSetBitOne(0));
        // 0 的第2位设为1 -> 4
        Assert.Equal(4, 0.CalcSetBitOne(2));
        // 1(0b01) 的第1位设为1 -> 3(0b11)
        Assert.Equal(3, 1.CalcSetBitOne(1));
    }

    #endregion

    #region CalcSetBitZero 测试

    [Fact]
    public void CalcSetBitZero_SetSpecifiedBitToZero_ReturnsCorrectValue()
    {
        // 7(0b111) 的第0位设为0 -> 6(0b110)
        Assert.Equal(6, 7.CalcSetBitZero(0));
        // 7(0b111) 的第1位设为0 -> 5(0b101)
        Assert.Equal(5, 7.CalcSetBitZero(1));
    }

    #endregion
}
