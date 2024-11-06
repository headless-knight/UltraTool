using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Ultra.Common.Compares;
using UltraTool.Helpers;

namespace UltraTool.Compares;

/// <summary>
/// 比较帮助类
/// </summary>
[PublicAPI]
public static class CompareHelper
{
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
        RangeMode.Open => start.CompareTo(value) < 0 && end.CompareTo(value) > 0,
        RangeMode.Close => start.CompareTo(value) <= 0 && end.CompareTo(value) >= 0,
        RangeMode.OpenClose => start.CompareTo(value) < 0 && end.CompareTo(value) >= 0,
        RangeMode.CloseOpen => start.CompareTo(value) <= 0 && end.CompareTo(value) > 0,
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

    /// <summary>
    /// 返回两个值中的最小值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最小值</returns>
    /// <remarks>此方法迁移至MathHelper，请使用MathHelper.Min代替</remarks>
    [Obsolete("Use MathHelper.Min instead")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Min<T>(in T value1, in T value2) where T : IComparable<T> => MathHelper.Min(value1, value2);

    /// <summary>
    /// 返回两个值中的最大值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最大值</returns>
    /// <remarks>此方法迁移至MathHelper，请使用MathHelper.Max代替</remarks>
    [Obsolete("Use MathHelper.Max instead")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Max<T>(in T value1, in T value2) where T : IComparable<T> => MathHelper.Max(value1, value2);

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <returns>中间值</returns>
    /// <remarks>此方法迁移至MathHelper，请使用MathHelper.Middle代替</remarks>
    [Obsolete("Use MathHelper.Middle instead")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Middle<T>(in T value1, in T value2, in T value3) where T : IComparable<T> =>
        MathHelper.Middle(value1, value2, value3);

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <param name="comparer">比较器</param>
    /// <returns>中间值</returns>
    /// <remarks>此方法迁移至MathHelper，请使用MathHelper.Middle代替</remarks>
    [Obsolete("Use MathHelper.Middle instead")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Middle<T>(in T value1, in T value2, in T value3, IComparer<T> comparer) =>
        MathHelper.Middle(value1, value2, value3, comparer);
}