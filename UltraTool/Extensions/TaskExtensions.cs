using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;

namespace UltraTool.Extensions;

/// <summary>
/// 任务拓展类
/// </summary>
[PublicAPI]
public static class TaskExtensions
{
    /// <summary>
    /// 忽略任务返回值
    /// </summary>
    /// <param name="task">任务</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IgnoreResult(this Task task) => TaskHelper.IgnoreResult(task);

    /// <summary>
    /// 忽略任务返回值
    /// </summary>
    /// <param name="task">任务</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IgnoreResult(this ValueTask task) => TaskHelper.IgnoreResult(task);

    /// <summary>
    /// 忽略任务异常
    /// </summary>
    /// <param name="task">源任务</param>
    /// <returns>包装后的任务</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task IgnoreException(this Task task) => TaskHelper.IgnoreException(task);

    /// <summary>
    /// 忽略任务异常
    /// </summary>
    /// <param name="task">源任务</param>
    /// <returns>包装后的任务</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask IgnoreException(this ValueTask task) => TaskHelper.IgnoreException(task);

    /// <summary>
    /// 获取任务结果
    /// </summary>
    /// <param name="task">任务</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GetResult(this ValueTask task)
    {
        if (!task.IsCompleted) task.AsTask().GetAwaiter().GetResult();
    }

    /// <summary>
    /// 获取任务结果
    /// </summary>
    /// <param name="task">任务</param>
    /// <returns>任务结果</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetResult<T>(this ValueTask<T> task) =>
        task.IsCompleted ? task.Result : task.AsTask().GetAwaiter().GetResult();

    /// <summary>
    /// 当任意任务完成时
    /// </summary>
    /// <param name="tasks">任务序列</param>
    /// <returns>等待任务</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task WhenAny(this IEnumerable<Task> tasks) => Task.WhenAny(tasks);

    /// <summary>
    /// 当全部任务完成时
    /// </summary>
    /// <param name="tasks">任务序列</param>
    /// <returns>等待任务</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task WhenAll(this IEnumerable<Task> tasks) => Task.WhenAll(tasks);

    /// <summary>
    /// 等待全部任务返回值
    /// </summary>
    /// <param name="tasks">任务序列</param>
    /// <returns>返回值数组</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<T[]> AwaitAll<T>(this IEnumerable<Task<T>> tasks) => TaskHelper.AwaitAll(tasks);

    /// <summary>
    /// 等待全部任务返回值
    /// </summary>
    /// <param name="tasks">{键:任务}序列</param>
    /// <returns>{键:返回值}数组</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<KeyValuePair<TKey, TValue>[]> AwaitAll<TKey, TValue>(
        [InstantHandle] this IEnumerable<KeyValuePair<TKey, Task<TValue>>> tasks) where TKey : notnull =>
        TaskHelper.AwaitAll(tasks);

    /// <summary>
    /// 等待全部任务返回值
    /// </summary>
    /// <param name="tasks">{键:任务}字典</param>
    /// <returns>{键:返回值}字典</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Dictionary<TKey, TValue>> AwaitAll<TKey, TValue>(
        IReadOnlyDictionary<TKey, Task<TValue>> tasks) where TKey : notnull => TaskHelper.AwaitAll(tasks);

    /// <summary>
    /// 当全部值任务完成时
    /// </summary>
    /// <param name="tasks">值任务序列</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask WhenAll([InstantHandle] this IEnumerable<ValueTask> tasks) => TaskHelper.WhenAll(tasks);

    /// <summary>
    /// 当全部值任务完成时
    /// </summary>
    /// <param name="tasks">值任务序列</param>
    /// <returns>结果数组</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T[]> AwaitAll<T>([InstantHandle] this IEnumerable<ValueTask<T>> tasks) =>
        TaskHelper.AwaitAll(tasks);
}