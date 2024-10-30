using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 日期时间拓展类
/// </summary>
[PublicAPI]
public static class DateTimeExtensions
{
    /// <summary>
    /// 判断日期时间是否为周末
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>是否为周末</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWeekEnd(this DateTime dt) => dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// 判断日期时间是否在闰年
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>是否在闰年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(this DateTime dt) => DateTimeHelper.IsLeapYear(dt.Year);

    /// <summary>
    /// 判断日期时间是否在闰月
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>是否在闰月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapMonth(this DateTime dt) => DateTimeHelper.IsLeapMonth(dt.Year, dt.Month);

    /// <summary>
    /// 判断日期时间是否为上午
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>是否为上午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAm(this DateTime dt) => DateTimeHelper.IsAm(dt.Hour);

    /// <summary>
    /// 判断日期时间是否为下午
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>是否为下午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPm(this DateTime dt) => DateTimeHelper.IsPm(dt.Hour);

    /// <summary>
    /// 判断日期时间与指定值是否在同一天
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(this DateTime dt, DateTime other) =>
        dt.Year == other.Year && dt.Month == other.Month && dt.Day == other.Day;

    /// <summary>
    /// 判断日期时间与指定值是否在同一月
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(this DateTime dt, DateTime other) =>
        dt.Year == other.Year && dt.Month == other.Month;

    /// <summary>
    /// 判断日期时间与指定值是否在同一年
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="other">指定值</param>
    /// <returns>是否在同一年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameYear(this DateTime dt, DateTime other) => dt.Year == other.Year;

    /// <summary>
    /// 将日期时间转化为秒级Unix时间戳
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>秒级Unix时间戳</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixTimeSeconds(this DateTime dt) =>
        new DateTimeOffset(dt).ToUnixTimeSeconds();

    /// <summary>
    /// 将日期时间转化为秒级Unix时间戳
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="offset">时区偏移量</param>
    /// <returns>秒级Unix时间戳</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixTimeSeconds(this DateTime dt, TimeSpan offset) =>
        new DateTimeOffset(dt, offset).ToUnixTimeSeconds();

    /// <summary>
    /// 将日期时间转化为毫秒级Unix时间戳
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>毫秒级Unix时间戳</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixTimeMilliseconds(this DateTime dt) =>
        new DateTimeOffset(dt).ToUnixTimeMilliseconds();

    /// <summary>
    /// 将日期时间转化为毫秒级Unix时间戳
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="offset">时区偏移量</param>
    /// <returns>毫秒级Unix时间戳</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToUnixTimeMilliseconds(this DateTime dt, TimeSpan offset) =>
        new DateTimeOffset(dt, offset).ToUnixTimeMilliseconds();

    /// <summary>
    /// 获取日期时间所在当年第几周
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>第几周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int WeekOfYear(this DateTime dt) => DateTimeHelper.GetWeekOfYear(dt.Year, dt.Month, dt.Day);

    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <returns>周几</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TodayOfWeek(this DateTime dt) => dt.DayOfWeek.TodayOfWeek();

    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    public static int TodayOfWeek(this DateTime dt, TimeSpan criticalValue)
    {
        var dayOfWeek = dt.DayOfWeek.TodayOfWeek();
        if (dt.TimeOfDay >= criticalValue) return dayOfWeek;

        dayOfWeek--;
        return dayOfWeek == 0 ? 7 : dayOfWeek;
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// 获取日期时间是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="dt">日期时间</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    public static int TodayOfWeek(this DateTime dt, TimeOnly criticalValue)
    {
        var dayOfWeek = dt.DayOfWeek.TodayOfWeek();
        if (dt.TimeOfDay >= criticalValue.AsTimeSpan()) return dayOfWeek;

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
    public static DateOnly GetDateOnly(this DateTime offset) =>
        new(offset.Year, offset.Month, offset.Day);

    /// <summary>
    /// 获取日期时间的时间
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>日期</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly GetTimeOnly(this DateTime offset) =>
        new(offset.Hour, offset.Minute, offset.Second, offset.Millisecond);
#endif
}