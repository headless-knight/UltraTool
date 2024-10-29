using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Collections;

namespace UltraTool.Helpers;

/// <summary>
/// 任务帮助类
/// </summary>
[PublicAPI]
public static class TaskHelper
{
    /// <summary>
    /// 忽略任务异常处理委托
    /// </summary>
    /// <param name="exception">异常</param>
    public delegate void ExceptionHandler(Exception exception);

    /// <summary>
    ///  捕获忽略任务异常事件
    /// </summary>
    public static event ExceptionHandler? IgnoreExceptionCaught;

    /// <summary>
    /// 忽略任务结果
    /// </summary>
    /// <param name="task">任务</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IgnoreResult(Task task) => task.ContinueWith(it =>
    {
        if (it.Exception == null) return;

        IgnoreExceptionCaught?.Invoke(it.Exception);
    });

    /// <summary>
    /// 忽略任务结果
    /// </summary>
    /// <param name="task">任务</param>
    public static async void IgnoreResult(ValueTask task)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            IgnoreExceptionCaught?.Invoke(e);
        }
    }

    /// <summary>
    /// 忽略任务异常
    /// </summary>
    /// <param name="task">源任务</param>
    /// <returns>包装后的任务</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task IgnoreException(Task task) => task.ContinueWith(it =>
    {
        if (it.Exception == null) return;

        IgnoreExceptionCaught?.Invoke(it.Exception);
    });

    /// <summary>
    /// 忽略任务异常
    /// </summary>
    /// <param name="task">源任务</param>
    /// <returns>包装后的任务</returns>
    public static async ValueTask IgnoreException(ValueTask task)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            IgnoreExceptionCaught?.Invoke(e);
        }
    }

    /// <summary>
    /// 等待两个任务的返回结果
    /// </summary>
    /// <param name="task1">任务1</param>
    /// <param name="task2">任务2</param>
    /// <returns>(结果1,结果2)</returns>
    public static async Task<(T1, T2)> AwaitAll<T1, T2>(Task<T1> task1, Task<T2> task2)
    {
        await Task.WhenAll(task1, task2).ConfigureAwait(false);
        return (task1.Result, task2.Result);
    }

    /// <summary>
    /// 等待三个任务的返回结果
    /// </summary>
    /// <param name="task1">任务1</param>
    /// <param name="task2">任务2</param>
    /// <param name="task3">任务3</param>
    /// <returns>(结果1,结果2,任务3)</returns>
    public static async Task<(T1, T2, T3)> AwaitAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
    {
        await Task.WhenAll(task1, task2, task3).ConfigureAwait(false);
        return (task1.Result, task2.Result, task3.Result);
    }

    /// <summary>
    /// 等待多个任务的返回结果
    /// </summary>
    /// <param name="tasks">任务数组</param>
    /// <returns>结果数组</returns>
    public static async Task<T[]> AwaitAll<T>(params Task<T>[] tasks)
    {
        if (tasks is not { Length: > 0 }) return [];

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return tasks.Select(task => task.Result).ToArray();
    }

    /// <summary>
    /// 等待多个任务的返回结果
    /// </summary>
    /// <param name="tasks">任务序列</param>
    /// <returns>结果数组</returns>
    public static async Task<T[]> AwaitAll<T>([InstantHandle] IEnumerable<Task<T>> tasks)
    {
        if (tasks.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return [];
        }

        switch (tasks)
        {
            case IReadOnlyCollection<Task<T>> taskColl:
            {
                await Task.WhenAll(taskColl).ConfigureAwait(false);
                return taskColl.Select(task => task.Result).ToArray();
            }
            case ICollection<Task<T>> taskColl:
            {
                await Task.WhenAll(taskColl).ConfigureAwait(false);
                return taskColl.Select(task => task.Result).ToArray();
            }
            default:
            {
                var taskArray = tasks.ToArray();
                await Task.WhenAll(taskArray).ConfigureAwait(false);
                return taskArray.Select(task => task.Result).ToArray();
            }
        }
    }

    /// <summary>
    /// 等待多个任务键值对的返回结果
    /// </summary>
    /// <param name="tasks">{键:任务}序列</param>
    /// <returns>{键:任务结果}数组</returns>
    public static async Task<KeyValuePair<TKey, TValue>[]> AwaitAll<TKey, TValue>(
        [InstantHandle] IEnumerable<KeyValuePair<TKey, Task<TValue>>> tasks) where TKey : notnull
    {
        if (tasks.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return [];
        }

        switch (tasks)
        {
            case IReadOnlyCollection<KeyValuePair<TKey, Task<TValue>>> taskColl:
            {
                await Task.WhenAll(taskColl.Select(pair => pair.Value)).ConfigureAwait(false);
                return taskColl.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.Result)).ToArray();
            }
            case ICollection<KeyValuePair<TKey, Task<TValue>>> taskColl:
            {
                await Task.WhenAll(taskColl.Select(pair => pair.Value)).ConfigureAwait(false);
                return taskColl.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.Result)).ToArray();
            }
            default:
            {
                var taskArray = tasks.ToArray();
                await Task.WhenAll(taskArray.Select(pair => pair.Value)).ConfigureAwait(false);
                return taskArray.Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value.Result)).ToArray();
            }
        }
    }

    /// <summary>
    /// 等待多个任务的返回结果
    /// </summary>
    /// <param name="tasks">{键:任务}字典</param>
    /// <returns>{键:返回结果}字典</returns>
    public static async Task<Dictionary<TKey, TValue>> AwaitAll<TKey, TValue>(
        IReadOnlyDictionary<TKey, Task<TValue>> tasks) where TKey : notnull
    {
        if (tasks is not { Count: > 0 }) return [];

        await Task.WhenAll(tasks.Values).ConfigureAwait(false);
        var result = new Dictionary<TKey, TValue>(tasks.Count);
        foreach (var (key, task) in tasks)
        {
            result.Add(key, task.Result);
        }

        return result;
    }

    /// <summary>
    /// 当全部值任务完成时
    /// </summary>
    /// <param name="tasks">值任务数组</param>
    public static async ValueTask WhenAll(params ValueTask[] tasks)
    {
        if (tasks is not { Length: > 0 }) return;

        foreach (var task in tasks)
        {
            await task.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 当全部值任务完成时
    /// </summary>
    /// <param name="tasks">值任务序列</param>
    public static async ValueTask WhenAll([InstantHandle] IEnumerable<ValueTask> tasks)
    {
        if (tasks.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        switch (tasks)
        {
            case ICollection<ValueTask> taskColl:
            {
                foreach (var task in taskColl)
                {
                    await task.ConfigureAwait(false);
                }

                break;
            }
            case IReadOnlyCollection<ValueTask> taskColl:
            {
                foreach (var task in taskColl)
                {
                    await task.ConfigureAwait(false);
                }

                break;
            }
            default:
            {
                var taskArray = tasks.ToArray();
                foreach (var task in taskArray)
                {
                    await task.ConfigureAwait(false);
                }

                break;
            }
        }
    }

    /// <summary>
    /// 等待多个值任务结果
    /// </summary>
    /// <param name="tasks">值任务数组</param>
    /// <returns>结果数组</returns>
    public static async ValueTask<T[]> AwaitAll<T>(params ValueTask<T>[] tasks)
    {
        if (tasks is not { Length: > 0 }) return [];

        var result = ArrayHelper.AllocateUninitializedArray<T>(tasks.Length);
        for (var i = 0; i < tasks.Length; i++)
        {
            result[i] = await tasks[i].ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// 当全部值任务完成时
    /// </summary>
    /// <param name="tasks">值任务序列</param>
    /// <returns>结果数组</returns>
    public static async ValueTask<T[]> AwaitAll<T>([InstantHandle] IEnumerable<ValueTask<T>> tasks)
    {
        if (tasks.TryGetNonEnumeratedCount(out var size))
        {
            if (size <= 0) return [];

            var result = ArrayHelper.AllocateUninitializedArray<T>(size);
            var index = 0;
            foreach (var task in tasks)
            {
                result[index++] = await task.ConfigureAwait(false);
            }

            return result;
        }

        var taskArray = tasks.ToArray();
        var resultArray = ArrayHelper.AllocateUninitializedArray<T>(taskArray.Length);
        for (var i = 0; i < taskArray.Length; i++)
        {
            resultArray[i] = await taskArray[i].ConfigureAwait(false);
        }

        return resultArray;
    }
}