using System.Buffers;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Numerics;

namespace UltraTool.Helpers;

/// <summary>
/// 转换帮助类
/// </summary>
[PublicAPI]
public static class ConvertHelper
{
    #region 十六进制

    /// <summary>小写十六进制字母表</summary>
    private const string LowerHexAlphabet = "0123456789abcdef";

    /// <summary>大写十六进制字母表</summary>
    private const string UpperHexAlphabet = "0123456789ABCDEF";

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
            var value = source[i];
            destination[i << 1] = alphabet[value >> 4];
            destination[(i << 1) + 1] = alphabet[value & 0xF];
        }

        return length;
    }

    /// <summary>
    /// 将字节数据转换为十六进制字符串
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <returns>十六进制字符串</returns>
    [Pure]
    public static string ToHexString(ReadOnlySpan<byte> source, bool lowerCase = false)
    {
        using var array = PooledArray.Get<char>(source.Length << 1, true);
        ToHexChars(source, array.Span, lowerCase);
        return new string(array.ReadOnlySpan);
    }

    /// <summary>
    /// 将十六进制字符转换为字节值
    /// </summary>
    /// <param name="ch">十六进制字符</param>
    /// <returns>字节值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte FromHexChar(char ch) => ch switch
    {
        >= '0' and <= '9' => (byte)(ch - '0'),
        >= 'a' and <= 'f' => (byte)(ch - 'a' + 10),
        >= 'A' and <= 'F' => (byte)(ch - 'A' + 10),
        _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, "Invalid hex character")
    };

    /// <summary>
    /// 将十六进制字符串转换为字节数据
    /// </summary>
    /// <param name="source">源十六进制字符串</param>
    /// <returns>字节数据</returns>
    [Pure]
    public static byte[] FromHexString(ReadOnlySpan<char> source)
    {
        // 字符串为空
        if (source.Length <= 0) return [];

        char[]? rent = null;
        try
        {
            // 输入字符串长度为奇数
            if (source.Length.IsOdd())
            {
                var length = source.Length + 1;
                // 分配原始长度+1的数组
                rent = ArrayPool<char>.Shared.Rent(length);
                // 在前面添加一个0
                rent[0] = '0';
                // 复制数据
                source.CopyTo(rent.AsSpan(1, source.Length));
                source = rent.AsReadOnlySpan(0, length);
            }

            var result = ArrayHelper.AllocateUninitializedArray<byte>(source.Length >> 1);
            FromHexStringInternal(source, result);
            return result;
        }
        finally
        {
            if (rent != null) ArrayPool<char>.Shared.Return(rent, true);
        }
    }

    /// <summary>
    /// 将十六进制字符串转换为字节数据，写入到输出跨度中
    /// </summary>
    /// <param name="source">源十六进制字符串</param>
    /// <param name="destination">输出跨度</param>
    /// <returns>写入长度</returns>
    public static int FromHexString(ReadOnlySpan<char> source, Span<byte> destination)
    {
        // 字符串为空
        if (source.Length <= 0) return 0;

        char[]? rent = null;
        try
        {
            // 输入字符串长度为奇数
            if (source.Length.IsOdd())
            {
                var length = source.Length + 1;
                // 分配原始长度+1的数组
                rent = ArrayPool<char>.Shared.Rent(length);
                // 在前面添加一个0
                rent[0] = '0';
                // 复制数据
                source.CopyTo(rent.AsSpan(1, source.Length));
                source = rent.AsReadOnlySpan(0, length);
            }

            // 输出跨度长度不足
            if (destination.Length < (source.Length >> 1))
            {
                throw new ArgumentException("destination span too small", nameof(destination));
            }

            return FromHexStringInternal(source, destination);
        }
        finally
        {
            if (rent != null) ArrayPool<char>.Shared.Return(rent, true);
        }
    }

    /// <summary>将十六进制字符串转为字节数据，内部实现</summary>
    private static int FromHexStringInternal(ReadOnlySpan<char> source, Span<byte> destination)
    {
        for (int i = 0, j = 0; j < source.Length; i++)
        {
            var f = FromHexChar(source[j++]) << 4;
            f |= FromHexChar(source[j++]);
            destination[i] = (byte)(f & 0xFF);
        }

        return source.Length >> 1;
    }

    #endregion

    #region 六十二进制

    /// <summary>Base62源基数</summary>
    private const int Base62SourceBase = 256;

    /// <summary>Base62目标基数</summary>
    private const int Base62TargetBase = 62;

    /// <summary>GMP风格62进制字母表</summary>
    private const string GmpBase62Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>字母倒置的GMP风格62进制字母表</summary>
    private const string InvertedBase62Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 将字节数据转换为六十二进制字符数组
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="inverted">是否为倒置GMP风格，默认为false</param>
    /// <returns>六十二进制字符数组</returns>
    public static char[] ToBase62Chars(ReadOnlySpan<byte> source, bool inverted = false)
    {
        if (source is not { Length: > 0 }) return [];

        using var dynamicArray = ToBase62CharsInternal(source, inverted);
        var chars = ArrayHelper.AllocateUninitializedArray<char>(dynamicArray.Length);
        Array.Copy(dynamicArray.GetRawArray(), chars, dynamicArray.Length);
        return chars;
    }

    /// <summary>
    /// 将字节数据转换为六十二进制字符串
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="inverted">是否为倒置GMP风格，默认为false</param>
    /// <returns>六十二进制字符串</returns>
    public static string ToBase62String(ReadOnlySpan<byte> source, bool inverted = false)
    {
        if (source is not { Length: > 0 }) return string.Empty;

        using var dynamicArray = ToBase62CharsInternal(source, inverted);
        return new string(dynamicArray.GetRawArray(), 0, dynamicArray.Length);
    }

    /// <summary>将字节数据转换为六十二进制字符数组，内部实现</summary>
    [MustDisposeResource]
    private static PooledDynamicArray<char> ToBase62CharsInternal(ReadOnlySpan<byte> source, bool inverted)
    {
        var alphabet = inverted ? InvertedBase62Alphabet : GmpBase62Alphabet;
        var estimatedLength = EstimateOutputLength(source.Length, Base62SourceBase, Base62TargetBase);
        var output = new PooledDynamicArray<char>(estimatedLength, true);
        var items = new PooledDynamicArray<byte>(source.Length, true);
        try
        {
            source.CopyTo(items.GetRawArray());
            while (items is { Length: > 0 })
            {
                var quotient = new PooledDynamicArray<byte>(items.Length, true);
                var remainder = 0;
                foreach (var item in items)
                {
                    var accumulator = (item & 0xFF) + remainder * Base62SourceBase;
                    var digit = (accumulator - (accumulator % Base62TargetBase)) / Base62TargetBase;
                    remainder = accumulator % Base62TargetBase;
                    if (quotient.Length > 0 || digit > 0)
                    {
                        quotient.Add((byte)digit);
                    }
                }

                output.Add(alphabet[remainder]);
                items.Dispose();
                items = quotient;
            }
        }
        finally
        {
            items.Dispose();
        }

        for (var i = 0; i < source.Length - 1 && source[i] == 0; i++)
        {
            output.Add(alphabet[0]);
        }

        Array.Reverse(output.GetRawArray(), 0, output.Length);
        return output;
    }

    #endregion

    /// <summary>估算转换后输出长度</summary>
    private static int EstimateOutputLength(int inputLength, int sourceBase, int targetBase) =>
        (int)Math.Ceiling((Math.Log(sourceBase) / Math.Log(targetBase)) * inputLength);
}