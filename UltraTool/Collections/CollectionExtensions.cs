using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 集合拓展类
/// </summary>
[PublicAPI]
public static class CollectionExtensions
{
    /// <summary>
    /// 判断集合是否包含指定序列的任意元素
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="values">序列</param>
    /// <returns>是否包含任意元素</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAny<T>(this IReadOnlyCollection<T> coll, [InstantHandle] IEnumerable<T> values) =>
        coll is { Count: > 0 } && values.Any(coll.Contains);

    /// <summary>
    /// 判断集合是否包含指定序列的全部元素
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="values">序列</param>
    /// <returns>是否包含全部元素</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsAll<T>(this IReadOnlyCollection<T> coll, [InstantHandle] IEnumerable<T> values) =>
        coll is { Count: > 0 } && values.All(coll.Contains);

    /// <summary>
    /// 当数据满足条件时添加
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="value">待添加值</param>
    /// <param name="match">条件委托</param>
    /// <returns>是否满足条件</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static bool AddIf<T>(this ICollection<T> coll, T value, Predicate<T> match)
    {
        if (!match.Invoke(value)) return false;

        coll.Add(value);
        return true;
    }

    /// <summary>
    /// 批量添加传入序列中满足条件的元素
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="range">待添加集合</param>
    /// <param name="match">条件委托</param>
    /// <returns>满足添加的数量</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static int AddRangeIf<T>(this ICollection<T> coll, [InstantHandle] IEnumerable<T> range, Predicate<T> match)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        // 不使用Linq，避免闭包
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in range)
        {
            if (coll.AddIf(item, match)) count++;
        }

        return count;
    }

    /// <summary>
    /// 如果集合不存在该值则添加值
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="value">待添加值</param>
    /// <returns>是否不存在集合中</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static bool AddIfNotContains<T>(this ICollection<T> coll, T value)
    {
        if (!coll.Contains(value)) return false;

        coll.Add(value);
        return true;
    }

    /// <summary>
    /// 添加非null数据，若数据不为null则添加并返回true
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="value">数据</param>
    /// <returns>是否非null</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static bool AddNonNull<T>(this ICollection<T> coll, [NotNullWhen(true)] T? value)
    {
        if (value == null) return false;

        coll.Add(value);
        return true;
    }

    /// <summary>
    /// 批量添加非null数据，若数据不为null则添加，返回添加元素数量
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="range">待添加序列</param>
    /// <returns>添加元素数量</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddNonNullRange<T>(this ICollection<T> coll, [InstantHandle] IEnumerable<T?> range) =>
        range.TryGetNonEnumeratedCount(out var size) && size <= 0 ? 0 : range.Count(coll.AddNonNull);

    /// <summary>
    /// 如果待删除值满足条件则执行删除
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="value">待删除值</param>
    /// <param name="match">条件委托</param>
    /// <returns>是否删除成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RemoveIf<T>(this ICollection<T> coll, T value, Predicate<T> match) =>
        match.Invoke(value) && coll.Remove(value);

    /// <summary>
    /// 删除<paramref name="range"/>待删除序列所有元素
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="range">待删除序列</param>
    /// <returns>成功删除个数</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int RemoveRange<T>(this ICollection<T> coll, [InstantHandle] IEnumerable<T> range)
    {
        if (coll is not { Count: > 0 }) return 0;

        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        // 不使用Linq，避免闭包
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in range)
        {
            if (coll.Remove(item)) count++;
        }

        return count;
    }

    /// <summary>
    /// 删除<paramref name="range"/>待删除序列中所有满足条件的元素
    /// </summary>
    /// <param name="coll">集合</param>
    /// <param name="range">待删除序列</param>
    /// <param name="match">条件委托</param>
    /// <returns>成功删除个数</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int RemoveRangeIf<T>(this ICollection<T> coll, [InstantHandle] IEnumerable<T> range,
        Predicate<T> match)
    {
        if (coll is not { Count: > 0 }) return 0;

        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        // 不使用Linq，避免闭包
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in range)
        {
            if (coll.RemoveIf(item, match)) count++;
        }

        return count;
    }
}