using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace UltraTool.Extensions;

/// <summary>
/// 可空类型拓展类
/// </summary>
[PublicAPI]
public static class NullableExtensions
{
    /// <summary>
    /// 调用参数必须为非null，否则抛出异常
    /// </summary>
    /// <param name="value">可空类型值</param>
    /// <param name="paramName">参数名，默认自动生成</param>
    /// <returns>非null值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T RequireNonNull<T>([NotNull] this T? value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : class =>
        value ?? throw new ArgumentNullException(paramName ?? nameof(value), "传入参数不能为空");

    /// <summary>
    /// 调用参数必须为非null，否则抛出异常
    /// </summary>
    /// <param name="value">可空类型值</param>
    /// <param name="paramName">参数名，默认自动生成</param>
    /// <returns>非null值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T RequireNonNull<T>([NotNull] this T? value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : struct =>
        value ?? throw new ArgumentNullException(paramName ?? nameof(value), "传入参数不能为空");
}