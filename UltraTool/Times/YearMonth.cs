using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;

namespace UltraTool.Times;

/// <summary>
/// 年月
/// </summary>
[PublicAPI]
public readonly struct YearMonth : IEquatable<YearMonth>, IComparable<YearMonth>
{
    /// <summary>(year * 100 + month)</summary>
    private readonly int _value;

    /// <summary>
    /// 年份
    /// </summary>
    public int Year => _value / 100;

    /// <summary>
    /// 月份
    /// </summary>
    public int Month => _value % 100;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="year">年</param>
    /// <param name="month">月</param>
    public YearMonth(int year, int month)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegativeOrZero(month);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(month, 12);
        _value = year * 100 + month;
    }

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
    public YearMonth AddMonths(int months)
    {
        var year = Year + (months / 12);
        var month = Month + (months % 12);
        while (month is 0 or > 12)
        {
            if (month == 0)
            {
                year--;
                month = 12;
                continue;
            }

            year++;
            month -= 12;
        }

        return new YearMonth(year, month);
    }

    /// <summary>
    /// 添加年份
    /// </summary>
    /// <param name="years">年份</param>
    /// <returns>年月</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public YearMonth AddYears(int years) => new(Year + years, Month);

#if NET6_0_OR_GREATER
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
#endif

    #region 接口实现

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(YearMonth other) => _value == other._value;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is YearMonth other && Equals(other);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => _value;

    /// <inheritdoc />
    public int CompareTo(YearMonth other) => _value.CompareTo(other._value);

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
    public static bool operator !=(YearMonth left, YearMonth right) => !(left == right);

    /// <summary>
    /// 大于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否大于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(YearMonth left, YearMonth right) => left.CompareTo(right) > 0;

    /// <summary>
    /// 小于运算符
    /// </summary>
    /// <param name="left">值1</param>
    /// <param name="right">值2</param>
    /// <returns>值1是否小于值2</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(YearMonth left, YearMonth right) => left.CompareTo(right) < 0;

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