using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool;

/// <summary>
/// 组合处置器
/// </summary>
[PublicAPI]
public sealed class CompositeDisposer : IDisposable
{
    private readonly IDisposable[] _disposables;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="disposables">可处置对象序列</param>
    public CompositeDisposer([InstantHandle] IEnumerable<IDisposable> disposables)
    {
        _disposables = disposables.ToArray();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    /// <summary>
    /// 创建组合处置器
    /// </summary>
    /// <param name="disposables">可处置对象数组</param>
    /// <returns>组合处置器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CompositeDisposer Create(params IDisposable[] disposables) => new(disposables);
}

/// <summary>
/// 异步组合处置器
/// </summary>
[PublicAPI]
public sealed class AsyncCompositeDisposer : IAsyncDisposable
{
    private readonly IAsyncDisposable[] _disposables;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="disposables">可处置对象序列</param>
    public AsyncCompositeDisposer([InstantHandle] IEnumerable<IAsyncDisposable> disposables)
    {
        _disposables = disposables.ToArray();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        foreach (var disposable in _disposables)
        {
            await disposable.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 创建异步组合处置器
    /// </summary>
    /// <param name="disposables">可处置对象数组</param>
    /// <returns>异步组合处置器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AsyncCompositeDisposer Create(params IAsyncDisposable[] disposables) => new(disposables);
}