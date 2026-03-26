using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Randoms;

namespace UltraTool.Collections;

/// <summary>
/// 列表拓展类
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// 若为null则返回空列表，否则返回原列表
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>原列表或空列表</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? list) =>
        list ?? Array.Empty<T>();

    /// <summary>
    /// 判断指定索引是否合法
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引是否合法</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidIndex<T>(this IReadOnlyList<T> list, int index) =>
        index >= 0 && index < list.Count;

    /// <summary>
    /// 判断指定索引是否非法
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引是否非法</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInvalidIndex<T>(this IReadOnlyList<T> list, int index) =>
        index < 0 || index >= list.Count;

    /// <summary>
    /// 尝试获取指定索引值，当索引不正确获取失败
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    /// <returns>是否成功获取</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public static bool TryGetValue<T>([NotNullWhen(true)] this IReadOnlyList<T>? list, int index,
        [MaybeNullWhen(false)] out T value)
    {
        if (list is not { Count: > 0 } || list.IsInvalidIndex(index))
        {
            value = default;
            return false;
        }

        value = list[index];
        return true;
    }

    /// <summary>
    /// 获取指定索引值，不存在则返回类型默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引值</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefault<T>(this IReadOnlyList<T> list, int index) =>
        list.TryGetValue(index, out var got) ? got : default;

    /// <summary>
    /// 获取指定索引值，获取不到返回默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>索引值</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T, TList>(this TList list, int index, T defaultValue)
        where TList : IReadOnlyList<T> => list.TryGetValue(index, out var got) ? got : defaultValue;

    /// <summary>
    /// 获取指定索引值，超出范围返回最接近该索引位置的值，若列表为空则返回类型默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引值</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public static T? GetNearestOrDefault<T>(this IReadOnlyList<T> list, int index)
    {
        // 列表为空，返回默认值
        if (list is not { Count: > 0 }) return default;

        // 索引小于0，返回第一个元素
        if (index < 0) return list[0];

        // 索引大于等于列表长度，返回最后一个元素，否则返回指定索引位置的元素
        return index >= list.Count ? list[^1] : list[index];
    }

    /// <summary>
    /// 获取指定索引值，超出范围返回最接近该索引位置的值，若列表为空则返回默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>索引值</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public static T GetNearestOrDefault<T, TList>(this TList list, int index, T defaultValue)
        where TList : IReadOnlyList<T>
    {
        // 列表为空，返回默认值
        if (list is not { Count: > 0 }) return defaultValue;

        // 索引小于0，返回第一个元素
        if (index < 0) return list[0];

        // 索引大于等于列表长度，返回最后一个元素，否则返回指定索引位置的元素
        return index >= list.Count ? list[^1] : list[index];
    }

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="range">待添加序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void AddRange<T>(this IList<T> list, [InstantHandle] IEnumerable<T> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var item in range)
        {
            list.Add(item);
        }
    }

    /// <summary>
    /// 删除列表第一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static T RemoveFirst<T>(this IList<T> list)
    {
        var removed = list[0];
        list.RemoveAt(0);
        return removed;
    }

    /// <summary>
    /// 尝试删除列表第一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>是否成功删除</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveFirst<T>([NotNullWhen(true)] this IList<T>? list)
    {
        if (list is not { Count: > 0 }) return false;

        list.RemoveAt(0);
        return true;
    }

    /// <summary>
    /// 尝试删除列表中匹配到的第一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <returns>是否成功删除</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveFirst<T>([NotNullWhen(true)] this IList<T>? list, Predicate<T> match)
    {
        if (list is not { Count: > 0 }) return false;

        for (var i = 0; i < list.Count; i++)
        {
            if (!match.Invoke(list[i])) continue;

            list.RemoveAt(i);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 尝试删除列表第一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="removed">删除的元素</param>
    /// <returns>是否成功删除</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveFirst<T>([NotNullWhen(true)] this IList<T>? list, [MaybeNullWhen(false)] out T removed)
    {
        if (list is not { Count: > 0 })
        {
            removed = default;
            return false;
        }

        removed = list[0];
        list.RemoveAt(0);
        return true;
    }

    /// <summary>
    /// 尝试删除列表第一个满足条件的元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <param name="removed">删除的元素</param>
    /// <returns>是否成功删除</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveFirst<T>([NotNullWhen(true)] this IList<T>? list, Predicate<T> match,
        [MaybeNullWhen(false)] out T removed)
    {
        if (list is not { Count: > 0 })
        {
            removed = default;
            return false;
        }

        for (var i = 0; i < list.Count; i++)
        {
            if (!match.Invoke(list[i])) continue;

            removed = list[i];
            list.RemoveAt(i);
            return true;
        }

        removed = default;
        return false;
    }

    /// <summary>
    /// 删除列表最后一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static T RemoveLast<T>(this IList<T> list)
    {
        var removed = list[^1];
        list.RemoveAt(list.Count - 1);
        return removed;
    }

    /// <summary>
    /// 尝试删除列表最后一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>是否删除成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveLast<T>([NotNullWhen(true)] this IList<T>? list)
    {
        if (list is not { Count: > 0 }) return false;

        list.RemoveAt(list.Count - 1);
        return true;
    }

    /// <summary>
    /// 尝试删除列表中匹配到的最后一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <returns>是否删除成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveLast<T>([NotNullWhen(true)] this IList<T>? list, Predicate<T> match)
    {
        if (list is not { Count: > 0 }) return false;

        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (!match.Invoke(list[i])) continue;

            list.RemoveAt(i);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 尝试删除列表最后一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="removed">删除的元素</param>
    /// <returns>是否删除成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveLast<T>([NotNullWhen(true)] this IList<T>? list, [MaybeNullWhen(false)] out T removed)
    {
        if (list is not { Count: > 0 })
        {
            removed = default;
            return false;
        }

        removed = list[^1];
        list.RemoveAt(list.Count - 1);
        return true;
    }

    /// <summary>
    /// 尝试删除列表中匹配到的最后一个元素
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="match">条件委托</param>
    /// <param name="removed">删除的元素</param>
    /// <returns>是否删除成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryRemoveLast<T>([NotNullWhen(true)] this IList<T>? list, Predicate<T> match,
        [MaybeNullWhen(false)] out T removed)
    {
        if (list is not { Count: > 0 })
        {
            removed = default;
            return false;
        }

        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (!match.Invoke(list[i])) continue;

            removed = list[i];
            list.RemoveAt(i);
            return true;
        }

        removed = default;
        return false;
    }

    /// <summary>
    /// 交换两个索引位置的数据
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index1">索引1</param>
    /// <param name="index2">索引2</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<T>(this IList<T> list, [NonNegativeValue] int index1, [NonNegativeValue] int index2) =>
        (list[index1], list[index2]) = (list[index2], list[index1]);

    /// <summary>
    /// 将列表中元素顺序随机打乱
    /// </summary>
    /// <param name="list">列表</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Shuffle<T>(this IList<T> list) => RandomHelper.Shared.Shuffle(list);

    /// <summary>
    /// 将一个非只读列表包装为只读列表
    /// </summary>
    /// <param name="list">只读列表</param>
    /// <returns>非只读列表</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> list) =>
        list as IReadOnlyList<T> ?? new ReadOnlyListBridge<T>(list);

    /// <summary>
    /// 将一个列表接口类型转换为数组，若非数组则拷贝为数组
    /// </summary>
    /// <param name="list">列表接口实例</param>
    /// <returns>数组</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] AsOrToArray<T>(this IList<T> list) => list as T[] ?? list.ToArray();

    /// <summary>
    /// 将一个列表接口类型转换为列表，若非列表则拷贝为列表
    /// </summary>
    /// <param name="list">列表接口实例</param>
    /// <returns>列表</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> AsOrToList<T>(this IList<T> list) => list as List<T> ?? list.ToList();

    /// <summary>只读列表桥接</summary>
    private sealed class ReadOnlyListBridge<T>(IList<T> list) : IReadOnlyList<T>
    {
        /// <inheritdoc />
        public int Count => list.Count;

        /// <inheritdoc />
        public T this[int index] => list[index];

        /// <inheritdoc />
        [MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        /// <inheritdoc />
        [MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}