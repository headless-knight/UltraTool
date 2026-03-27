using System.Globalization;
using System.Runtime.CompilerServices;
using UltraTool.Helpers;

namespace UltraTool.Times;

/// <summary>
/// 日期时间帮助类
/// </summary>
public static class DateTimeHelper
{
    #region 常量

    /// <summary>闰月月份</summary>
    private const int LeapMonth = 2;

    /// <summary>年总天数</summary>
    private const int DayAmountOfYear = 365;

    /// <summary>闰年总天数</summary>
    private const int DayAmountOfLeapYear = 366;

    /// <summary>最大月份总天数</summary>
    private const int MaxMonthDayAmount = 31;

    /// <summary>正午时刻</summary>
    private const int NoonHour = 12;

    /// <summary>
    /// 最小月份
    /// </summary>
    public const int MinMonth = 1;

    /// <summary>
    /// 最大月份
    /// </summary>
    public const int MaxMonth = 12;

    /// <summary>
    /// 一秒钟毫秒数
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset FromUnixTimeSeconds(long seconds) =>
        DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime();

    /// <summary>
    /// 将毫秒级时间戳转为本地时间
    /// </summary>
    /// <param name="milliseconds">毫秒级时间戳</param>
    /// <returns>本地时间</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset FromUnixTimeMilliseconds(long milliseconds) =>
        DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).ToLocalTime();

    /// <summary>
    /// 判断年份是否为闰年
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>是否闰年</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLeapYear(int year) => DateTime.IsLeapYear(year);

    /// <summary>
    /// 判断指定年份月份是否为闰月
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>是否闰月</returns>
    public static bool IsLeapMonth(int year, int month) => IsLeapYear(year) && month == LeapMonth;

    /// <summary>
    /// 获取指定年份有多少周
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>周数</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetWeekAmount(int year) => GetWeekOfYear(year, MaxMonth, MaxMonthDayAmount);

    /// <summary>
    /// 计算指定日期位于当年第几周
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>周数</returns>
    public static int GetWeekOfYear(int year, int month, int day)
    {
        var end = new DateTime(year, month, day);
        var calendar = CultureInfo.InvariantCulture.Calendar;
        // 周一作为每周第一天计算
        return calendar.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    /// <summary>
    /// 获取指定年份有多少天
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>天数</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDayAmount(int year) => IsLeapYear(year) ? DayAmountOfLeapYear : DayAmountOfYear;

    /// <summary>
    /// 获取指定年份指定月份有多少天
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>天数</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetDayAmount(int year, int month) => DateTime.DaysInMonth(year, month);

    #region 获取时刻

    /// <summary>
    /// 获取当前时刻后的下一个指定时刻
    /// </summary>
    /// <param name="moment">时刻</param>
    /// <returns>下一个指定时刻</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset GetNextMoment(TimeSpan moment) => GetNextMoment(DateTimeOffset.Now, moment);

    /// <summary>
    /// 获取日期时间后的下一个指定时刻
    /// </summary>
    /// <param name="afterThis">基准日期时间</param>
    /// <param name="moment">时刻</param>
    /// <returns>下一个指定时刻</returns>
    public static DateTimeOffset GetNextMoment(DateTimeOffset afterThis, TimeSpan moment)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(moment.TotalMilliseconds, OneDayMilliseconds);
        var nextMoment = new DateTimeOffset(afterThis.Date.Add(moment), afterThis.Offset);
        return nextMoment > afterThis ? nextMoment : nextMoment.AddDays(1);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// 获取日期时间后的下一个指定周几时刻
    /// </summary>
    /// <param name="afterThis">基准日期时间</param>
    /// <param name="dayOfWeek">周几，范围[1,7]，1为周一</param>
    /// <param name="moment">时刻</param>
    /// <returns>下一个指定时刻</returns>
    public static DateTimeOffset GetNextWeekMoment(DateTimeOffset afterThis, int dayOfWeek, TimeSpan moment)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(moment.TotalMilliseconds, OneDayMilliseconds);
        var weekDate = GetDayOfWeek(dayOfWeek, afterThis.GetDateOnly());
        var nextMoment = OffsetOf(weekDate, moment.AsTimeOnly(), afterThis.Offset);
        return nextMoment > afterThis ? nextMoment : nextMoment.AddDays(7);
    }

    /// <summary>
    /// 获取日期时间后的下一个指定月几号时刻
    /// </summary>
    /// <param name="afterThis">基准日期时间</param>
    /// <param name="dayOfMonth">几号，范围[1,28|29|30|31]，负数为倒数第几天</param>
    /// <param name="moment"></param>
    /// <returns>下一个指定时刻</returns>
    public static DateTimeOffset GetNextMonthMoment(DateTimeOffset afterThis, int dayOfMonth, TimeSpan moment)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(moment.TotalMilliseconds, OneDayMilliseconds);
        var yearMonth = afterThis.GetYearMonth();
        var monthDate = GetDayOfMonth(dayOfMonth, yearMonth);
        var nextMoment = OffsetOf(monthDate, moment.AsTimeOnly(), afterThis.Offset);
        // 计算出的日期时间大于基准日期时间，则返回计算出的日期时间
        if (nextMoment > afterThis) return nextMoment;

        // 如果是几号正数，则返回下个月的日期时间
        if (dayOfMonth > 0) return nextMoment.AddMonths(1);

        // 如果是几号负数，则添加下个月的天数
        var nextMonthDayAmount = yearMonth.AddMonths(1).GetDayAmount();
        return nextMoment.AddDays(nextMonthDayAmount);
    }
