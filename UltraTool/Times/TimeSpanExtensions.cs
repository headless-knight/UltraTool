using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Times;

/// <summary>
/// 时间跨度拓展类
/// </summary>
[PublicAPI]
public static class TimeSpanExtensions
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// 时间跨度转为时间
    /// </summary>
    /// <param name="span">时间跨度</param>
    /// <returns>时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeOnly AsTimeOnly(this TimeSpan span) => new(span.Ticks);
#endif
}