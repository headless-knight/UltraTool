using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 偏移日期时间拓展类
/// </summary>
[PublicAPI]
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// 判断日期时间是否为周末
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>是否为周末</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWeekEnd(this DateTimeOffset offset) =>
        offset.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// 判断日期时间是否在闰年
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>是否在闰年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(this DateTimeOffset offset) => DateTimeHelper.IsLeapYear(offset.Year);

    /// <summary>
    /// 判断日期时间是否在闰月
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>是否在闰月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapMonth(this DateTimeOffset offset) => DateTimeHelper.IsLeapMonth(offset.Year, offset.Month);

    /// <summary>
    /// 判断日期时间是否为上午
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>是否为上午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAm(this DateTimeOffset offset) => DateTimeHelper.IsAm(offset.Hour);

    /// <summary>
    /// 判断日期时间是否为下午
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>是否为下午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPm(this DateTimeOffset offset) => DateTimeHelper.IsPm(offset.Hour);

    /// <summary>
    /// 判断日期时间与指定值是否在同一天
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(this DateTimeOffset offset, DateTimeOffset other) =>
        offset.Year == other.Year && offset.Month == other.Month && offset.Day == other.Day;

    /// <summary>
    /// 判断日期时间与指定值是否在同一月
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(this DateTimeOffset offset, DateTimeOffset other) =>
        offset.Year == other.Year && offset.Month == other.Month;

    /// <summary>
    /// 判断日期时间与指定值是否在同一年
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameYear(this DateTimeOffset offset, DateTimeOffset other) => offset.Year == other.Year;

    /// <summary>
    /// 获取日期时间的年月
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>年月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YearMonth GetYearMonth(this DateTimeOffset offset) => new(offset.Year, offset.Month);

    /// <summary>
    /// 获取日期时间所在当年第几周
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>第几周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int WeekOfYear(this DateTimeOffset offset) =>
        DateTimeHelper.GetWeekOfYear(offset.Year, offset.Month, offset.Day);

    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>周几</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TodayOfWeek(this DateTimeOffset offset) => offset.DayOfWeek.TodayOfWeek();

    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    [Pure]
    public static int TodayOfWeek(this DateTimeOffset offset, TimeSpan criticalValue)
    {
        var dayOfWeek = offset.DayOfWeek.TodayOfWeek();
        if (offset.TimeOfDay >= criticalValue) return dayOfWeek;

        dayOfWeek--;
        return dayOfWeek == 0 ? 7 : dayOfWeek;
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    [Pure]
    public static int TodayOfWeek(this DateTimeOffset offset, TimeOnly criticalValue)
    {
        var dayOfWeek = offset.DayOfWeek.TodayOfWeek();
        if (offset.TimeOfDay >= criticalValue.AsTimeSpan()) return dayOfWeek;

        dayOfWeek--;
        return dayOfWeek == 0 ? 7 : dayOfWeek;
    }

    /// <summary>
    /// 获取日期时间的日期
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>日期</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetDateOnly(this DateTimeOffset offset) =>
        new(offset.Year, offset.Month, offset.Day);

    /// <summary>
    /// 获取日期时间的时间
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>日期</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly GetTimeOnly(this DateTimeOffset offset) =>
        new(offset.Hour, offset.Minute, offset.Second, offset.Millisecond);
#endif
}