#endif

    #endregion

    #region 周日期

#if NET6_0_OR_GREATER
    /// <summary>
    /// 获取当前周的指定周几日期
    /// </summary>
    /// <param name="dayOfWeek">周几，范围[1,7]，1为周一</param>
    /// <returns>周几日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetDayOfWeek(int dayOfWeek) =>
        GetDayOfWeek(dayOfWeek, NowDateOnly);

    /// <summary>
    /// 获取指定周的指定周几日期
    /// </summary>
    /// <param name="dayOfWeek">周几，范围[1,7]，1为周一</param>
    /// <param name="date">指定周任意日期，用于界定指定周</param>
    /// <returns>周几日期</returns>
    public static DateOnly GetDayOfWeek(int dayOfWeek, DateOnly date)
    {
        if (dayOfWeek is < 1 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, "周几范围必须在1到7之间");
        }

        return date.AddDays(dayOfWeek - date.DayOfWeek.TodayOfWeek());
    }

    /// <summary>
    /// 获取当前周的周一日期
    /// </summary>
    /// <returns>周一日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetMondayOfWeek() => GetMondayOfWeek(NowDateOnly);

    /// <summary>
    /// 获取指定周的周一日期
    /// </summary>
    /// <param name="date">指定周任意日期，用于界定指定周</param>
    /// <returns>周一日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetMondayOfWeek(DateOnly date) => GetDayOfWeek(1, date);

    /// <summary>
    /// 获取当前周的周日日期
    /// </summary>
    /// <returns>周日日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetSundayOfWeek() => GetSundayOfWeek(NowDateOnly);

    /// <summary>
    /// 获取指定周的周日日期
    /// </summary>
    /// <param name="date">指定周任意日期，用于界定指定周</param>
    /// <returns>周日日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateOnly GetSundayOfWeek(DateOnly date) => GetDayOfWeek(7, date);

    /// <summary>
    /// 获取当前日期是当周周几，周一为1，周日为7
    /// </summary>
    /// <returns>周几</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTodayOfWeek() => NowDateOnly.TodayOfWeek();

    /// <summary>
    /// 获取当前日期是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTodayOfWeek(TimeOnly criticalValue) =>
        DateTimeOffset.Now.TodayOfWeek(criticalValue);

    /// <summary>
    /// 获取当前日期是当周周几，周一为1，周日为7
    /// </summary>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>周几</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTodayOfWeek(TimeSpan criticalValue) =>
        DateTimeOffset.Now.TodayOfWeek(criticalValue);

    /// <summary>
    /// 获取指定日期是当周周几，周一为1，周日为7
    /// </summary>
    /// <returns>周几</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTodayOfWeek(int year, int month, int day) =>
        new DateOnly(year, month, day).TodayOfWeek();
#endif

    #endregion

    #region 月日期

