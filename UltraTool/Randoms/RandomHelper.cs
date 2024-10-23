namespace UltraTool.Randoms;

/// <summary>
/// 随机帮助类
/// </summary>
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
}