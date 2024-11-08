using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;

namespace UltraTool.Times.Ranges;

/// <summary>
/// 日期时间范围
/// </summary>
[PublicAPI]
public readonly struct DateTimeRange : IComparableRange<DateTime>
{
    /// <summary>
    /// 日期时间起始值
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// 日期时间结束值
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">日期时间起始值</param>
    /// <param name="end">日期时间结束值</param>
    /// <param name="mode">区间类型，默认闭区间</param>
    public DateTimeRange(in DateTime start, in DateTime end, RangeMode mode = RangeMode.Close)
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
    /// 构造方法
    /// </summary>
    /// <param name="start">日期时间起始值</param>
    /// <param name="end">日期时间结束值</param>
    /// <param name="mode">区间类型</param>
    public void Deconstruct(out DateTime start, out DateTime end, out RangeMode mode)
    {
        start = Start;
        end = End;
        mode = Mode;
    }

    /// <summary>
    /// 判断传入值是否在日期时间范围内
    /// </summary>
    /// <param name="value">日期时间值</param>
    /// <returns>是否在日期时间范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(in DateTime value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在日期时间范围外
    /// </summary>
    /// <param name="value">日期时间值</param>
    /// <returns>是否在日期时间范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in DateTime value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}