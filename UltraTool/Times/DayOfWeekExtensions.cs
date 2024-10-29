using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 周几枚举拓展类
/// </summary>
[PublicAPI]
public static class DayOfWeekExtensions
{
    /// <summary>
    /// 将枚举转为数值，周一为1，周日为7
    /// </summary>
    /// <param name="dayOfWeek">周几枚举</param>
    /// <returns>周几数值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TodayOfWeek(this DayOfWeek dayOfWeek) =>
        dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
}