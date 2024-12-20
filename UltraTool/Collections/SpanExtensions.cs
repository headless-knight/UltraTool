using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using UltraTool.Helpers;

namespace UltraTool.Collections;

/// <summary>
/// 跨度拓展类
/// </summary>
[PublicAPI]
public static class SpanExtensions
{
    /// <summary>
    /// 反转跨度至指定跨度
    /// </summary>
    /// <param name="span">源跨度</param>
    /// <param name="destination">目标跨度</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseTo<T>(this Span<T> span, Span<T> destination) =>
        ReverseTo((ReadOnlySpan<T>)span, destination);

    /// <summary>
    /// 反转跨度至指定跨度
    /// </summary>
    /// <param name="source">源跨度</param>
    /// <param name="destination">目标跨度</param>
    public static void ReverseTo<T>(this ReadOnlySpan<T> source, Span<T> destination)
    {
        ArgumentOutOfRangeHelper.ThrowIfLessThan(destination.Length, source.Length);
        for (var i = 0; i < source.Length; i++)
        {
            destination[i] = source[source.Length - 1 - i];
        }
    }

    /// <summary>
    /// 将跨度内容输出为字符串
    /// </summary>
    /// <param name="source">跨度</param>
    /// <returns>字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DumpAsString<T>(this Span<T> source) => DumpAsString((ReadOnlySpan<T>)source);

    /// <summary>
    /// 将跨度内容输出为字符串
    /// </summary>
    /// <param name="source">跨度</param>
    /// <returns>字符串</returns>
    [Pure]
    public static string DumpAsString<T>(this ReadOnlySpan<T> source)
    {
        var sb = new StringBuilder(source.Length + 4);
        sb.Append("{ ");
        foreach (var item in source)
        {
            sb.Append(item);
            sb.Append(", ");
        }

        if (source.Length > 0)
        {
            sb.Length -= 2;
        }

        sb.Append(" }");
        return sb.ToString();
    }
}