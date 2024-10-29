using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Numerics;

/// <summary>
/// 浮点型扩展
/// </summary>
[PublicAPI]
public static class FloatExtensions
{
    /// <summary>默认误差</summary>
    private const float DefaultTolerance = 0.0001F;

    /// <summary>Decimal默认误差</summary>
    private const decimal DefaultToleranceDecimal = 0.0001M;

    /// <summary>
    /// 判断两个浮点数是否近似相等
    /// </summary>
    /// <param name="number">当前值</param>
    /// <param name="other">另一个值</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultTolerance"/></param>
    /// <returns>是否近似相等</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateTo(this float number, float other, float tolerance = DefaultTolerance) =>
        Math.Abs(number - other) < tolerance;

    /// <summary>
    /// 判断两个浮点数是否近似相等
    /// </summary>
    /// <param name="number">当前值</param>
    /// <param name="other">另一个值</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultTolerance"/></param>
    /// <returns>是否近似相等</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateTo(this double number, double other, double tolerance = DefaultTolerance) =>
        Math.Abs(number - other) < tolerance;

    /// <summary>
    /// 判断两个浮点数是否近似相等
    /// </summary>
    /// <param name="number">当前值</param>
    /// <param name="other">另一个值</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultToleranceDecimal"/></param>
    /// <returns>是否近似相等</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateTo(this decimal number, decimal other, decimal tolerance = DefaultToleranceDecimal) =>
        Math.Abs(number - other) < tolerance;

    /// <summary>
    /// 判断浮点数是否近似等于0
    /// </summary>
    /// <param name="number">浮点数</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultTolerance"/></param>
    /// <returns>是否近似等于0</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateToZero(this float number, float tolerance = DefaultTolerance) =>
        number.ApproximateTo(0, tolerance);

    /// <summary>
    /// 判断浮点数是否近似等于0
    /// </summary>
    /// <param name="number">浮点数</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultTolerance"/></param>
    /// <returns>是否近似等于0</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateToZero(this double number, double tolerance = DefaultTolerance) =>
        number.ApproximateTo(0, tolerance);

    /// <summary>
    /// 判断浮点数是否近似等于0
    /// </summary>
    /// <param name="number">浮点数</param>
    /// <param name="tolerance">误差，默认为<see cref="DefaultToleranceDecimal"/></param>
    /// <returns>是否近似等于0</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateToZero(this decimal number, decimal tolerance = DefaultToleranceDecimal) =>
        number.ApproximateTo(0, tolerance);
}