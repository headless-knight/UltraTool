using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Numerics;

/// <summary>
/// 整型帮助类
/// </summary>
[PublicAPI]
public static class IntegerHelper
{
    /// <summary>
    /// 两个有符号字节拼接为短整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short SpliceToShort(sbyte upper, sbyte lower) =>
        (short)((upper << 8) | (byte)lower);

    /// <summary>
    /// 两个字节拼接为短整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower"></param>
    /// <returns></returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short SpliceToShort(byte upper, byte lower) =>
        (short)((upper << 8) | lower);

    /// <summary>
    /// 两个有符号字节拼接为无符号短整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort SpliceToUShort(sbyte upper, sbyte lower) =>
        (ushort)((upper << 8) | (byte)lower);

    /// <summary>
    /// 两个字节拼接为无符号短整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort SpliceToUShort(byte upper, byte lower) =>
        (ushort)((upper << 8) | lower);

    /// <summary>
    /// 两个短整型拼接为整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SpliceToInt(short upper, short lower) =>
        (upper << 16) | (ushort)lower;

    /// <summary>
    /// 两个无符号短整型拼接为整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SpliceToInt(ushort upper, ushort lower) =>
        (upper << 16) | lower;

    /// <summary>
    /// 两个短整型拼接为无符号整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint SpliceToUInt(short upper, short lower) =>
        ((uint)upper << 16) | (ushort)lower;

    /// <summary>
    /// 两个无符号短整型拼接为无符号整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint SpliceToUInt(ushort upper, ushort lower) =>
        ((uint)upper << 16) | lower;

    /// <summary>
    /// 两个整型拼接为长整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long SpliceToLong(int upper, int lower) =>
        ((long)upper << 32) | (uint)lower;

    /// <summary>
    /// 两个无符号整型拼接为长整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long SpliceToLong(uint upper, uint lower) =>
        ((long)upper << 32) | lower;

    /// <summary>
    /// 两个整型拼接为无符号长整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>无符号长整型</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SpliceToULong(int upper, int lower) =>
        ((ulong)upper << 32) | (uint)lower;

    /// <summary>
    /// 两个无符号整型拼接为无符号长整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>无符号长整型</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SpliceToULong(uint upper, uint lower) =>
        ((ulong)upper << 32) | lower;

#if NET7_0_OR_GREATER
    /// <summary>
    /// 两个长整型拼接为128位整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int128 SpliceToInt128(long upper, long lower) => new((ulong)upper, (ulong)lower);

    /// <summary>
    /// 两个无符号长整型拼接为128位整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>拼接结果</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int128 SpliceToInt128(ulong upper, ulong lower) => new(upper, lower);

    /// <summary>
    /// 两个长整型拼接为无符号128位整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>无符号128整型</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt128 SpliceToUInt128(long upper, long lower) => new((ulong)upper, (ulong)lower);

    /// <summary>
    /// 两个无符号长整型拼接为无符号128位整型
    /// </summary>
    /// <param name="upper">高位</param>
    /// <param name="lower">低位</param>
    /// <returns>无符号128整型</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UInt128 SpliceToUInt128(ulong upper, ulong lower) => new(upper, lower);
#endif

    /// <summary>
    /// 将短整型按高低位解析为两个有符号字节
    /// </summary>
    /// <param name="number">短整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (sbyte Upper, sbyte Lower) SplitToSByte(short number) =>
        ((sbyte)(number >> 8), (sbyte)(number & 0xFF));

    /// <summary>
    /// 将无符号短整型按高低位解析为两个有符号字节
    /// </summary>
    /// <param name="number">无符号短整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (sbyte Upper, sbyte Lower) SplitToSByte(ushort number) =>
        ((sbyte)(number >> 8), (sbyte)(number & 0xFF));

    /// <summary>
    /// 将短整型按高低位解析为两个字节
    /// </summary>
    /// <param name="number">短整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (byte Upper, byte Lower) SplitToByte(short number) =>
        ((byte)(number >> 8), (byte)(number & 0xFF));

