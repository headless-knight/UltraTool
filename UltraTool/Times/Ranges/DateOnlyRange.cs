#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;

namespace UltraTool.Times.Ranges;

/// <summary>
/// 日期范围
/// </summary>
[PublicAPI]
public readonly struct DateOnlyRange : IComparableRange<DateOnly>
{
    /// <summary>
    /// 日期起始值
    /// </summary>
    public DateOnly Start { get; }

    /// <summary>
    /// 日期结束值
    /// </summary>
    public DateOnly End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间类型，默认闭区间</param>
    public DateOnlyRange(in DateOnly start, in DateOnly end, RangeMode mode = RangeMode.Close)
    {
        if (start > end)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        Start = start;
        End = end;
        Mode = mode;
    }

    /// <summary>
    /// 解构方法
    /// </summary>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间类型</param>
    public void Deconstruct(out DateOnly start, out DateOnly end, out RangeMode mode)
    {
        start = Start;
        end = End;
        mode = Mode;
    }

    /// <summary>
    /// 判断传入值是否在日期范围内
    /// </summary>
    /// <param name="value">日期值</param>
    /// <returns>是否在日期范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(in DateOnly value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在日期范围外
    /// </summary>
    /// <param name="value">日期值</param>
    /// <returns>是否在日期范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in DateOnly value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}
#endif