#if NET6_0_OR_GREATER
    /// <summary>
    /// 获取指定年月的指定几号的日期
    /// </summary>
    /// <param name="dayOfMonth">几号，负数为倒数第几天</param>
    /// <param name="yearMonth"></param>
    /// <returns>日期</returns>
    public static DateOnly GetDayOfMonth(int dayOfMonth, YearMonth yearMonth)
    {
        ArgumentOutOfRangeHelper.ThrowIfZero(dayOfMonth);
        var dayAmount = yearMonth.GetDayAmount();
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(Math.Abs(dayOfMonth), dayAmount, nameof(dayOfMonth));
        return dayOfMonth > 0 ? yearMonth.ToDateOnly(dayOfMonth) : yearMonth.ToDateOnly(dayAmount + dayOfMonth + 1);
    }
#endif

    #endregion

#if NET6_0_OR_GREATER
    /// <summary>
    /// 将日期与时间转化为日期时间
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="time">时间</param>
    /// <param name="kind">日期时间种类，默认为本地</param>
    /// <returns>日期时间</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime Of(DateOnly date, TimeOnly time, DateTimeKind kind = DateTimeKind.Local) =>
        new(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond, kind);

    /// <summary>
    /// 将日期与时间转化为偏移日期时间
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="time">时间</param>
    /// <param name="offset">时间偏移量</param>
    /// <returns>日期时间</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset OffsetOf(DateOnly date, TimeOnly time, TimeSpan offset) =>
        new(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, time.Millisecond, offset);

    /// <summary>
    /// 将日期与时间转化为偏移日期时间
    /// </summary>
    /// <param name="date">日期</param>
    /// <param name="time">时间</param>
    /// <param name="kind">日期时间种类，默认为本地</param>
    /// <returns>偏移日期时间</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset OffsetOf(DateOnly date, TimeOnly time, DateTimeKind kind = DateTimeKind.Local) =>
        new(Of(date, time, kind));
#endif

    /// <summary>
    /// 判断时刻是否为上午
    /// </summary>
    /// <param name="hour">时</param>
    /// <returns>是否为上午</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAm(int hour) => hour < NoonHour;

    /// <summary>
    /// 判断时刻是否为下午
    /// </summary>
    /// <param name="hour">时</param>
    /// <returns>是否为下午</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPm(int hour) => hour >= NoonHour;

    #region 年月日判断

    /// <summary>
    /// 判断是否为有效月日
    /// </summary>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>是否为有效月日</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidMonthDay(int month, int day) => day > 0 && (month switch
    {
        1 or 3 or 5 or 7 or 8 or 10 or 12 => day <= 31,
        4 or 6 or 9 or 11 => day <= 30,
        2 => day <= 29,
        _ => false
    });

    /// <summary>
    /// 判断是否为有效日期
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">天数</param>
    /// <returns>是否为有效日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidDate(int year, int month, int day) => day > 0 && (month == LeapMonth
        ? (IsLeapYear(year) ? day <= 29 : day <= 28)
        : IsValidMonthDay(month, day));

    /// <summary>
    /// 拼接日期为数字
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="day">日期</param>
    /// <returns>year * 10000 + month * 100 + day</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SpliceDateNumber(int year, int month, int day) =>
        year * 10000 + month * 100 + day;

    /// <summary>
    /// 分割日期数字
    /// </summary>
    /// <param name="date">year * 10000 + month * 100 + day</param>
    /// <returns>(年份, 月份, 日期)</returns>
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
    public static bool IsSameDay(DateTimeOffset offset1, DateTimeOffset offset2, TimeSpan criticalValue)
    {
        if (offset1.TimeOfDay < criticalValue)
        {
            offset1 = offset1.AddDays(-1);
        }

        if (offset2.TimeOfDay < criticalValue)
        {
            offset2 = offset2.AddDays(-1);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSameWeek(DateOnly date1, DateOnly date2) =>
        IsSameWeek(date1.AsDateTime(), date2.AsDateTime());

    /// <summary>
    /// 判断两个日期时间是否同一周
    /// </summary>
    /// <param name="dt1">日期时间1</param>
    /// <param name="dt2">日期时间2</param>
    /// <param name="criticalValue">临界值，时间小于此值视作前一天</param>
    /// <returns>是否同一周</returns>
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
    public static bool IsSameWeek(DateTime dt1, DateTime dt2)
    {
        var calendar = CultureInfo.InvariantCulture.Calendar;
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
    public static bool IsSameWeek(DateTimeOffset offset1, DateTimeOffset offset2)
    {
        var calendar = CultureInfo.InvariantCulture.Calendar;
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