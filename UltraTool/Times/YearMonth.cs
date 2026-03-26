using System.Runtime.CompilerServices;
using UltraTool.Helpers;

namespace UltraTool.Times;

/// <summary>
/// 年月
/// </summary>
public readonly struct YearMonth : IEquatable<YearMonth>, IComparable<YearMonth>
{
    /// <summary>
    /// year * 12 + month - 1
    /// </summary>
    private readonly int _totalMonths;

    /// <summary>
    /// 年份
    /// </summary>
    public int Year => _totalMonths / DateTimeHelper.MaxMonth;

    /// <summary>
    /// 月份
    /// </summary>
    public int Month => _totalMonths % DateTimeHelper.MaxMonth + 1;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    public YearMonth(int year, int month)
    {
        ArgumentOutOfRangeHelper.ThrowIfLessThan(month, DateTimeHelper.MinMonth);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(month, DateTimeHelper.MaxMonth);
        _totalMonths = year * DateTimeHelper.MaxMonth + month - 1;
    }

    /// <summary>
    /// 内部构造方法，直接使用总月数
    /// </summary>
    /// <param name="totalMonths">总月数</param>
    private YearMonth(int totalMonths)
    {
        _totalMonths = totalMonths;
    }

    /// <summary>
    /// 获取当前年月
    /// </summary>
    public static YearMonth Now
    {
        get
        {
            var now = DateTime.Now;
            return new YearMonth(now.Year, now.Month);
        }
    }

    /// <summary>
    /// 从 DateTime 创建年月
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YearMonth FromDateTime(DateTime dateTime) => new(dateTime.Year, dateTime.Month);

    /// <summary>
    /// 尝试解析年月字符串，支持格式：yyyyMM, yyyy-MM, yyyy/MM
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="result">解析结果</param>
    /// <returns>是否解析成功</returns>
    public static bool TryParse(ReadOnlySpan<char> str, out YearMonth result)
    {
        str = str.Trim();
        switch (str.Length)
        {
            // yyyyMM
            case 6:
            {
                if (int.TryParse(str, out var value))
                {
                    var year = value / 100;
                    var month = value % 100;
                    if (month is >= 1 and <= 12)
                    {
                        result = new YearMonth(year, month);
                        return true;
                    }
                }

                break;
            }
            // yyyy-MM或yyyy/MM
            case 7:
            {
                if ((str[4] == '-' || str[4] == '/'))
                {
                    if (int.TryParse(str[..4], out var year) && int.TryParse(str[5..], out var month) &&
                        month is >= DateTimeHelper.MinMonth and <= DateTimeHelper.MaxMonth)
                    {
                        result = new YearMonth(year, month);
                        return true;
                    }
                }

                break;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// 解析年月字符串
    /// </summary>
    /// <param name="str">字符串</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YearMonth Parse(ReadOnlySpan<char> str) =>
        TryParse(str, out var result) ? result : throw new FormatException("Cannot parse YearMonth");

    /// <summary>
    /// 解构方法
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    public void Deconstruct(out int year, out int month)
    {
        year = Year;
        month = Month;
    }

    /// <summary>
    /// 是否为闰年
    /// </summary>
    /// <returns>是否为闰年</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsLeapYear() => DateTimeHelper.IsLeapYear(Year);

    /// <summary>
    /// 是否为闰月
    /// </summary>
    /// <returns>是否为闰年</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsLeapMonth() => DateTimeHelper.IsLeapMonth(Year, Month);

    /// <summary>
    /// 获取月份天数
    /// </summary>
    /// <returns>天数</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetDayAmount() => DateTimeHelper.GetDayAmount(Year, Month);

    /// <summary>
    /// 添加月份
    /// </summary>
    /// <param name="months">月份</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public YearMonth AddMonths(int months) => new(_totalMonths + months);

    /// <summary>
    /// 添加年份
    /// </summary>
    /// <param name="years">年份</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public YearMonth AddYears(int years) => new(_totalMonths + years * DateTimeHelper.MaxMonth);

    /// <summary>
    /// 计算与另一个年月之间的月份差
    /// </summary>
    /// <param name="other">另一个年月</param>
    /// <returns>月份差（正数表示 this 在 other 之后）</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int MonthsBetween(YearMonth other) => _totalMonths - other._totalMonths;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => $"{Year:D4}-{Month:D2}";

    /// <summary>
    /// 转为紧凑格式字符串 (yyyyMM)
    /// </summary>
    /// <returns>字符串</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToCompactString() => $"{Year:D4}{Month:D2}";

#if NET6_0_OR_GREATER
    /// <summary>
    /// 从 DateOnly 创建年月
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YearMonth FromDateOnly(DateOnly date) => new(date.Year, date.Month);

    /// <summary>
    /// 转为当月1号日期
    /// </summary>
    /// <returns>日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly ToDateOnly() => ToDateOnly(1);

    /// <summary>
    /// 转为日期
    /// </summary>
    /// <param name="day">几号</param>
    /// <returns>日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly ToDateOnly(int day) => new(Year, Month, day);

    /// <summary>
    /// 转为当月最后一天的日期
    /// </summary>
    /// <returns>日期</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateOnly ToLastDayDateOnly() => new(Year, Month, GetDayAmount());
#endif

    #region 接口实现

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(YearMonth other) => _totalMonths == other._totalMonths;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is YearMonth other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _totalMonths;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(YearMonth other) => _totalMonths.CompareTo(other._totalMonths);

    #endregion

    #region 运算符重载

    /// <summary>
    /// 相等运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>是否相等</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(YearMonth left, YearMonth right) => left.Equals(right);

    /// <summary>
    /// 不等运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>是否不相等</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(YearMonth left, YearMonth right) => !left.Equals(right);

    /// <summary>
    /// 大于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否大于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(YearMonth left, YearMonth right) => left._totalMonths > right._totalMonths;

    /// <summary>
    /// 小于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否小于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(YearMonth left, YearMonth right) => left._totalMonths < right._totalMonths;

    /// <summary>
    /// 大于等于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否大于等于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(YearMonth left, YearMonth right) => left._totalMonths >= right._totalMonths;

    /// <summary>
    /// 小于等于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否小于等于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(YearMonth left, YearMonth right) => left._totalMonths <= right._totalMonths;

#if NET6_0_OR_GREATER
    /// <summary>
    /// 日期隐式转换为年月
    /// </summary>
    /// <param name="date">日期</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator YearMonth(DateOnly date) => new(date.Year, date.Month);
#endif

    #endregion
}