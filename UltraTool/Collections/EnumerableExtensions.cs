using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 序列拓展类
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
#if !NET6_0_OR_GREATER
    /// <summary>
    /// 尝试不使用枚举器获取序列元素数量
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="count">元素数量</param>
    /// <returns>是否成功获取</returns>
    public static bool TryGetNonEnumeratedCount<T>([NoEnumeration] this IEnumerable<T> source, out int count)
    {
        switch (source)
        {
            case ICollection<T> coll:
            {
                count = coll.Count;
                return true;
            }
            case IReadOnlyCollection<T> roColl:
            {
                count = roColl.Count;
                return true;
            }
            default:
            {
                count = 0;
                return false;
            }
        }
    }
#endif
}