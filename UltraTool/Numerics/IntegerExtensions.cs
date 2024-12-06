#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;

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

    /// <summary>
    /// 获取整数二进制中1的个数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>1的个数</returns>
    [Pure]
    public static int GetBitOneCount(this int number)
    {
        var count = 0;
        while (number != 0)
        {
            count++;
            number &= (number - 1);
        }

        return count;
    }

    /// <summary>
    /// 判断整数指定索引位是否为1
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>是否为1</returns>
    [Pure]
    public static bool IsBitOne(this int number, int index)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, 32);
        return (number & (1 << index)) != 0;
    }

    /// <summary>
    /// 计算出将整数指定索引位设置为1的新值
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>新值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalcSetBitOne(this int number, int index)
    {
        IntegerHelper.SetBitOne(ref number, index);
        return number;
    }

    /// <summary>
    /// 计算出将整数指定索引位设置为0的新值
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>新值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalcSetBitZero(this int number, int index)
    {
        IntegerHelper.SetBitZero(ref number, index);
        return number;
    }

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

    /// <summary>
    /// 获取整数的二进制位数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>二进制位数</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetBitCount<T>(this T number) where T : IBinaryInteger<T> =>
        number.GetByteCount() * 8;

    /// <summary>
    /// 获取整数二进制中1的个数
    /// </summary>
    /// <param name="number">整数</param>
    /// <returns>1的个数</returns>
    [Pure]
    public static int GetBitOneCount<T>(this T number) where T : IBinaryInteger<T>
    {
        var count = 0;
        while (number != T.Zero)
        {
            count++;
            number &= (number - T.One);
        }

        return count;
    }

    /// <summary>
    /// 判断整数指定索引位是否为1
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>是否为1</returns>
    [Pure]
    public static bool IsBitOne<T>(this T number, int index) where T : IBinaryInteger<T>
    {
        var count = number.GetBitCount();
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, count);
        return (number & (T.One << index)) != T.Zero;
    }

    /// <summary>
    /// 计算出将整数指定索引位设置为1的新值
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>新值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T CalcSetBitOne<T>(this T number, int index) where T : IBinaryInteger<T>
    {
        IntegerHelper.SetBitOne(ref number, index);
        return number;
    }

    /// <summary>
    /// 计算出将整数指定索引位设置为0的新值
    /// </summary>
    /// <param name="number">整数</param>
    /// <param name="index">位索引</param>
    /// <returns>新值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T CalcSetBitZero<T>(this T number, int index) where T : IBinaryInteger<T>
    {
        IntegerHelper.SetBitZero(ref number, index);
        return number;
    }
#endif
}