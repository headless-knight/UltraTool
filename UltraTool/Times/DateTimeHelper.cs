using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 日期时间帮助类
/// </summary>
[PublicAPI]
public static class DateTimeHelper
{
    #region 常量

    /// <summary>闰月月份</summary>
    private const int LeapMonth = 2;

    /// <summary>年总天数</summary>
    private const int DayAmountOfYear = 365;

    /// <summary>闰年总天数</summary>
    private const int DayAmountOfLeapYear = 366;

    /// <summary>最小月份</summary>
    private const int MinMonth = 1;

    /// <summary>最大月份</summary>
    private const int MaxMonth = 12;

    /// <summary>最大月份总天数</summary>
    private const int MaxMonthDayAmount = 31;

    /// <summary>正午时刻</summary>
    private const int NoonHour = 12;

    /// <summary>
    /// 一秒毫秒数
    /// </summary>
    public const int OneSecondMilliseconds = 1000;

    /// <summary>
    /// 一分钟秒数
    /// </summary>
    public const int OneMinuteSeconds = 60;

    /// <summary>
    /// 一分钟毫秒数
    /// </summary>
    public const int OneMinuteMilliseconds = OneMinuteSeconds * OneSecondMilliseconds;

    /// <summary>
    /// 一小时分钟数
    /// </summary>
    public const int OneHourMinutes = 60;

    /// <summary>
    /// 一小时秒数
    /// </summary>
    public const int OneHourSeconds = OneHourMinutes * OneMinuteSeconds;

    /// <summary>
    /// 一小时毫秒数
    /// </summary>
    public const int OneHourMilliseconds = OneHourSeconds * OneSecondMilliseconds;

    /// <summary>
    /// 一天小时数
    /// </summary>
    public const int OneDayHours = 24;

    /// <summary>
    /// 一天分钟数
    /// </summary>
    public const int OneDayMinutes = OneDayHours * OneHourMinutes;

    /// <summary>
    /// 一天秒数
    /// </summary>
    public const int OneDaySeconds = OneDayMinutes * OneMinuteSeconds;

    /// <summary>
    /// 一天毫秒数
    /// </summary>
    public const int OneDayMilliseconds = OneDaySeconds * OneSecondMilliseconds;

    #endregion

    /// <summary>
    /// 判断年份是否为闰年
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>是否闰年</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(int year) => DateTime.IsLeapYear(year);

    /// <summary>
    /// 判断指定年份月份是否为闰月
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>是否闰月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapMonth(int year, int month) => month == LeapMonth && IsLeapYear(year);

    /// <summary>
    /// 获取指定年份有多少周
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>周数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetWeekAmount(int year) => GetWeekOfYear(year, MaxMonth, MaxMonthDayAmount);

    /// <summary>
    /// 获取指定年月日为当年第几周
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>周数</returns>
    [Pure]
    public static int GetWeekOfYear(int year, int month, int day)
    {
        var end = new DateTime(year, month, day);
        var calendar = CultureInfo.CurrentCulture.Calendar;
        // 周一作为每周第一天计算
        return calendar.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    /// <summary>
    /// 获取指定年份有多少天
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>天数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDayAmount(int year) => IsLeapYear(year) ? DayAmountOfYear : DayAmountOfLeapYear;

    /// <summary>
    /// 获取指定年份指定月份有多少天
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>天数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDayAmount(int year, int month) => DateTime.DaysInMonth(year, month);
}