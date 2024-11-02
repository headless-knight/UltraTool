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

    #region 属性

    /// <summary>
    /// 当前日期时间
    /// </summary>
    public static DateTimeOffset Now => DateTimeOffset.Now;

    /// <summary>
    /// 当前UTC日期时间
    /// </summary>
    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    /// <summary>
    /// 当前时间戳，单位为秒
    /// </summary>
    public static long NowUnixTimeSeconds => Now.ToUnixTimeSeconds();

    /// <summary>
    /// 当前时间戳，单位为毫秒
    /// </summary>
    public static long NowUnixTimeMilliseconds => Now.ToUnixTimeMilliseconds();

#if NET6_0_OR_GREATER
    /// <summary>
    /// 当前日期
    /// </summary>
    public static DateOnly NowDateOnly => Now.GetDateOnly();

    /// <summary>
    /// 当前时间
    /// </summary>
    public static TimeOnly NowTimeOnly => Now.GetTimeOnly();

    /// <summary>
    /// 当前UTC日期
    /// </summary>
    public static DateOnly UtcNowDateOnly => UtcNow.GetDateOnly();

    /// <summary>
    /// 当前UTC时间
    /// </summary>
    public static TimeOnly UtcNowTimeOnly => UtcNow.GetTimeOnly();
#endif

    #endregion

    /// <summary>
    /// 将秒级时间戳转为本地时间
    /// </summary>
    /// <param name="seconds">秒级时间戳</param>
    /// <returns>本地时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset FromUnixTimeSeconds(long seconds) =>
        DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime();

    /// <summary>
    /// 将毫秒级时间戳转为本地时间
    /// </summary>
    /// <param name="milliseconds">毫秒级时间戳</param>
    /// <returns>本地时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds) =>
        DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).ToLocalTime();

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

    /// <summary>
    /// 判断时刻是否为上午
    /// </summary>
    /// <param name="hour">时</param>
    /// <returns>是否为上午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAm(int hour) => hour < NoonHour;

    /// <summary>
    /// 判断时刻是否为下午
    /// </summary>
    /// <param name="hour">时</param>
    /// <returns>是否为下午</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPm(int hour) => hour >= NoonHour;

    #region 年月日判断

    /// <summary>
    /// 判断是否为有效月日
    /// </summary>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>是否为有效月日</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidMonthDay(int month, int day) => month switch
    {
        1 or 3 or 5 or 7 or 8 or 10 or 12 => day <= 31,
        4 or 6 or 9 or 11 => day <= 30,
        2 => day <= 29,
        _ => false
    };

    /// <summary>
    /// 判断是否为有效日期
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>是否为有效日期</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidDate(int year, int month, int day) => month == 2
        ? (IsLeapYear(year) ? day <= 29 : day <= 28)
        : IsValidMonthDay(month, day);

    /// <summary>
    /// 拼接日期为数字
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">日期</param>
    /// <returns>year * 10000 + month * 100 + day</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SpliceDateNumber(int year, int month, int day) =>
        year * 10000 + month * 100 + day;

    /// <summary>
    /// 分割日期数字
    /// </summary>
    /// <param name="date">year * 10000 + month * 100 + day</param>
    /// <returns>(年份, 月份, 日期)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int Year, int Month, int Day) SplitDateNumber(int date) =>
        (date / 10000, date / 100 % 100, date % 100);

    #endregion

    #region 同一天判断

#if NET6_0_OR_GREATER
    /// <summary>
    /// 判断两个日期是否为同一天
    /// </summary>
    /// <param name="date1">日期1</param>
    /// <param name="date2">日期2</param>
    /// <returns>是否同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(DateOnly date1, DateOnly date2) =>
        date1.DayNumber == date2.DayNumber;

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(DateTime dt1, DateTime dt2, TimeOnly criticalValue) =>
        IsSameDay(dt1, dt2, criticalValue.AsTimeSpan());

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(DateTimeOffset offset1, DateTimeOffset offset2, TimeOnly criticalValue) =>
        IsSameDay(offset1, offset2, criticalValue.AsTimeSpan());
#endif

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <returns>是否同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(DateTime dt1, DateTime dt2) =>
        dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day;

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一天</returns>
    [Pure]
    public static bool IsSameDay(DateTime dt1, DateTime dt2, TimeSpan criticalValue)
    {
        if (dt1.TimeOfDay < criticalValue)
        {
            dt1 = dt1.AddDays(-1);
        }

        if (dt2.TimeOfDay < criticalValue)
        {
            dt2 = dt2.AddDays(-1);
        }

        return IsSameDay(dt1, dt2);
    }

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <returns>是否同一天</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameDay(DateTimeOffset offset1, DateTimeOffset offset2) =>
        offset1.Year == offset2.Year && offset1.Month == offset2.Month && offset1.Day == offset2.Day;

    /// <summary>
    /// 判断两个日期时间是否为同一天
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一天</returns>
    [Pure]
    public static bool IsSameDay(DateTimeOffset offset1, DateTimeOffset offset2, TimeSpan criticalValue)
    {
        if (offset1.TimeOfDay < criticalValue)
        {
            offset1 = offset1.AddDays(-1);
        }

        if (offset2.TimeOfDay < criticalValue)
        {
            offset2 = offset1.AddDays(-1);
        }

        return IsSameDay(offset1, offset2);
    }

    #endregion

    #region 同一周判断

