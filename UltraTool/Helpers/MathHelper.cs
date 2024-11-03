using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Helpers;

/// <summary>
/// 数学帮助类
/// </summary>
[PublicAPI]
public static class MathHelper
{
    /// <summary>long范围内的阶乘结果</summary>
    private static readonly long[] Factorials =
    [
        1L, 1L, 2L, 6L, 24L, 120L, 720L, 5040L, 40320L, 362880L, 3628800L, 39916800L, 479001600L, 6227020800L,
        87178291200L, 1307674368000L, 20922789888000L, 355687428096000L, 6402373705728000L, 121645100408832000L,
        2432902008176640000L
    ];

    /// <summary>
    /// 返回两个值中的最小值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最小值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Min<T>(in T value1, in T value2) where T : IComparable<T> =>
        value1.CompareTo(value2) < 0 ? value1 : value2;

    /// <summary>
    /// 返回两个值中的最大值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最大值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Max<T>(in T value1, in T value2) where T : IComparable<T> =>
        value1.CompareTo(value2) > 0 ? value1 : value2;

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <returns>中间值</returns>
    [Pure]
    public static T Middle<T>(in T value1, in T value2, in T value3) where T : IComparable<T>
    {
        if (value1.CompareTo(value2) < 0)
        {
            if (value3.CompareTo(value2) > 0) return value2;

            return value3.CompareTo(value1) < 0 ? value1 : value3;
        }

        if (value3.CompareTo(value1) > 0) return value1;

        return value3.CompareTo(value2) < 0 ? value2 : value3;
    }

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <param name="comparer">比较器</param>
    /// <returns>中间值</returns>
    [Pure]
    public static T Middle<T>(in T value1, in T value2, in T value3, IComparer<T> comparer)
    {
        if (comparer.Compare(value1, value2) < 0)
        {
            if (comparer.Compare(value3, value2) > 0) return value2;

            return comparer.Compare(value3, value1) < 0 ? value1 : value3;
        }

        if (comparer.Compare(value3, value1) > 0) return value1;

        return comparer.Compare(value3, value2) < 0 ? value2 : value3;
    }

    /// <summary>
    /// 传入的两个值，按(小值,大值)返回
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>(小值,大值)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (T Min, T Max) MinMax<T>(in T value1, in T value2) where T : IComparable<T> =>
        value1.CompareTo(value2) < 0 ? (value1, value2) : (value2, value1);

    /// <summary>
    /// 计算数n的阶乘
    /// </summary>
    /// <param name="n">数n</param>
    /// <returns>数n的阶乘</returns>
    [Pure]
    public static long Factorial(int n)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(n);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(n, Factorials.Length);
        return Factorials[n];
    }

    /// <summary>
    /// 计算最大公约数
    /// </summary>
    /// <param name="a">值1</param>
    /// <param name="b">值2</param>
    /// <returns>最大公约数</returns>
    [Pure]
    public static int Gcd(int a, int b)
    {
        var c = a % b;
        while (c != 0)
        {
            a = b;
            b = c;
            c = a % b;
        }

        return b;
    }

    /// <summary>
    /// 计算最小公倍数
    /// </summary>
    /// <param name="a">值1</param>
    /// <param name="b">值2</param>
    /// <returns>最小公倍数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Multiple(int a, int b) => a / Gcd(a, b) * b;

#if NET7_0_OR_GREATER
    /// <summary>
    /// 计算最大公约数
    /// </summary>
    /// <param name="a">值1</param>
    /// <param name="b">值2</param>
    /// <returns>最大公约数</returns>
    [Pure]
    public static T Gcd<T>(T a, T b) where T : IBinaryInteger<T>
    {
        var c = a % b;
        while (c != T.Zero)
        {
            a = b;
            b = c;
            c = a % b;
        }

        return b;
    }

    /// <summary>
    /// 计算最小公倍数
    /// </summary>
    /// <param name="a">值1</param>
    /// <param name="b">值2</param>
    /// <returns>最小公倍数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Multiple<T>(T a, T b) where T : IBinaryInteger<T> => a / Gcd(a, b) * b;
#endif
}