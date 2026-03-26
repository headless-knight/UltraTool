using UltraTool.Extensions;

namespace UltraTool.Tests.Extensions;

/// <summary>
/// NullableExtensions 单元测试
/// </summary>
public class NullableExtensionsTests
{
    #region RequireNonNull ReferenceType Tests

    [Fact]
    public void RequireNonNull_ReferenceTypeNonNull_ReturnsOriginalValue()
    {
        string? value = "hello";
        var result = value.RequireNonNull();
        Assert.Equal("hello", result);
    }

    [Fact]
    public void RequireNonNull_ReferenceTypeNull_ThrowsException()
    {
        string? value = null;
        Assert.Throws<ArgumentNullException>(() => value.RequireNonNull());
    }

    #endregion

    #region RequireNonNull ValueType Tests

    [Fact]
    public void RequireNonNull_ValueTypeNonNull_ReturnsOriginalValue()
    {
        int? value = 42;
        var result = value.RequireNonNull();
        Assert.Equal(42, result);
    }

    [Fact]
    public void RequireNonNull_ValueTypeNull_ThrowsException()
    {
        int? value = null;
        Assert.Throws<ArgumentNullException>(() => value.RequireNonNull());
    }

    #endregion
}
