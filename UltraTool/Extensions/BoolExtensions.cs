using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Extensions;

/// <summary>
/// 布尔拓展类
/// </summary>
[PublicAPI]
public static class BoolExtensions
{
    /// <summary>
    /// 计算布尔序列中为True的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>True的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountTrue([InstantHandle] this IEnumerable<bool> iter) => iter.Count(static item => item);

    /// <summary>
    /// 计算布尔序列中为True的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>True的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountTrue([InstantHandle] this IEnumerable<bool?> iter) =>
        iter.Count(static item => item is true);

    /// <summary>
    /// 计算布尔序列中为False的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>False的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountFalse([InstantHandle] this IEnumerable<bool> iter) => iter.Count(static item => !item);

    /// <summary>
    /// 计算布尔序列中为False的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>False的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountFalse([InstantHandle] this IEnumerable<bool?> iter) =>
        iter.Count(static item => item is false);

    /// <summary>
    /// 计算布尔序列中为True的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>True的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountNotTrue([InstantHandle] this IEnumerable<bool?> iter) =>
        iter.Count(static item => item is not true);

    /// <summary>
    /// 计算布尔序列中为False的数量
    /// </summary>
    /// <param name="iter">布尔序列</param>
    /// <returns>False的数量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountNotFalse([InstantHandle] this IEnumerable<bool?> iter) =>
        iter.Count(static item => item is not false);
}