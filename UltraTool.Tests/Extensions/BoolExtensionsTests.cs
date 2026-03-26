using UltraTool.Extensions;

namespace UltraTool.Tests.Extensions;

/// <summary>
/// BoolExtensions 单元测试
/// </summary>
public class BoolExtensionsTests
{
    #region CountTrue 测试

    [Fact]
    public void CountTrue_MixedBoolSequence_ReturnsTrueCount()
    {
        var list = new List<bool> { true, false, true, true, false };
        Assert.Equal(3, list.CountTrue());
    }

    [Fact]
    public void CountTrue_AllFalse_ReturnsZero()
    {
        var list = new List<bool> { false, false, false };
        Assert.Equal(0, list.CountTrue());
    }

    [Fact]
    public void CountTrue_NullableBoolSequence_ReturnsTrueCount()
    {
        var list = new List<bool?> { true, false, null, true, null };
        Assert.Equal(2, list.CountTrue());
    }

    #endregion

    #region CountFalse 测试

    [Fact]
    public void CountFalse_MixedBoolSequence_ReturnsFalseCount()
    {
        var list = new List<bool> { true, false, true, true, false };
        Assert.Equal(2, list.CountFalse());
    }

    [Fact]
    public void CountFalse_NullableBoolSequence_ReturnsFalseCount()
    {
        var list = new List<bool?> { true, false, null, true, false };
        Assert.Equal(2, list.CountFalse());
    }

    #endregion

    #region CountNotTrue 测试

    [Fact]
    public void CountNotTrue_NullableBoolSequence_ReturnsNotTrueCount()
    {
        var list = new List<bool?> { true, false, null, true, null };
        // false=1, null=2 -> 3
        Assert.Equal(3, list.CountNotTrue());
    }

    #endregion

    #region CountNotFalse 测试

    [Fact]
    public void CountNotFalse_NullableBoolSequence_ReturnsNotFalseCount()
    {
        var list = new List<bool?> { true, false, null, true, null };
        // true=2, null=2 -> 4
        Assert.Equal(4, list.CountNotFalse());
    }

    #endregion
}
