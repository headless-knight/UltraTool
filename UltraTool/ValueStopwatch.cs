using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool;

/// <summary>
/// 值类型秒表
/// </summary>
[PublicAPI]
public struct ValueStopwatch
{
    private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
    private long _value;

    /// <summary>
    /// 秒表是否在运行
    /// </summary>
    public readonly bool IsRunning => _value > 0;

    /// <summary>
    /// 持续时间跨度
    /// </summary>
    public readonly TimeSpan Elapsed => TimeSpan.FromTicks(ElapsedTicks);

    /// <summary>
    /// 持续刻度数
    /// </summary>
    public readonly long ElapsedTicks
    {
        [Pure]
        get
        {
            // 时间戳值为正表示秒表开始运行的时间
            // 负值表示秒表停止的总持续时间的负值
            var timestamp = _value;
            long delta;
            if (IsRunning)
            {
                var end = Stopwatch.GetTimestamp();
                delta = end - timestamp;
            }
            else
            {
                delta = -timestamp;
            }

            return (long)(delta * TimestampToTicks);
        }
    }

    /// <summary>
    /// 持续毫秒数
    /// </summary>
    public readonly long ElapsedMilliseconds => ElapsedTicks / 10000L;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="timestamp">时间戳</param>
    public ValueStopwatch(long timestamp)
    {
        _value = timestamp;
    }

    /// <summary>
    /// 获取此实例的计数器值
    /// </summary>
    /// <remarks>
    /// 时间戳值为正表示秒表开始运行的时间
    /// 负值表示秒表停止的总持续时间的负值
    /// </remarks>
    /// <returns>计数器值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly long GetRawTimestamp() => _value;

    /// <summary>
    /// 启动秒表
    /// </summary>
    public void Start()
    {
        var timestamp = _value;
        // 正在运行中
        if (IsRunning) return;

        // 秒表停止，此值为零或负值
        // 将负值添加到当前时间戳以再次启动秒表
        var newValue = GetTimestamp() + timestamp;
        if (newValue == 0)
        {
            newValue = 1;
        }

        _value = newValue;
    }

    /// <summary>
    /// 重新启动秒表
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Restart() => _value = GetTimestamp();

    /// <summary>
    /// 重置秒表
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => _value = 0;

    /// <summary>
    /// 停止秒表
    /// </summary>
    public void Stop()
    {
        var timestamp = _value;
        // 已经停止
        if (!IsRunning) return;

        var end = GetTimestamp();
        var delta = end - timestamp;
        _value = -delta;
    }

    #region 静态方法

    /// <summary>
    /// 创建并启动新秒表实例
    /// </summary>
    /// <returns>秒表</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueStopwatch StartNew() => new(GetTimestamp());

    /// <summary>
    /// 创建并启动新秒表实例
    /// </summary>
    /// <param name="elapsed">初始时间跨度</param>
    /// <returns>秒表</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueStopwatch StartNew(TimeSpan elapsed) =>
        new(GetTimestamp() - (long)(elapsed.TotalSeconds * Stopwatch.Frequency));

    /// <summary>
    /// 获取计时器机制中的刻度数
    /// </summary>
    /// <returns>刻度数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long GetTimestamp() => Stopwatch.GetTimestamp();

    /// <summary>
    /// 计算秒表起始时间戳至今的时间跨度
    /// </summary>
    /// <param name="startingTimestamp">起始时间戳</param>
    /// <returns>时间跨度</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan GetElapsedTime(long startingTimestamp) => GetElapsedTime(startingTimestamp, GetTimestamp());

    /// <summary>
    /// 计算秒表时间戳之间的时间跨度
    /// </summary>
    /// <param name="startingTimestamp">起始时间戳</param>
    /// <param name="endingTimestamp">结束时间戳</param>
    /// <returns>时间跨度</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
        new((long)((endingTimestamp - startingTimestamp) * TimestampToTicks));

    /// <summary>
    /// 构造已停止的秒表实例
    /// </summary>
    /// <param name="start">起始时间戳</param>
    /// <param name="end">结束时间戳</param>
    /// <returns>秒表</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueStopwatch FromTimestamp(long start, long end) => new(-(end - start));

    #endregion
}