using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Ultra.Common.Compares;
using UltraTool.Compares;

namespace UltraTool.Times.Ranges;

/// <summary>
/// 偏移日期时间范围
/// </summary>
[PublicAPI]
public readonly struct DateTimeOffsetRange : IComparableRange<DateTimeOffset>
{
    /// <summary>
    /// 日期时间起始值
    /// </summary>
    public DateTimeOffset Start { get; }

    /// <summary>
    /// 日期时间结束值
    /// </summary>
    public DateTimeOffset End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="range">日期时间范围</param>
    public DateTimeOffsetRange(in DateTimeRange range)
    {
        Start = range.Start;
        End = range.End;
        Mode = range.Mode;
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">日期时间起始值</param>
    /// <param name="end">日期时间结束值</param>
    /// <param name="mode">区间类型，默认闭区间</param>
    public DateTimeOffsetRange(in DateTimeOffset start, in DateTimeOffset end, RangeMode mode = RangeMode.Close)
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
    /// <param name="start">日期时间起始值</param>
    /// <param name="end">日期时间结束值</param>
    /// <param name="mode">区间类型</param>
    public void Deconstruct(out DateTimeOffset start, out DateTimeOffset end, out RangeMode mode)
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
    public bool IsIn(in DateTimeOffset value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在日期时间范围外
    /// </summary>
    /// <param name="value">日期时间值</param>
    /// <returns>是否在日期时间范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in DateTimeOffset value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 日期时间范围隐式转换为偏移日期时间范围
    /// </summary>
    /// <param name="range">日期时间范围</param>
    /// <returns>偏移日期时间范围</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator DateTimeOffsetRange(DateTimeRange range) => new(range);
}