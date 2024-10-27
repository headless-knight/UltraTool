using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 集合帮助类
/// </summary>
[PublicAPI]
public static class CollectionHelper
{
    /// <summary>
    /// 两个集合的并集<br/>
    /// 针对一个集合中存在多个相同元素的情况，计算两个集合中此元素的个数，保留最多的个数<br/>
    /// 例如：集合1：[a, b, c, c, c]，集合2：[a, b, c, c]<br/>
    /// 结果：[a, b, c, c, c]，此结果中只保留了三个c
    /// </summary>
    /// <param name="coll1">集合1</param>
    /// <param name="coll2">集合2</param>
    /// <returns>并集的集合</returns>
    [Pure]
    public static List<T> Union<T>(IReadOnlyCollection<T> coll1, IReadOnlyCollection<T> coll2) where T : notnull
    {
        if (coll1 is not { Count: > 0 }) return [..coll2];

        if (coll2 is not { Count: > 0 }) return [..coll1];

        var result = new List<T>(Math.Max(coll1.Count, coll2.Count));
        var dict1 = coll1.CountMap();
        var dict2 = coll2.CountMap();
        var set = new HashSet<T>(coll1);
        set.AddRange(coll2);
        foreach (var item in set)
        {
            var count = Math.Max(dict1.GetValueOrDefault(item, 0), dict2.GetValueOrDefault(item, 0));
            for (var i = 0; i < count; i++)
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// 两个集合的交集<br/>
    /// 针对一个集合中存在多个相同元素的情况，计算两个集合中此元素的个数，保留最少的个数<br/>
    /// 例如：集合1：[a, b, c, c, c]，集合2：[a, b, c, c]<br/>
    /// 结果：[a, b, c, c]，此结果中只保留了两个c
    /// </summary>
    /// <param name="coll1">集合1</param>
    /// <param name="coll2">集合2</param>
    /// <returns>交集的集合</returns>
    [Pure]
    public static List<T> Intersection<T>(IReadOnlyCollection<T> coll1, IReadOnlyCollection<T> coll2) where T : notnull
    {
        if (coll1 is not { Count: > 0 } || coll2 is not { Count: > 0 }) return [];

        var result = new List<T>(Math.Min(coll1.Count, coll2.Count));
        var dict1 = coll1.CountMap();
        var dict2 = coll2.CountMap();
        var set = new HashSet<T>(coll1);
        foreach (var item in set)
        {
            var count = Math.Min(dict1.GetValueOrDefault(item, 0), dict2.GetValueOrDefault(item, 0));
            for (var i = 0; i < count; i++)
            {
                result.Add(item);
            }
        }

        return result;
    }

    /// <summary>
    /// 两个集合的差集<br/>
    /// 针对一个集合中存在多个相同元素的情况，计算两个集合中此元素的个数，保留两个集合中此元素个数差的个数<br/>
    /// 例如：
    /// <code>
    /// disjunction([a, b, c, c, c], [a, b, c, c]) -> [c]
    /// disjunction([a, b], [])                    -> [a, b]
    /// disjunction([a, b, c], [b, c, d])          -> [a, d]
    /// </code>
    /// </summary>
    /// <param name="coll1">集合1</param>
    /// <param name="coll2">集合2</param>
    /// <returns>差集的集合</returns>
    [Pure]
    public static List<T> Disjunction<T>(IReadOnlyCollection<T> coll1, IReadOnlyCollection<T> coll2) where T : notnull
    {
        if (coll1 is not { Count: > 0 }) return coll2.ToList();

        if (coll2 is not { Count: > 0 }) return coll1.ToList();

        var result = new List<T>();
        var dict1 = coll1.CountMap();
        var dict2 = coll2.CountMap();
        var set = new HashSet<T>(coll1);
        set.AddRange(coll2);
        foreach (var item in set)
        {
            var count = Math.Abs(dict1.GetValueOrDefault(item, 0) - dict2.GetValueOrDefault(item, 0));
            for (var i = 0; i < count; i++)
            {
                result.Add(item);
            }
        }

        return result;
    }
}