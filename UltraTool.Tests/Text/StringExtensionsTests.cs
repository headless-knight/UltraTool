using UltraTool.Text;

namespace UltraTool.Tests.Text;

/// <summary>
/// StringExtensions 单元测试
/// </summary>
public class StringExtensionsTests
{
    #region EmptyIfNull 测试

    [Fact]
    public void EmptyIfNull_Null_ReturnsEmptyString()
    {
        string? str = null;
        Assert.Equal(string.Empty, str.EmptyIfNull());
    }

    [Fact]
    public void EmptyIfNull_NonNull_ReturnsOriginalString()
    {
        var str = "hello";
        Assert.Equal("hello", str.EmptyIfNull());
    }

    #endregion

    #region IsEmpty / IsNotEmpty 测试

    [Fact]
    public void IsEmpty_EmptyString_ReturnsTrue()
    {
        Assert.True(string.Empty.IsEmpty());
    }

    [Fact]
    public void IsEmpty_NonEmptyString_ReturnsFalse()
    {
        Assert.False("hello".IsEmpty());
    }

    [Fact]
    public void IsNotEmpty_NonEmptyString_ReturnsTrue()
    {
        Assert.True("hello".IsNotEmpty());
    }

    [Fact]
    public void IsNotEmpty_EmptyString_ReturnsFalse()
    {
        Assert.False(string.Empty.IsNotEmpty());
    }

    #endregion

    #region IsBlank / IsNotBlank 测试

    [Fact]
    public void IsBlank_AllWhitespaceString_ReturnsTrue()
    {
        Assert.True("   ".IsBlank());
    }

    [Fact]
    public void IsBlank_ContainsNonWhitespace_ReturnsFalse()
    {
        Assert.False(" a ".IsBlank());
    }

    [Fact]
    public void IsBlank_EmptyString_ReturnsTrue()
    {
        Assert.True(string.Empty.IsBlank());
    }

    [Fact]
    public void IsNotBlank_ContainsNonWhitespace_ReturnsTrue()
    {
        Assert.True("hello".IsNotBlank());
    }

    [Fact]
    public void IsNotBlank_AllWhitespaceString_ReturnsFalse()
    {
        Assert.False("   ".IsNotBlank());
    }

    #endregion

    #region IsNullOrEmpty / IsNotNullOrEmpty 测试

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("hello", false)]
    public void IsNullOrEmpty_VariousInputs_ReturnsCorrectResult(string? str, bool expected)
    {
        Assert.Equal(expected, str.IsNullOrEmpty());
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("hello", true)]
    public void IsNotNullOrEmpty_VariousInputs_ReturnsCorrectResult(string? str, bool expected)
    {
        Assert.Equal(expected, str.IsNotNullOrEmpty());
    }

    #endregion

    #region IsNullOrBlank / IsNotNullOrBlank 测试

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("hello", false)]
    public void IsNullOrBlank_VariousInputs_ReturnsCorrectResult(string? str, bool expected)
    {
        Assert.Equal(expected, str.IsNullOrBlank());
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("hello", true)]
    public void IsNotNullOrBlank_VariousInputs_ReturnsCorrectResult(string? str, bool expected)
    {
        Assert.Equal(expected, str.IsNotNullOrBlank());
    }

    #endregion

    #region GetBytes 测试

    [Fact]
    public void GetBytes_UTF8_ReturnsCorrectByteArray()
    {
        var bytes = "ABC".GetBytes();
        Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, bytes);
    }

    [Fact]
    public void GetBytes_WriteToSpan_WritesCorrectly()
    {
        var bytes = new byte[3];
        "ABC".GetBytes(bytes);
        Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, bytes);
    }

    #endregion

    #region GetBytesPooled 测试

    [Fact]
    public void GetBytesPooled_ReturnsCorrectPooledArray()
    {
        using var pooled = "ABC".GetBytesPooled();
        Assert.Equal(3, pooled.Length);
        Assert.Equal(new byte[] { 0x41, 0x42, 0x43 }, pooled.ReadOnlySpan.ToArray());
    }

    #endregion
}
