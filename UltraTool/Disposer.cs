using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool;

/// <summary>
/// 处置器
/// </summary>
/// <param name="disposer">处置委托</param>
[PublicAPI]
public sealed class Disposer(Action disposer) : IDisposable
{
    private int _disposeFlag;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposeFlag, 1, 0) == 0) disposer.Invoke();
    }
}

/// <summary>
/// 处置器
/// </summary>
/// <param name="disposer">处置委托，入参(状态)</param>
/// <param name="state">状态</param>
[PublicAPI]
public sealed class Disposer<T>(Action<T> disposer, T state) : IDisposable
{
    private int _disposeFlag;

    /// <summary>
    /// 状态
    /// </summary>
    public T State => state;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposeFlag, 1, 0) == 0) disposer.Invoke(state);
    }
}

/// <summary>
/// 异步处置器
/// </summary>
/// <param name="disposer">异步处置委托</param>
[PublicAPI]
public sealed class AsyncDisposer(Func<ValueTask> disposer) : IAsyncDisposable
{
    private int _disposeFlag;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync() => Interlocked.CompareExchange(ref _disposeFlag, 1, 0) != 0
        ? new ValueTask()
        : disposer.Invoke();
}

/// <summary>
/// 异步处置器
/// </summary>
/// <param name="disposer">异步处置委托，入参(状态)</param>
/// <param name="state">状态</param>
[PublicAPI]
public sealed class AsyncDisposer<T>(Func<T, ValueTask> disposer, T state) : IAsyncDisposable
{
    private int _disposeFlag;

    /// <summary>
    /// 状态
    /// </summary>
    public T State => state;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync() => Interlocked.CompareExchange(ref _disposeFlag, 1, 0) != 0
        ? new ValueTask()
        : disposer.Invoke(state);
}

/// <summary>
/// 值类型处置器
/// </summary>
/// <param name="disposer">处置委托</param>
[PublicAPI]
public struct ValueDisposer(Action disposer) : IDisposable
{
    private int _disposeFlag;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposeFlag, 1, 0) == 0) disposer.Invoke();
    }
}

/// <summary>
/// 值类型处置器
/// </summary>
/// <param name="disposer">处置委托，入参(状态)</param>
/// <param name="state">状态</param>
[PublicAPI]
public struct ValueDisposer<T>(Action<T> disposer, T state) : IDisposable
{
    private int _disposeFlag;

    /// <summary>
    /// 状态
    /// </summary>
    public readonly T State => state;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposeFlag, 1, 0) == 0) disposer.Invoke(state);
    }
}

/// <summary>
/// 值类型异步处置器
/// </summary>
/// <param name="disposer">异步处置委托</param>
[PublicAPI]
public struct ValueAsyncDisposer(Func<ValueTask> disposer) : IAsyncDisposable
{
    private int _disposeFlag;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync() => Interlocked.CompareExchange(ref _disposeFlag, 1, 0) != 0
        ? new ValueTask()
        : disposer.Invoke();
}

/// <summary>
/// 值类型异步处置器
/// </summary>
/// <param name="disposer">异步处置委托，入参(状态)</param>
/// <param name="state">状态</param>
[PublicAPI]
public struct ValueAsyncDisposer<T>(Func<T, ValueTask> disposer, T state) : IAsyncDisposable
{
    private int _disposeFlag;

    /// <summary>
    /// 状态
    /// </summary>
    public readonly T State => state;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync() => Interlocked.CompareExchange(ref _disposeFlag, 1, 0) != 0
        ? new ValueTask()
        : disposer.Invoke(state);
}