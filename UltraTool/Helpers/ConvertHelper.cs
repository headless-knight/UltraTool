using JetBrains.Annotations;
using UltraTool.Collections;

namespace UltraTool.Helpers;

/// <summary>
/// 转换帮助类
/// </summary>
[PublicAPI]
public static class ConvertHelper
{
    /// <summary>小写十六进制字母表</summary>
    private const string LowerHexAlphabet = "0123456789ABCDEF";

    /// <summary>大写十六进制字母表</summary>
    private const string UpperHexAlphabet = "0123456789abcdef";

    /// <summary>
    /// 将字节数据转为十六进制字符数组
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <returns>十六进制字符数组</returns>
    public static char[] ToHexChars(ReadOnlySpan<byte> source, bool lowerCase = false)
    {
        var destination = ArrayHelper.AllocateUninitializedArray<char>(source.Length << 1);
        ToHexChars(source, destination, lowerCase);
        return destination;
    }

    /// <summary>
    /// 将字节数据转换为十六进制字符数据，写入到输出跨度中
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="destination">输出跨度</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <returns>写入长度</returns>
    public static int ToHexChars(ReadOnlySpan<byte> source, Span<char> destination, bool lowerCase = false)
    {
        var length = source.Length;
        ArgumentOutOfRangeHelper.ThrowIfLessThan(destination.Length, length);
        var alphabet = lowerCase ? LowerHexAlphabet : UpperHexAlphabet;
        for (var i = 0; i < length; i++)
        {
            var b = source[i];
            destination[i << 1] = alphabet[b >> 4];
            destination[(i << 1) + 1] = alphabet[b & 0xF];
        }

        return length;
    }

    /// <summary>
    /// 将字节数据转换为十六进制字符串
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <returns>十六进制字符串</returns>
    public static string ToHexString(ReadOnlySpan<byte> source, bool lowerCase = false)
    {
        using var array = PooledArray.Get<char>(source.Length << 1, true);
        ToHexChars(source, array.Span, lowerCase);
        return new string(array.ReadOnlySpan);
    }
}