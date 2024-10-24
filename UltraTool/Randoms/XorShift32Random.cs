using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Randoms;

/// <summary>
/// XorShift32算法随机数生成器
/// </summary>
/// <param name="seed">随机数种子</param>
[PublicAPI]
public struct XorShift32Random(int seed)
{
    /// <summary>
    /// 当前随机种子值
    /// </summary>
    public int CurrentValue { get; private set; } = seed;

    /// <summary>
    /// 计算下一个随机数
    /// </summary>
    /// <returns>随机数</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Next() => CurrentValue = RandomHelper.XorShift32(CurrentValue);
}