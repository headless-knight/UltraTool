using UltraTool.Helpers;

namespace UltraTool.Tests.Helpers;

/// <summary>
/// ConvertHelper 单元测试
/// </summary>
public class ConvertHelperTests
{
    #region ToHexChars 测试

    [Fact]
    public void ToHexChars_EmptyArray_ReturnsEmptyCharArray()
    {
        var result = ConvertHelper.ToHexChars(ReadOnlySpan<byte>.Empty);
        Assert.Empty(result);
    }

    [Fact]
    public void ToHexChars_UpperCaseMode_ReturnsUpperCaseHex()
    {
        byte[] source = [0xAB, 0xCD, 0xEF];
        var result = ConvertHelper.ToHexChars(source, lowerCase: false);
        Assert.Equal("ABCDEF", new string(result));
    }

    [Fact]
    public void ToHexChars_LowerCaseMode_ReturnsLowerCaseHex()
    {
        byte[] source = [0xAB, 0xCD, 0xEF];
        var result = ConvertHelper.ToHexChars(source, lowerCase: true);
        Assert.Equal("abcdef", new string(result));
    }

    [Fact]
    public void ToHexChars_SingleByte_ConvertsCorrectly()
    {
        byte[] source = [0x0F];
        var result = ConvertHelper.ToHexChars(source, lowerCase: false);
        Assert.Equal("0F", new string(result));
    }

    [Fact]
    public void ToHexChars_WriteToSpan_ReturnsCorrectLength()
    {
        byte[] source = [0xAB, 0xCD];
        var destination = new char[4];
        var written = ConvertHelper.ToHexChars(source, destination, lowerCase: true);
        Assert.Equal(2, written);
        Assert.Equal("abcd", new string(destination));
    }

    #endregion

    #region ToHexString 测试

    [Fact]
    public void ToHexString_EmptyArray_ReturnsEmptyString()
    {
        var result = ConvertHelper.ToHexString(ReadOnlySpan<byte>.Empty);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHexString_UpperCaseMode_ReturnsUpperCaseHexString()
    {
        byte[] source = [0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF];
        var result = ConvertHelper.ToHexString(source, lowerCase: false);
        Assert.Equal("0123456789ABCDEF", result);
    }

    [Fact]
    public void ToHexString_LowerCaseMode_ReturnsLowerCaseHexString()
    {
        byte[] source = [0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF];
        var result = ConvertHelper.ToHexString(source, lowerCase: true);
        Assert.Equal("0123456789abcdef", result);
    }

    #endregion

    #region FromHexChar 测试

    [Theory]
    [InlineData('0', 0)]
    [InlineData('9', 9)]
    [InlineData('a', 10)]
    [InlineData('f', 15)]
    [InlineData('A', 10)]
    [InlineData('F', 15)]
    public void FromHexChar_ValidChar_ReturnsCorrectByteValue(char ch, byte expected)
    {
        var result = ConvertHelper.FromHexChar(ch);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData('g')]
    [InlineData('G')]
    [InlineData('z')]
    [InlineData(' ')]
    public void FromHexChar_InvalidChar_ThrowsException(char ch)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ConvertHelper.FromHexChar(ch));
    }

    #endregion

    #region FromHexString 测试

    [Fact]
    public void FromHexString_EmptyString_ReturnsEmptyArray()
    {
        var result = ConvertHelper.FromHexString(ReadOnlySpan<char>.Empty);
        Assert.Empty(result);
    }

    [Fact]
    public void FromHexString_EvenLength_ConvertsCorrectly()
    {
        var result = ConvertHelper.FromHexString("ABCDEF");
        Assert.Equal(new byte[] { 0xAB, 0xCD, 0xEF }, result);
    }

    [Fact]
    public void FromHexString_OddLength_PadsZeroAndConvertsCorrectly()
    {
        // "ABC" -> "0ABC" -> [0x0A, 0xBC]
        var result = ConvertHelper.FromHexString("ABC");
        Assert.Equal(new byte[] { 0x0A, 0xBC }, result);
    }

    [Fact]
    public void FromHexString_LowerCaseInput_ConvertsCorrectly()
    {
        var result = ConvertHelper.FromHexString("abcdef");
        Assert.Equal(new byte[] { 0xAB, 0xCD, 0xEF }, result);
    }

    [Fact]
    public void FromHexString_WriteToSpan_ReturnsCorrectLength()
    {
        var destination = new byte[3];
        var written = ConvertHelper.FromHexString("ABCDEF", destination);
        Assert.Equal(3, written);
        Assert.Equal(new byte[] { 0xAB, 0xCD, 0xEF }, destination);
    }

    [Fact]
    public void FromHexString_InsufficientSpan_ThrowsException()
    {
        var destination = new byte[1];
        Assert.Throws<ArgumentException>(() => ConvertHelper.FromHexString("ABCDEF", destination));
    }

    [Fact]
    public void ToHexString_FromHexString_RoundTripConsistent()
    {
        byte[] original = [0x00, 0xFF, 0x12, 0x34, 0xAB];
        var hex = ConvertHelper.ToHexString(original, lowerCase: true);
        var result = ConvertHelper.FromHexString(hex);
        Assert.Equal(original, result);
    }

    #endregion
}
