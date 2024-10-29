using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 偏移日期时间拓展类
/// </summary>
[PublicAPI]
public static class DateTimeOffsetExtensions
{
#if NET6_0_OR_GREATER
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

    /// <summary>
    /// 获取日期时间的年月
    /// </summary>
    /// <param name="offset">日期时间</param>
    /// <returns>年月</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YearMonth GetYearMonth(this DateTimeOffset offset) => new(offset.Year, offset.Month);
}