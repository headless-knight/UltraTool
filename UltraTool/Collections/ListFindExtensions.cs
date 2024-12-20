using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;
using UltraTool.Helpers;

namespace UltraTool.Collections;

/// <summary>
/// 列表查找扩展类
/// </summary>
[PublicAPI]
public static class ListFindExtensions
{
    /// <summary>
    /// 找到列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlyList<T> list, T value, IEqualityComparer<T>? comparer = null) =>
        list.IndexOf(0, list.Count, value, comparer);

    /// <summary>
    /// 找到列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int IndexOf<T>(this IReadOnlyList<T> list, int index, int count, T value,
        IEqualityComparer<T>? comparer = null)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        comparer ??= EqualityComparer<T>.Default;
        var end = index + count;
        for (var i = index; i < end; i++)
        {
            if (comparer.Equals(list[i], value)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 从后往前查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this IReadOnlyList<T> list, T value, IEqualityComparer<T>? comparer = null) =>
        list.LastIndexOf(list.Count - 1, list.Count, value, comparer);

    /// <summary>
    /// 从后往前查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int LastIndexOf<T>(this IReadOnlyList<T> list, int index, int count, T value,
        IEqualityComparer<T>? comparer = null)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, list.Count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(index - count + 1);
        comparer ??= EqualityComparer<T>.Default;
        var num = index - count;
        for (var i = index; i > num; i--)
        {
            if (comparer.Equals(list[i], value)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 找到列表中所有与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<int> IndexesOf<T>(this IReadOnlyList<T> list, T value,
        IEqualityComparer<T>? comparer = null) => list.IndexesOf(0, list.Count, value, comparer);

    /// <summary>
    /// 找到列表中所有与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<int> IndexesOf<T>(this IReadOnlyList<T> list, int index, int count, T value,
        IEqualityComparer<T>? comparer = null)
    {
        if (count <= 0) yield break;

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        comparer ??= EqualityComparer<T>.Default;
        var end = index + count;
        for (var i = index; i < end; i++)
        {
            if (comparer.Equals(list[i], value)) yield return i;
        }
    }

    /// <summary>
    /// 返回列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素)</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> match) =>
        list.FindIndex(0, list.Count, match);

    /// <summary>
    /// 返回列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素)</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int FindIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (match.Invoke(list[i])) return i;
        }

        return -1;
    }

    /// <summary>
    /// 返回列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindIndex<T, TArgs>(this IReadOnlyList<T> list,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args) =>
        list.FindIndex(0, list.Count, match, args);

    /// <summary>
    /// 返回列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int FindIndex<T, TArgs>(this IReadOnlyList<T> list, int startIndex, int count,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (match.Invoke(list[i], args)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 从后往前查找列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindLastIndex<T>(this IReadOnlyList<T> list, Predicate<T> match) =>
        list.FindLastIndex(list.Count - 1, list.Count, match);

    /// <summary>
    /// 从后往前查找列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">开始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <returns>索引</returns>
    public static int FindLastIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(startIndex, list.Count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex - count + 1);
        var num = startIndex - count;
        for (var i = startIndex; i > num; i--)
        {
            if (match.Invoke(list[i])) return i;
        }

        return -1;
    }

    /// <summary>
    /// 从后往前查找列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindLastIndex<T, TArgs>(this IReadOnlyList<T> list,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args) =>
        list.FindLastIndex(list.Count - 1, list.Count, match, args);

    /// <summary>
    /// 从后往前查找列表中符合条件委托的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">开始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引</returns>
    public static int FindLastIndex<T, TArgs>(this IReadOnlyList<T> list, int startIndex, int count,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args)
    {
        if (count <= 0) return -1;

        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(startIndex, list.Count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex - count + 1);
        var num = startIndex - count;
        for (var i = startIndex; i > num; i--)
        {
            if (match.Invoke(list[i], args)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素)</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<int> FindIndexes<T>(this IReadOnlyList<T> list, Predicate<T> match) =>
        list.FindIndexes(0, list.Count, match);

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素)</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<int> FindIndexes<T>(this IReadOnlyList<T> list, int startIndex, int count,
        Predicate<T> match)
    {
        if (count <= 0) yield break;

        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (match.Invoke(list[i])) yield return i;
        }
    }

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<int> FindIndexes<T, TArgs>(this IReadOnlyList<T> list, Func<T, TArgs, bool> match,
        TArgs args) => list.FindIndexes(0, list.Count, match, args);

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<int> FindIndexes<T, TArgs>(this IReadOnlyList<T> list, int startIndex, int count,
        Func<T, TArgs, bool> match, TArgs args)
    {
        if (count <= 0) yield break;

        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (match.Invoke(list[i], args)) yield return i;
        }
    }

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindFirst<T>(this IReadOnlyList<T> list, Predicate<T> match,
        [MaybeNullWhen(false)] out T found) => list.TryFindFirst(0, list.Count, match, out found);

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindFirst<T>(this IReadOnlyList<T> list, int index, int count, Predicate<T> match,
        [MaybeNullWhen(false)] out T found)
    {
        if (count <= 0)
        {
            found = default;
            return false;
        }

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        var end = index + count;
        // 不使用foreach，避免装箱
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = index; i < end; i++)
        {
            var item = list[i];
            if (!match.Invoke(item)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindFirst<T, TArgs>(this IReadOnlyList<T> list,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args, [MaybeNullWhen(false)] out T found) =>
        list.TryFindFirst(0, list.Count, match, args, out found);

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindFirst<T, TArgs>(this IReadOnlyList<T> list, int index, int count,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args, [MaybeNullWhen(false)] out T found)
    {
        if (count <= 0)
        {
            found = default;
            return false;
        }

        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        var end = index + count;
        // 不使用foreach，避免装箱
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = index; i < end; i++)
        {
            var item = list[i];
            if (!match.Invoke(item, args)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    /// <summary>
    /// 尝试从列表中从后往前找到一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindLast<T>(this IReadOnlyList<T> list, Predicate<T> match,
        [MaybeNullWhen(false)] out T found) => list.TryFindLast(list.Count - 1, list.Count, match, out found);

    /// <summary>
    /// 尝试从列表中从后往前找到一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">开始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindLast<T>(this IReadOnlyList<T> list, int index, int count, Predicate<T> match,
        [MaybeNullWhen(false)] out T found)
    {
        if (count <= 0)
        {
            found = default;
            return false;
        }

        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, list.Count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(index - count + 1);
        var num = index - count;
        for (var i = index; i > num; i--)
        {
            var item = list[i];
            if (!match.Invoke(item)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    /// <summary>
    /// 尝试从列表中从后往前找到一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindLast<T, TArgs>(this IReadOnlyList<T> list,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args, [MaybeNullWhen(false)] out T found) =>
        list.TryFindLast(list.Count - 1, list.Count, match, args, out found);

    /// <summary>
    /// 尝试从列表中从后往前找到一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">开始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托，入参(遍历元素,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindLast<T, TArgs>(this IReadOnlyList<T> list, int index, int count,
        [RequireStaticDelegate] Func<T, TArgs, bool> match, TArgs args, [MaybeNullWhen(false)] out T found)
    {
        if (count <= 0)
        {
            found = default;
            return false;
        }

        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, list.Count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(index - count + 1);
        var num = index - count;
        for (var i = index; i > num; i--)
        {
            var item = list[i];
            if (!match.Invoke(item, args)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">查找值</param>
    /// <param name="comparison">元素比较表达式</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T>(this IReadOnlyList<T> list, T value, Comparison<T> comparison) =>
        list.BinarySearch(value, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="value">查找值</param>
    /// <param name="comparison">元素比较表达式</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int count, T value,
        Comparison<T> comparison) => list.BinarySearch(index, count, value, new ComparisonComparer<T>(comparison));

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">查找值</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T>(this IReadOnlyList<T> list, T value, IComparer<T>? comparer = null) =>
        list.BinarySearch(0, list.Count, value, comparer);

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="value">查找值</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int BinarySearch<T>(this IReadOnlyList<T> list, int index, int count, T value,
        IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        comparer ??= Comparer<T>.Default;
        var start = index;
        var end = index + count - 1;
        while (start <= end)
        {
            var current = start + (end - start >> 1);
            var compared = comparer.Compare(list[current], value);
            switch (compared)
            {
                case 0: return current;
                case < 0:
                {
                    start = current + 1;
                    break;
                }
                default:
                {
                    end = current - 1;
                    break;
                }
            }
        }

        return ~start;
    }
}