using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Numerics;

/// <summary>
/// 整型拓展类
/// </summary>
[PublicAPI]
public static class IntegerExtensions
{
    /// <summary>
    /// 判断整数是否为奇数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>是否为奇数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd(this int number) => (number & 1) == 1;

    /// <summary>
    /// 判断整数是否为偶数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>是否为偶数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(this int number) => (number & 1) == 0;

#if NET7_0_OR_GREATER
    /// <summary>
    /// 判断整数是否为奇数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>是否为奇数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOdd<T>(this T number) where T : IBinaryInteger<T> => (number & T.One) == T.One;

    /// <summary>
    /// 判断整数是否为偶数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>是否为偶数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven<T>(this T number) where T : IBinaryInteger<T> => (number & T.One) == T.Zero;
#endif
}