#if NET6_0_OR_GREATER
    /// <summary>
    /// 判断两个日期是否同一周
    /// </summary>
    /// <param name="date1">日期1</param>
    /// <param name="date2">日期2</param>
    /// <returns>是否同一周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameWeek(DateOnly date1, DateOnly date2) =>
        IsSameDay(date1.AsDateTime(), date2.AsDateTime());

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameWeek(DateTime dt1, DateTime dt2, TimeOnly criticalValue) =>
        IsSameWeek(dt1, dt2, criticalValue.AsTimeSpan());

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一周</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameWeek(DateTimeOffset offset1, DateTimeOffset offset2, TimeOnly criticalValue) =>
        IsSameWeek(offset1, offset2, criticalValue.AsTimeSpan());
#endif

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <returns>是否同一周</returns>
    [Pure]
    public static bool IsSameWeek(DateTime dt1, DateTime dt2)
    {
        var calendar = CultureInfo.CurrentCulture.Calendar;
        // 周一作为每周第一天计算
        return calendar.GetWeekOfYear(dt1, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
               == calendar.GetWeekOfYear(dt2, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一周</returns>
    [Pure]
    public static bool IsSameWeek(DateTime dt1, DateTime dt2, TimeSpan criticalValue)
    {
        if (dt1.TodayOfWeek() == 1 && dt1.TimeOfDay < criticalValue)
        {
            dt1 = dt1.AddDays(-1);
        }

        if (dt2.TodayOfWeek() == 1 && dt2.TimeOfDay < criticalValue)
        {
            dt2 = dt2.AddDays(-1);
        }

        return IsSameWeek(dt1, dt2);
    }

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <returns>是否同一周</returns>
    [Pure]
    public static bool IsSameWeek(DateTimeOffset offset1, DateTimeOffset offset2)
    {
        var calendar = CultureInfo.CurrentCulture.Calendar;
        // 周一作为每周第一天计算
        return calendar.GetWeekOfYear(offset1.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
               == calendar.GetWeekOfYear(offset2.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一周</returns>
    [Pure]
    public static bool IsSameWeek(DateTimeOffset offset1, DateTimeOffset offset2, TimeSpan criticalValue)
    {
        if (offset1.TodayOfWeek() == 1 && offset1.TimeOfDay < criticalValue)
        {
            offset1 = offset1.AddDays(-1);
        }

        if (offset2.TodayOfWeek() == 1 && offset2.TimeOfDay < criticalValue)
        {
            offset2 = offset2.AddDays(-1);
        }

        return IsSameWeek(offset1, offset2);
    }

    #endregion

    #region 同一月判断

#if NET6_0_OR_GREATER
    /// <summary>
    /// 判断两个日期是否为同一月
    /// </summary>
    /// <param name="date1">日期1</param>
    /// <param name="date2">日期2</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(DateOnly date1, DateOnly date2) =>
        date1.Year == date2.Year && date1.Month == date2.Month;

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(DateTime dt1, DateTime dt2, TimeOnly criticalValue) =>
        IsSameMonth(dt1, dt2, criticalValue.AsTimeSpan());

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(DateTimeOffset offset1, DateTimeOffset offset2, TimeOnly criticalValue) =>
        IsSameMonth(offset1, offset2, criticalValue.AsTimeSpan());
#endif

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(DateTime dt1, DateTime dt2) =>
        dt1.Year == dt2.Year && dt1.Month == dt2.Month;

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    public static bool IsSameMonth(DateTime dt1, DateTime dt2, TimeSpan criticalValue)
    {
        if (dt1.Day == 1 && dt1.TimeOfDay < criticalValue)
        {
            dt1 = dt1.AddDays(-1);
        }

        if (dt2.Day == 1 && dt2.TimeOfDay < criticalValue)
        {
            dt2 = dt2.AddDays(-1);
        }

        return IsSameMonth(dt1, dt2);
    }

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameMonth(DateTimeOffset offset1, DateTimeOffset offset2) =>
        offset1.Year == offset2.Year && offset1.Month == offset2.Month;

    /// <summary>
    /// 判断两个日期时间是否为同一月
    /// </summary>
    /// <param name="offset1">日期时间1</param>
    /// <param name="offset2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否为同一月</returns>
    [Pure]
    public static bool IsSameMonth(DateTimeOffset offset1, DateTimeOffset offset2, TimeSpan criticalValue)
    {
        if (offset1.Day == 1 && offset1.TimeOfDay < criticalValue)
        {
            offset1 = offset1.AddDays(-1);
        }

        if (offset2.Day == 1 && offset2.TimeOfDay < criticalValue)
        {
            offset2 = offset2.AddDays(-1);
        }

        return IsSameMonth(offset1, offset2);
    }

    #endregion
}