    /// <summary>
    /// 将无符号短整型按高低位解析为两个字节
    /// </summary>
    /// <param name="number">无符号短整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (byte Upper, byte Lower) SplitToByte(ushort number) =>
        ((byte)(number >> 8), (byte)(number & 0xFF));

    /// <summary>
    /// 将整型按高低位解析为两个短整型
    /// </summary>
    /// <param name="number">整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (short Upper, short Lower) SplitToShort(int number) =>
        ((short)(number >> 16), (short)(number & 0xFFFF));

    /// <summary>
    /// 将无符号整型按高低位解析为两个短整型
    /// </summary>
    /// <param name="number">整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (short Upper, short Lower) SplitToShort(uint number) =>
        ((short)(number >> 16), (short)(number & 0xFFFF));

    /// <summary>
    /// 将整型按高低位解析为两个无符号短整型
    /// </summary>
    /// <param name="number">无符号整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ushort Upper, ushort Lower) SplitToUShort(int number) =>
        ((ushort)(number >> 16), (ushort)(number & 0xFFFF));

    /// <summary>
    /// 将无符号整型按高低位解析为两个无符号短整型
    /// </summary>
    /// <param name="number">无符号整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ushort Upper, ushort Lower) SplitToUShort(uint number) =>
        ((ushort)(number >> 16), (ushort)(number & 0xFFFF));

    /// <summary>
    /// 将长整型按高低位解析为两个整型
    /// </summary>
    /// <param name="number">长整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int Upper, int Lower) SplitToInt(long number) =>
        ((int)(number >> 32), (int)(number & 0xFFFFFFFF));

    /// <summary>
    /// 将无符号长整型按高低位解析为两个整型
    /// </summary>
    /// <param name="number">长整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int Upper, int Lower) SplitToInt(ulong number) =>
        ((int)(number >> 32), (int)(number & 0xFFFFFFFF));

    /// <summary>
    /// 将长整型按高低位解析为两个无符号整型
    /// </summary>
    /// <param name="number">无符号长整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (uint Upper, uint Lower) SplitToUInt(long number) =>
        ((uint)(number >> 32), (uint)(number & 0xFFFFFFFF));

    /// <summary>
    /// 将无符号长整型按高低位解析为两个无符号整型
    /// </summary>
    /// <param name="number">无符号长整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (uint Upper, uint Lower) SplitToUInt(ulong number) =>
        ((uint)(number >> 32), (uint)(number & 0xFFFFFFFF));

#if NET7_0_OR_GREATER
    /// <summary>
    /// 将128位整型按高低位解析为两个长整型
    /// </summary>
    /// <param name="number">128位整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (long Upper, long Lower) SplitToLong(Int128 number) =>
        ((long)(number >> 64), (long)(number & 0xFFFFFFFFFFFFFFFF));

    /// <summary>
    /// 将无符号128位整型按高低位解析为两个长整型
    /// </summary>
    /// <param name="number">128位整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (long Upper, long Lower) SplitToLong(UInt128 number) =>
        ((long)(number >> 64), (long)(number & 0xFFFFFFFFFFFFFFFF));

    /// <summary>
    /// 将128位整型按高低位解析为两个无符号长整型
    /// </summary>
    /// <param name="number">无符号128位整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ulong Upper, ulong Lower) SplitToULong(Int128 number) =>
        ((ulong)(number >> 64), (ulong)(number & 0xFFFFFFFFFFFFFFFF));

    /// <summary>
    /// 将无符号128位整型按高低位解析为两个无符号长整型
    /// </summary>
    /// <param name="number">无符号128位整型</param>
    /// <returns>(高位,低位)</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ulong Upper, ulong Lower) SplitToULong(UInt128 number) =>
        ((ulong)(number >> 64), (ulong)(number & 0xFFFFFFFFFFFFFFFF));
#endif
}