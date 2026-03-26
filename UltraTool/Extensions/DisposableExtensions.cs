using System.Runtime.CompilerServices;

namespace UltraTool.Extensions;

/// <summary>
/// 可处置对象拓展类
/// </summary>
public static class DisposableExtensions
{
    /// <summary>
    /// 尝试将对象处置
    /// </summary>
    /// <param name="value">对象</param>
    /// <returns>是否成功处置</returns>
    public static bool TryDepose<T>(this T value)
    {
        if (value is not IDisposable disposable) return false;

        disposable.Dispose();
        return true;
    }

    /// <summary>
    /// 若对象不为空则进行处置，否则返回完成任务
    /// </summary>
    /// <param name="disposable">可处置对象</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask DisposeIfNotNullAsync<T>(this T? disposable) where T : IAsyncDisposable =>
        disposable?.DisposeAsync() ?? new ValueTask();
}