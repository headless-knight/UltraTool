using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace UltraTool.Text;

/// <summary>
/// 字符串拓展类
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <summary>
    /// 若为null则返回空字符串，否则返回原字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>原字符串或空字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EmptyIfNull(this string? str) => str ?? string.Empty;

    /// <summary>
    /// 判断字符串是否为空字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否空字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this string str) => str.Length == 0;

    /// <summary>
    /// 判断字符串是否非字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否非空字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty(this string str) => str.Length != 0;

    /// <summary>
    /// 判断字符串是否全为空白符
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否全为空白符</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWhiteSpace(this string str) => str.All(char.IsWhiteSpace);

    /// <summary>
    /// 判断字符串是否非全空白符
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否非全空白符</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotWhiteSpace(this string str) => !str.All(char.IsWhiteSpace);

    /// <summary>
    /// 判断字符串是否为null或空串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否为null或空串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// 判断字符串是否不为null或空串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否不为null或空串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? str) => !string.IsNullOrEmpty(str);

    /// <summary>
    /// 判断字符串是否为null或空白符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否为null或空白符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str) => string.IsNullOrWhiteSpace(str);

    /// <summary>
    /// 判断字符串是否不为null或空白符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>是否不为null或空白符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? str) => !string.IsNullOrWhiteSpace(str);

    /// <summary>
    /// 将字符串转为字节数组
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>字节数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(this string str) => str.GetBytes(Encoding.UTF8);

    /// <summary>
    /// 将字符串转为字节数据并写入字节跨度
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="bytes">输出字节跨度</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetBytes(this string str, Span<byte> bytes) => str.GetBytes(Encoding.UTF8, bytes);

    /// <summary>
    /// 将字符串转为字节数组
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码</param>
    /// <returns>字节数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(this string str, Encoding encoding) => encoding.GetBytes(str);

    /// <summary>
    /// 将字符串写入字节跨度
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码</param>
    /// <param name="bytes">输出字节跨度</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetBytes(this string str, Encoding encoding, Span<byte> bytes) =>
        encoding.GetBytes(str, bytes);

    /// <summary>
    /// 将字符串转为池化字节数组
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="encoding">编码，默认为UTF8</param>
    /// <returns>池化字节数组</returns>
    [Pure, MustDisposeResource]
    public static PooledArray<byte> GetBytesPooled(this string str, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var bytes = PooledArray.Get<byte>(encoding.GetByteCount(str));
        encoding.GetBytes(str, bytes.Span);
        return bytes;
    }
}