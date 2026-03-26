using UltraTool.Numerics;

namespace UltraTool.Tests.Numerics;

/// <summary>
/// FloatExtensions 单元测试
/// </summary>
public class FloatExtensionsTests
{
    #region ApproximateTo float 测试

    [Fact]
    public void ApproximateTo_Float_ApproximatelyEqual_ReturnsTrue()
    {
        Assert.True(1.0f.ApproximateTo(1.00005f));
    }

    [Fact]
    public void ApproximateTo_Float_LargeDifference_ReturnsFalse()
    {
        Assert.False(1.0f.ApproximateTo(1.1f));
    }

    [Fact]
    public void ApproximateTo_Float_CustomTolerance_ReturnsCorrectResult()
    {
        Assert.True(1.0f.ApproximateTo(1.05f, 0.1f));
        Assert.False(1.0f.ApproximateTo(1.2f, 0.1f));
    }

    #endregion

    #region ApproximateTo double 测试

    [Fact]
    public void ApproximateTo_Double_ApproximatelyEqual_ReturnsTrue()
    {
        Assert.True(1.0.ApproximateTo(1.00005));
    }

    [Fact]
    public void ApproximateTo_Double_LargeDifference_ReturnsFalse()
    {
        Assert.False(1.0.ApproximateTo(1.1));
    }

    #endregion

    #region ApproximateTo decimal 测试

    [Fact]
    public void ApproximateTo_Decimal_ApproximatelyEqual_ReturnsTrue()
    {
        Assert.True(1.0m.ApproximateTo(1.00005m));
    }

    [Fact]
    public void ApproximateTo_Decimal_LargeDifference_ReturnsFalse()
    {
        Assert.False(1.0m.ApproximateTo(1.1m));
    }

    #endregion

    #region ApproximateToZero 测试

    [Fact]
    public void ApproximateToZero_Float_NearZero_ReturnsTrue()
    {
        Assert.True(0.00001f.ApproximateToZero());
    }

    [Fact]
    public void ApproximateToZero_Float_NonZero_ReturnsFalse()
    {
        Assert.False(1.0f.ApproximateToZero());
    }

    [Fact]
    public void ApproximateToZero_Double_NearZero_ReturnsTrue()
    {
        Assert.True(0.00001.ApproximateToZero());
    }

    [Fact]
    public void ApproximateToZero_Double_NonZero_ReturnsFalse()
    {
        Assert.False(1.0.ApproximateToZero());
    }

    [Fact]
    public void ApproximateToZero_Decimal_NearZero_ReturnsTrue()
    {
        Assert.True(0.00001m.ApproximateToZero());
    }

    [Fact]
    public void ApproximateToZero_Decimal_NonZero_ReturnsFalse()
    {
        Assert.False(1.0m.ApproximateToZero());
    }

    #endregion
}
