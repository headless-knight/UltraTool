using JetBrains.Annotations;

namespace UltraTool.Randoms;

/// <summary>
/// 随机帮助类
/// </summary>
[PublicAPI]
public static class RandomHelper
{
#if !NET6_0_OR_GREATER
    private static Random? _shared;
#endif

    /// <summary>
    /// 共享实例
    /// </summary>
    public static Random Shared =>
#if NET6_0_OR_GREATER
        Random.Shared;
#else
        _shared ??= new Random();
#endif

    /// <summary>
    /// XorShift算法生成32位整型随机数
    /// </summary>
    /// <param name="value">随机种子</param>
    /// <returns>随机数</returns>
    /// <remarks>位移数：13,17,5</remarks>
    [Pure]
    public static int XorShift32(int value)
    {
        value ^= value << 13;
        value ^= value >> 17;
        value ^= value << 5;
        return value;
    }

    /// <summary>
    /// XorShift算法生成无符号32位整型随机数
    /// </summary>
    /// <param name="value">随机种子</param>
    /// <returns>随机数</returns>
    /// <remarks>位移数：13,17,5</remarks>
    [Pure]
    public static uint XorShiftU32(uint value)
    {
        value ^= value << 13;
        value ^= value >> 17;
        value ^= value << 5;
        return value;
    }

    /// <summary>
    /// XorShift算法生成64位整型随机数
    /// </summary>
    /// <param name="value">随机种子</param>
    /// <returns>随机数</returns>
    /// <remarks>位移数：12,25,27</remarks>
    [Pure]
    public static long XorShift64(long value)
    {
        value ^= value >> 12;
        value ^= value << 25;
        value ^= value >> 27;
        return value;
    }

    /// <summary>
    /// XorShift算法生成无符号64位整型随机数
    /// </summary>
    /// <param name="value">随机种子</param>
    /// <returns>随机数</returns>
    /// <remarks>位移数：12,25,27</remarks>
    [Pure]
    public static ulong XorShiftU64(ulong value)
    {
        value ^= value >> 12;
        value ^= value << 25;
        value ^= value >> 27;
        return value;
    }
}