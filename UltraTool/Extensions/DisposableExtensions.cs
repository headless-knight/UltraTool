using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Extensions;

/// <summary>
/// 可处置对象拓展类
/// </summary>
[PublicAPI]
public static class DisposableExtensions
{
    /// <summary>
    /// 尝试将对象转为可处置对象进行处置
    /// </summary>
    /// <param name="value">对象</param>
    /// <returns>是否可转换处置对象</returns>
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