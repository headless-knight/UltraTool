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
    #region 线性查找

    /// <summary>
    /// 查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlyList<T> list, T value, IEqualityComparer<T>? comparer = null) =>
        list.IndexOf(value, 0, list.Count, comparer);

    /// <summary>
    /// 查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="index">起始索引</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this IReadOnlyList<T> list, T value, int index,
        IEqualityComparer<T>? comparer = null) => list.IndexOf(value, index, list.Count - index, comparer);

    /// <summary>
    /// 查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int IndexOf<T>(this IReadOnlyList<T> list, T value, int index, int count,
        IEqualityComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return -1;

        comparer ??= EqualityComparer<T>.Default;
        var end = index + count - 1;
        for (var i = index; i <= end; i++)
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
        list.LastIndexOf(value, list.Count - 1, list.Count, comparer);

    /// <summary>
    /// 从后往前查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="index">起始索引</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf<T>(this IReadOnlyList<T> list, T value, int index,
        IEqualityComparer<T>? comparer = null) => list.LastIndexOf(value, index, index + 1, comparer);

    /// <summary>
    /// 从后往前查找列表中第一个与传入值相等的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">值</param>
    /// <param name="index">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int LastIndexOf<T>(this IReadOnlyList<T> list, T value, int index, int count,
        IEqualityComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(index - count + 1);
        if (count == 0) return -1;

        comparer ??= EqualityComparer<T>.Default;
        var end = index - count + 1;
        for (var i = index; i >= end; i--)
        {
            if (comparer.Equals(list[i], value)) return i;
        }

        return -1;
    }

    /// <summary>
    /// 查找列表中所有与传入值相等的元素的索引
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
    /// 查找列表中所有与传入值相等的元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="value">值</param>
    /// <param name="comparer">元素相等比较器，默认为null</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<int> IndexesOf<T>(this IReadOnlyList<T> list, int index, T value,
        IEqualityComparer<T>? comparer = null) => list.IndexesOf(index, list.Count - index, value, comparer);

    /// <summary>
    /// 查找列表中所有与传入值相等的元素的索引
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
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) yield break;

        comparer ??= EqualityComparer<T>.Default;
        var end = index + count - 1;
        for (var i = index; i <= end; i++)
        {
            if (comparer.Equals(list[i], value)) yield return i;
        }
    }

    #endregion

    #region 线性条件查找

    /// <summary>
    /// 返回列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> match) =>
        list.FindIndex(0, list.Count, match);

    /// <summary>
    /// 返回列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindIndex<T>(this IReadOnlyList<T> list, int startIndex, Predicate<T> match) =>
        list.FindIndex(startIndex, list.Count - startIndex, match);

    /// <summary>
    /// 返回列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static int FindIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        if (count == 0) return -1;

        var end = startIndex + count - 1;
        for (var i = startIndex; i <= end; i++)
        {
            if (match.Invoke(list[i])) return i;
        }

        return -1;
    }

    /// <summary>
    /// 从后往前查找列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindLastIndex<T>(this IReadOnlyList<T> list, Predicate<T> match) =>
        list.FindLastIndex(list.Count - 1, list.Count, match);

    /// <summary>
    /// 从后往前查找列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FindLastIndex<T>(this IReadOnlyList<T> list, int startIndex, Predicate<T> match) =>
        list.FindLastIndex(startIndex, startIndex + 1, match);

    /// <summary>
    /// 从后往前查找列表中符合匹配条件的第一个元素的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    public static int FindLastIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex - count + 1);
        if (count == 0) return -1;

        var end = startIndex - count + 1;
        for (var i = startIndex; i >= end; i--)
        {
            if (match.Invoke(list[i])) return i;
        }

        return -1;
    }

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
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
    /// <param name="match">条件委托</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<int> FindIndexes<T>(this IReadOnlyList<T> list, int startIndex, Predicate<T> match) =>
        list.FindIndexes(startIndex, list.Count - startIndex, match);

    /// <summary>
    /// 返回列表中所有符合条件的索引
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<int> FindIndexes<T>(this IReadOnlyList<T> list, int startIndex, int count,
        Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex - count + 1);
        if (count == 0) yield break;

        var end = startIndex + count - 1;
        for (var i = startIndex; i <= end; i++)
        {
            if (match.Invoke(list[i])) yield return i;
        }
    }

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindFirst<T>(this IReadOnlyList<T> list, Predicate<T> match,
        [MaybeNullWhen(false)] out T found) => list.TryFindFirst(0, list.Count, match, out found);

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">条件委托</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindFirst<T>(this IReadOnlyList<T> list, int startIndex, Predicate<T> match,
        [MaybeNullWhen(false)] out T found) => list.TryFindFirst(startIndex, list.Count - startIndex, match, out found);

    /// <summary>
    /// 尝试从列表中找到第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindFirst<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match,
        [MaybeNullWhen(false)] out T found)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, list.Count);
        if (count == 0)
        {
            found = default;
            return false;
        }

        var end = startIndex + count - 1;
        for (var i = startIndex; i <= end; i++)
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
    /// <param name="match">匹配条件，入参(遍历元素)</param>
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
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">匹配条件，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFindLast<T>(this IReadOnlyList<T> list, int startIndex, Predicate<T> match,
        [MaybeNullWhen(false)] out T found) => list.TryFindLast(startIndex, startIndex + 1, match, out found);

    /// <summary>
    /// 尝试从列表中从后往前找到一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">匹配条件，入参(遍历元素)</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryFindLast<T>(this IReadOnlyList<T> list, int startIndex, int count, Predicate<T> match,
        [MaybeNullWhen(false)] out T found)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfNegative(startIndex - count + 1);
        if (count == 0)
        {
            found = default;
            return false;
        }

        var end = startIndex - count + 1;
        for (var i = startIndex; i >= end; i--)
        {
            var item = list[i];
            if (!match.Invoke(item)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    #endregion

    #region 二分查找

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">查找值</param>
    /// <param name="comparison">元素比较表达式</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T, TList>(this TList list, T value, Comparison<T> comparison)
        where TList : IReadOnlyList<T> => list.BinarySearch(0, list.Count, value, comparison);

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="value">查找值</param>
    /// <param name="comparison">元素比较表达式</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T, TList>(this TList list, int index, T value, Comparison<T> comparison)
        where TList : IReadOnlyList<T> => list.BinarySearch(index, list.Count - index, value, comparison);

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
    public static int BinarySearch<T, TList>(this TList list, int index, int count, T value, Comparison<T> comparison)
        where TList : IReadOnlyList<T>
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return ~index;

        return BinarySearchInternal(list, index, count, value, new ValueComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="value">查找值</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T, TList>(this TList list, T value, IComparer<T>? comparer = null)
        where TList : IReadOnlyList<T> => list.BinarySearch(0, list.Count, value, comparer);

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">起始索引</param>
    /// <param name="value">查找值</param>
    /// <param name="comparer">元素比较器，默认为null</param>
    /// <returns>查找值索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BinarySearch<T, TList>(this TList list, int index, T value, IComparer<T>? comparer = null)
        where TList : IReadOnlyList<T> => list.BinarySearch(index, list.Count - index, value, comparer);

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
    public static int BinarySearch<T, TList>(this TList list, int index, int count, T value,
        IComparer<T>? comparer = null) where TList : IReadOnlyList<T>
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfNegative(count);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, list.Count);
        if (count == 0) return ~index;

        return BinarySearchInternal(list, index, count, value, comparer ?? Comparer<T>.Default);
    }

    /// <summary>二分查找，内部实现</summary>
    private static int BinarySearchInternal<T, TList, TComparer>(this TList list, int index, int count, T value,
        TComparer comparer) where TList : IReadOnlyList<T> where TComparer : IComparer<T>
    {
        var left = index;
        var right = index + count - 1;
        while (left <= right)
        {
            var mid = left + (right - left >> 1);
            var compared = comparer.Compare(list[mid], value);
            switch (compared)
            {
                case 0: return mid;
                case < 0:
                {
                    left = mid + 1;
                    break;
                }
                case > 0:
                {
                    right = mid - 1;
                    break;
                }
            }
        }

        return ~left;
    }

    #endregion
}