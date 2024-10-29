#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 时间拓展类
/// </summary>
[PublicAPI]
public static class TimeOnlyExtensions
{
    /// <summary>
    /// 获取日期是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>周几</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TodayOfWeek(this DateOnly date) => date.DayOfWeek.TodayOfWeek();

    /// <summary>
    /// 判断时间是否为上午
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>是否为上午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAm(this TimeOnly time) => DateTimeHelper.IsAm(time.Hour);

    /// <summary>
    /// 判断时间是否为下午
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>是否为下午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPm(this TimeOnly time) => DateTimeHelper.IsPm(time.Hour);

    /// <summary>
    /// 时间转为时间跨度
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>时间跨度</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan AsTimeSpan(this TimeOnly time) => new(time.Ticks);
}
#endif