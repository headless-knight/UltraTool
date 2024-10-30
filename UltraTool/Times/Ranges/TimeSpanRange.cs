using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Ultra.Common.Compares;
using UltraTool.Compares;

namespace UltraTool.Times.Ranges;

/// <summary>
/// 时间跨度范围
/// </summary>
[PublicAPI]
public readonly struct TimeSpanRange : IComparableRange<TimeSpan>
{
    /// <summary>
    /// 时间跨度起始值
    /// </summary>
    public TimeSpan Start { get; }

    /// <summary>
    /// 时间跨度结束值
    /// </summary>
    public TimeSpan End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">时间跨度起始值</param>
    /// <param name="end">时间跨度结束值</param>
    /// <param name="mode">区间类型，默认闭区间</param>
    public TimeSpanRange(in TimeSpan start, in TimeSpan end, RangeMode mode = RangeMode.Close)
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
    /// <param name="start">时间跨度起始值</param>
    /// <param name="end">时间跨度结束值</param>
    /// <param name="mode">区间类型</param>
    public void Deconstruct(out TimeSpan start, out TimeSpan end, out RangeMode mode)
    {
        start = Start;
        end = End;
        mode = Mode;
    }

    /// <summary>
    /// 判断传入值是否在时间跨度范围内
    /// </summary>
    /// <param name="value">时间跨度值</param>
    /// <returns>是否在时间跨度范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(in TimeSpan value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在时间跨度范围外
    /// </summary>
    /// <param name="value">时间跨度值</param>
    /// <returns>是否在时间跨度范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in TimeSpan value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}