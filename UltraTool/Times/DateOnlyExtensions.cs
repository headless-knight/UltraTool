#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 日期拓展类
/// </summary>
[PublicAPI]
public static class DateOnlyExtensions
{
    /// <summary>
    /// 判断日期是否为周末
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>是否为周末</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWeekEnd(this DateOnly date) => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// 判断日期是否在闰年
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>是否在闰年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(this DateOnly date) => DateTimeHelper.IsLeapYear(date.Year);

    /// <summary>
    /// 判断日期是否在闰月
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>是否在闰月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapMonth(this DateOnly date) => DateTimeHelper.IsLeapMonth(date.Year, date.Month);

    /// <summary>
    /// 判断日期与指定值是否在同一天
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(this DateOnly date, DateOnly other) =>
        date.Year == other.Year && date.Month == other.Month && date.Day == other.Day;

    /// <summary>
    /// 判断日期与指定值是否在同一月
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(this DateOnly date, DateOnly other) =>
        date.Year == other.Year && date.Month == other.Month;

    /// <summary>
    /// 判断日期与指定值是否在同一年
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameYear(this DateOnly date, DateOnly other) => date.Year == other.Year;

    /// <summary>
    /// 获取日期所在当年第几周
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>第几周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int WeekOfYear(this DateOnly date) => DateTimeHelper.GetWeekOfYear(date.Year, date.Month, date.Day);

    /// <summary>
    /// 将日期转为日期时间
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>日期时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime AsDateTime(this DateOnly date) => new(date.Year, date.Month, date.Day);
}
#endif