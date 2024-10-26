using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 列表拓展类
/// </summary>
[PublicAPI]
public static class ListExtensions
{
    /// <summary>
    /// 若为null则返回空列表，否则返回原列表
    /// </summary>
    /// <param name="list">列表</param>
    /// <returns>原列表或空列表</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? list) =>
        list ?? Array.Empty<T>();

    /// <summary>
    /// 判断指定索引是否合法
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引是否合法</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValidIndex<T>(this IReadOnlyList<T> list, int index) =>
        index >= 0 && index < list.Count;

    /// <summary>
    /// 判断指定索引是否非法
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引是否非法</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
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
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryGetValue<T>([NotNullWhen(true)] this IReadOnlyList<T>? list, int index,
        [MaybeNullWhen(false)] out T value)
    {
        if (list is not { Count: > 0 })
        {
            value = default;
            return false;
        }

        if (list.IsInvalidIndex(index))
        {
            value = default;
            return false;
        }

        value = list[index];
        return true;
    }

    /// <summary>
    /// 获取指定索引值，不存在则返回默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <returns>索引值或默认值</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefault<T>(this IReadOnlyList<T> list, int index) =>
        list.TryGetValue(index, out var got) ? got : default;

    /// <summary>
    /// 获取指定索引值，获取不到返回默认值
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index">索引</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>获取值或默认值</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValueOrDefault<T>(this IReadOnlyList<T> list, int index, T defaultValue) =>
        list.TryGetValue(index, out var got) ? got : defaultValue;

    /// <summary>
    /// 交换两个索引位置的数据
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="index1">索引1</param>
    /// <param name="index2">索引2</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<T>(this IList<T> list, int index1, int index2) =>
        (list[index1], list[index2]) = (list[index2], list[index1]);
}