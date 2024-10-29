#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool;

/// <summary>
/// 异常构建器
/// </summary>
[PublicAPI]
public interface IExceptionBuilder
{
    /// <summary>
    /// 是否有错误信息
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// 添加错误信息
    /// </summary>
    /// <param name="error">错误信息</param>
    void AddError(string error);

    /// <summary>
    /// 获取错误信息字符串
    /// </summary>
    /// <returns>错误信息</returns>
    string GetErrorString();

    /// <summary>
    /// 如果有错误信息，抛出异常
    /// </summary>
    void ThrowIfHasError<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
        T>() where T : Exception
    {
        if (!HasError) return;

        var exception = (Exception)Activator.CreateInstance(typeof(T), GetErrorString())!;
        throw exception;
    }

    /// <summary>
    /// 如果有错误信息，抛出异常
    /// </summary>
    /// <param name="creator">异常构造委托，入参(错误信息)</param>
    void ThrowIfHasError<T>(Func<string, T> creator) where T : Exception
    {
        if (!HasError) return;

        var exception = creator.Invoke(GetErrorString());
        throw exception;
    }
}

/// <summary>
/// 异常构建器
/// </summary>
[PublicAPI]
public interface IExceptionBuilder<out T> : IExceptionBuilder where T : Exception
{
    /// <summary>
    /// 构建异常
    /// </summary>
    /// <returns>异常</returns>
    T Build();

    /// <summary>
    /// 如果有错误信息，抛出异常
    /// </summary>
    void ThrowIfHasError();
}

/// <summary>
/// 异常构建器静态类
/// </summary>
[PublicAPI]
public static class ExceptionBuilder
{
    /// <summary>
    /// 创建异常构建器
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <param name="creator">异常构造委托，入参(错误信息)</param>
    /// <returns>异常构建器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ExceptionBuilder<T> Create<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
        T>(string? title = null, Func<string, T>? creator = null) where T : Exception => new(title, creator);

    /// <summary>
    /// 创建默认异常构建器
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <returns>异常构建器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ExceptionBuilder<Exception> CreateDefault(string? title = null) =>
        Create<Exception>(title, static error => new Exception(error));
}

/// <summary>
/// 异常构建器
/// </summary>
/// <remarks>当调用<see cref="IDisposable.Dispose"/>时，将调用<see cref="IExceptionBuilder{T}.ThrowIfHasError"/></remarks>
[PublicAPI]
public sealed class ExceptionBuilder<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
    T> : IExceptionBuilder<T>, IDisposable where T : Exception
{
    private readonly string? _title;
    private readonly Func<string, T>? _creator;
    private List<string>? _errors;

    /// <inheritdoc />
    public bool HasError => _errors is { Count: > 0 };

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <param name="creator">异常构造委托，入参(错误信息)，默认为null</param>
    public ExceptionBuilder(string? title = null, Func<string, T>? creator = null)
    {
        _title = title;
        _creator = creator;
    }

    /// <inheritdoc />
    public void AddError(string error)
    {
        var errors = _errors ??= [];
        errors.Add($"error: {error}");
    }

    /// <inheritdoc />
    public string GetErrorString()
    {
        if (_errors is not { Count: > 0 }) return string.Empty;

        var errors = string.Join('\n', _errors);
        return string.IsNullOrWhiteSpace(_title) ? errors : $"{_title}\n{errors}";
    }

    /// <inheritdoc />
    public T Build()
    {
        var message = GetErrorString();
        return _creator == null ? (T)Activator.CreateInstance(typeof(T), message)! : _creator.Invoke(message);
    }

    /// <inheritdoc />
    public void ThrowIfHasError()
    {
        if (!HasError) return;

        var exception = Build();
        throw exception;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => ThrowIfHasError();
}

/// <summary>
/// 值类型异常构建器静态类
/// </summary>
[PublicAPI]
public static class ValueExceptionBuilder
{
    /// <summary>
    /// 创建值类型异常构建器
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <param name="creator">异常构造委托，入参(错误信息)</param>
    /// <returns>异常构建器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueExceptionBuilder<T> Create<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
        T>(string? title = null, Func<string, T>? creator = null) where T : Exception => new(title, creator);

    /// <summary>
    /// 创建默认值类型异常构建器
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <returns>异常构建器</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueExceptionBuilder<Exception> CreateDefault(string? title = null) =>
        Create<Exception>(title, static error => new Exception(error));
}

/// <summary>
/// 值类型异常构建器
/// </summary>
/// <remarks>当调用<see cref="IDisposable.Dispose"/>时，将调用<see cref="IExceptionBuilder{T}.ThrowIfHasError"/></remarks>
[PublicAPI]
public struct ValueExceptionBuilder<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
#endif
    T> : IExceptionBuilder<T>, IDisposable where T : Exception
{
    private readonly string? _title;
    private readonly Func<string, T>? _creator;
    private List<string>? _errors;

    /// <inheritdoc />
    public readonly bool HasError => _errors is { Count: > 0 };

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="title">标题，默认为null</param>
    /// <param name="creator">异常构造委托，入参(错误信息)，默认为null</param>
    public ValueExceptionBuilder(string? title = null, Func<string, T>? creator = null)
    {
        _title = title;
        _creator = creator;
    }

    /// <inheritdoc />
    public void AddError(string error)
    {
        var errors = _errors ??= [];
        errors.Add($"error: {error}");
    }

    /// <inheritdoc />
    public readonly string GetErrorString()
    {
        if (_errors is not { Count: > 0 }) return string.Empty;

        var errors = string.Join('\n', _errors);
        return string.IsNullOrWhiteSpace(_title) ? errors : $"{_title}\n{errors}";
    }

    /// <inheritdoc />
    public readonly T Build()
    {
        var message = GetErrorString();
        return _creator == null ? (T)Activator.CreateInstance(typeof(T), message)! : _creator.Invoke(message);
    }

    /// <inheritdoc />
    public readonly void ThrowIfHasError()
    {
        if (!HasError) return;

        var exception = Build();
        throw exception;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose() => ThrowIfHasError();
}