using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 比较范围
/// </summary>
[PublicAPI]
public readonly struct ComparableRange<T> : IComparableRange<T> where T : IComparable<T>
{
    /// <summary>
    /// 范围起始值
    /// </summary>
    public T Start { get; }

    /// <summary>
    /// 范围结束值
    /// </summary>
    public T End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">范围起始值</param>
    /// <param name="end">范围结束值</param>
    /// <param name="mode">区间类型</param>
    public ComparableRange(T start, T end, RangeMode mode = RangeMode.Close)
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        Start = start;
        End = end;
        Mode = mode;
    }

    /// <summary>
    /// 判断传入值是否在比较范围内
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>是否在比较范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(in T value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在比较范围外
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>是否在比较范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in T value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}