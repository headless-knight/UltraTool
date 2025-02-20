﻿using System.Runtime.CompilerServices;

namespace UltraTool.Helpers;

/// <summary>
/// 参数越界帮助类
/// </summary>
internal static class ArgumentOutOfRangeHelper
{
    /// <summary>
    /// 如果值小于0则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="paramName">参数名</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(int value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) => ThrowIfLessThan(value, 0, paramName);

    /// <summary>
    /// 如果值小于0则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="paramName">参数名</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(float value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) => ThrowIfLessThan(value, 0, paramName);

    /// <summary>
    /// 如果值小于0则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="paramName">参数名</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegative(double value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) => ThrowIfLessThan(value, 0, paramName);

    /// <summary>
    /// 如果值小于或等于0则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="paramName">参数名</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNegativeOrZero(int value,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) => ThrowIfLessThanOrEqual(value, 0, paramName);

    /// <summary>
    /// 如果值小于目标值则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <param name="paramName">参数名</param>
    public static void ThrowIfLessThan<T>(T value, T other,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : IComparable<T>
    {
        if (value.CompareTo(other) >= 0) return;

        throw new ArgumentOutOfRangeException(paramName, value, $"The value must be greater than or equal {other}");
    }

    /// <summary>
    /// 如果值小于目标值则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <param name="paramName">参数名</param>
    public static void ThrowIfLessThanOrEqual<T>(T value, T other,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : IComparable<T>
    {
        if (value.CompareTo(other) >= 0) return;

        throw new ArgumentOutOfRangeException(paramName, value, $"The value must be greater than {other}");
    }

    /// <summary>
    /// 如果值大于目标值则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <param name="paramName">参数名</param>
    public static void ThrowIfGreaterThan<T>(T value, T other,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : IComparable<T>
    {
        if (value.CompareTo(other) <= 0) return;

        throw new ArgumentOutOfRangeException(paramName, value, $"The value must be less than or equal {other}");
    }

    /// <summary>
    /// 如果值大于或等于目标值则抛出异常
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="other">目标值</param>
    /// <param name="paramName">参数名</param>
    public static void ThrowIfGreaterThanOrEqual<T>(T value, T other,
#if NETCOREAPP3_0_OR_GREATER
        [CallerArgumentExpression(nameof(value))]
#endif
        string? paramName = null) where T : IComparable<T>
    {
        if (value.CompareTo(other) < 0) return;

        throw new ArgumentOutOfRangeException(paramName, value, $"The value must be less than {other}");
    }
}