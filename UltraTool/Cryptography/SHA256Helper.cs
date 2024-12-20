using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Helpers;

namespace UltraTool.Cryptography;

/// <summary>
/// SHA256帮助类
/// </summary>
[PublicAPI]
public static class SHA256Helper
{
    /// <summary>SHA256数据字节数量</summary>
    private const int SHA256ByteCount = 32;

    /// <summary>
    /// 输入字节数据，进行SHA256计算，返回计算结果
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <returns>计算结果</returns>
    public static byte[] Compute(ReadOnlySpan<byte> source)
    {
        var destination = ArrayHelper.AllocateUninitializedArray<byte>(SHA256ByteCount);
        Compute(source, destination);
        return destination;
    }

    /// <summary>
    /// 输入字节数据，进行SHA256计算，计算结果输出至指定跨度
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="destination">输出跨度</param>
    /// <returns>输出长度</returns>
    public static int Compute(ReadOnlySpan<byte> source, Span<byte> destination)
    {
#if NET5_0_OR_GREATER
        return SHA256.HashData(source, destination);
#else
        if (destination.Length < SHA256ByteCount)
        {
            throw new ArgumentException("destination span too small");
        }

        using var sha256 = SHA256.Create();
        sha256.TryComputeHash(source, destination, out var written);
        return written;
#endif
    }

    /// <summary>
    /// 输入字符串，对其字节数据进行SHA256计算，返回计算结果
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <param name="encoding">源字符串编码，输入null时使用<see cref="Encoding.UTF8"/></param>
    /// <returns>计算结果</returns>
    public static byte[] Compute(ReadOnlySpan<char> source, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        using var bytes = PooledArray.Get<byte>(encoding.GetByteCount(source), true);
        encoding.GetBytes(source, bytes.Span);
        return Compute(bytes.ReadOnlySpan);
    }

    /// <summary>
    /// 输入字符串，对其字节数据进行SHA256计算，计算结果输出至指定跨度
    /// </summary>
    /// <param name="source">源字符串</param>
    /// <param name="destination">输出跨度</param>
    /// <param name="encoding">源字符串编码，输入null时使用<see cref="Encoding.UTF8"/></param>
    /// <returns>输出长度</returns>
    public static int Compute(ReadOnlySpan<char> source, Span<byte> destination, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        using var bytes = PooledArray.Get<byte>(encoding.GetByteCount(source), true);
        encoding.GetBytes(source, bytes.Span);
        return Compute(bytes.ReadOnlySpan, destination);
    }

    /// <summary>
    /// 输入字节数据，进行SHA256计算，计算结果输出为十六进制字符串
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <returns>SHA256字符串</returns>
    public static string ComputeAsString(ReadOnlySpan<byte> source, bool lowerCase = false)
    {
        Span<byte> destination = stackalloc byte[SHA256ByteCount];
        Compute(source, destination);
        return ConvertHelper.ToHexString(destination, lowerCase);
    }

    /// <summary>
    /// 输入字符串，对其字节数据进行SHA256计算，计算结果输出为十六进制字符串
    /// </summary>
    /// <param name="source">源字节数据</param>
    /// <param name="lowerCase">是否小写，默认为false</param>
    /// <param name="encoding">源字符串编码，输入null时使用<see cref="Encoding.UTF8"/></param>
    /// <returns>SHA256字符串</returns>
    public static string ComputeAsString(ReadOnlySpan<char> source, bool lowerCase = false, Encoding? encoding = null)
    {
        Span<byte> destination = stackalloc byte[SHA256ByteCount];
        Compute(source, destination, encoding);
        return ConvertHelper.ToHexString(destination, lowerCase);
    }
}