using System.Runtime.CompilerServices;
using UltraTool.Helpers;
using UltraTool.Randoms;

namespace UltraTool.Collections;

/// <summary>
/// 跨度拓展类
/// </summary>
public static class SpanExtensions
{
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
    /// 将跨度中元素顺序随机打乱
    /// </summary>
    /// <param name="span">跨度</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Shuffle<T>(this Span<T> span) => RandomHelper.Shared.Shuffle(span);
}