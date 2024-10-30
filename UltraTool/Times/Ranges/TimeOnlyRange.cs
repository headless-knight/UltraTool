#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Ultra.Common.Compares;
using UltraTool.Compares;

namespace UltraTool.Times.Ranges;

/// <summary>
/// 时间范围
/// </summary>
[PublicAPI]
public readonly struct TimeOnlyRange : IComparableRange<TimeOnly>
{
    /// <summary>
    /// 时间起始值
    /// </summary>
    public TimeOnly Start { get; }

    /// <summary>
    /// 时间结束值
    /// </summary>
    public TimeOnly End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="start">时间起始值</param>
    /// <param name="end">时间结束值</param>
    /// <param name="mode">区间类型，默认闭区间</param>
    public TimeOnlyRange(in TimeOnly start, in TimeOnly end, RangeMode mode = RangeMode.Close)
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
    /// <param name="start">时间起始值</param>
    /// <param name="end">时间结束值</param>
    /// <param name="mode">区间类型</param>
    public void Deconstruct(out TimeOnly start, out TimeOnly end, out RangeMode mode)
    {
        start = Start;
        end = End;
        mode = Mode;
    }

    /// <summary>
    /// 判断传入值是否在时间范围内
    /// </summary>
    /// <param name="value">时间值</param>
    /// <returns>是否在时间范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(in TimeOnly value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断传入值是否在时间范围外
    /// </summary>
    /// <param name="value">时间值</param>
    /// <returns>是否在时间范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(in TimeOnly value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}
#endif