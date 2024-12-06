using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 比较帮助类
/// </summary>
[PublicAPI]
public static class CompareHelper
{
    /// <summary>
    /// 判断值是否小于目标值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <returns>是否小于目标值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThan<T>(in T value, in T other) where T : IComparable<T> =>
        value.CompareTo(other) < 0;

    /// <summary>
    /// 判断值是否小于目标值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <param name="comparer">比较器</param>
    /// <returns>是否小于目标值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThan<T>(in T value, in T other, IComparer<T> comparer) =>
        comparer.Compare(value, other) < 0;

    /// <summary>
    /// 判断值是否小于等于目标值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <returns>是否小于等于目标值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessThanOrEquals<T>(in T value, in T other) where T : IComparable<T> =>
        value.CompareTo(other) <= 0;

    /// <summary>
    /// 判断值是否大于目标值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <returns>是否大于目标值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThan<T>(in T value, in T other) where T : IComparable<T> =>
        value.CompareTo(other) > 0;

    /// <summary>
    /// 判断值是否大于等于目标值
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <returns>是否大于等于目标值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterThanOrEquals<T>(in T value, in T other) where T : IComparable<T> =>
        value.CompareTo(other) >= 0;

    /// <summary>
    /// 判断指定值是否在区间内
    /// </summary>
    /// <param name="value">比较值</param>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间模式，默认闭区间</param>
    /// <returns>是否在区间内</returns>
    [Pure]
    public static bool InRange<T>(in T value, in T start, in T end, RangeMode mode = RangeMode.Close)
        where T : IComparable<T>
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        return InRangeInternal(value, start, end, mode);
    }

    /// <summary>判断指定值是否在区间内，内部实现</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool InRangeInternal<T>(in T value, in T start, in T end, RangeMode mode)
        where T : IComparable<T> => mode switch
    {
        RangeMode.Open => IsLessThan(start, value) && IsGreaterThan(end, value),
        RangeMode.Close => IsLessThanOrEquals(start, value) && IsGreaterThanOrEquals(end, value),
        RangeMode.OpenClose => IsLessThan(start, value) && IsGreaterThanOrEquals(end, value),
        RangeMode.CloseOpen => IsLessThanOrEquals(start, value) && IsGreaterThan(end, value),
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported range mode")
    };

    /// <summary>
    /// 判断指定值是否在区间外
    /// </summary>
    /// <param name="value">比较值</param>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间模式，默认闭区间</param>
    /// <returns>是否在区间外</returns>
    [Pure]
    public static bool OutRange<T>(in T value, in T start, in T end, in RangeMode mode = RangeMode.Close)
        where T : IComparable<T>
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        return OutRangeInternal(value, start, end, mode);
    }

    /// <summary>判断指定值是否在区间外，内部实现</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool OutRangeInternal<T>(in T value, in T start, in T end, in RangeMode mode)
        where T : IComparable<T> => mode switch
    {
        RangeMode.Open => start.CompareTo(value) >= 0 || end.CompareTo(value) <= 0,
        RangeMode.Close => start.CompareTo(value) < 0 || end.CompareTo(value) > 0,
        RangeMode.OpenClose => start.CompareTo(value) <= 0 || end.CompareTo(value) > 0,
        RangeMode.CloseOpen => start.CompareTo(value) < 0 || end.CompareTo(value) >= 0,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported range mode")
    };
}