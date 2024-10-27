using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Randoms;

namespace UltraTool.Collections;

/// <summary>
/// 字典拓展类
/// </summary>
[PublicAPI]
public static class DictionaryExtensions
{
    /// <summary>
    /// 若为null则返回空字典，否则返回原字典
    /// </summary>
    /// <param name="dict">字典</param>
    /// <returns>原字典或空字典</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyDictionary<TKey, TValue> EmptyIfNull<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue>? dict) where TKey : notnull =>
        dict ?? ImmutableDictionary<TKey, TValue>.Empty;

    /// <summary>
    /// 将字典中所有键填充为指定值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="value">值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void Fill<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue value)
    {
        foreach (var key in dict.Keys)
        {
            dict[key] = value;
        }
    }

    /// <summary>
    /// 获取指定键对应值，不存在则添加并返回缺省值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">缺省值</param>
    /// <returns>获取值或缺省值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addValue)
        where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.GetOrAdd(key, addValue);
        }

        if (!dict.TryGetValue(key, out var value))
        {
            dict[key] = value = addValue;
        }

        return value;
    }

    /// <summary>
    /// 获取指定键对应值，不存在则执行生成值委托生成值，添加并返回
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="creator">值生成委托，入参(键)</param>
    /// <returns>获取值或生成值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        Func<TKey, TValue> creator) where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.GetOrAdd(key, creator);
        }

        if (!dict.TryGetValue(key, out var value))
        {
            dict[key] = value = creator.Invoke(key);
        }

        return value;
    }

    /// <summary>
    /// 获取指定键对应值，不存在则执行生成值委托生成值，添加并返回
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="creator">值生成委托，入参(键,额外参数)</param>
    /// <param name="args">额外参数</param>
    /// <returns>获取值或生成值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrAdd<TKey, TValue, TArgs>(this IDictionary<TKey, TValue> dict, TKey key,
        [RequireStaticDelegate] Func<TKey, TArgs, TValue> creator, TArgs args) where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.GetOrAdd(key, creator, args);
        }

        if (!dict.TryGetValue(key, out var value))
        {
            dict[key] = value = creator.Invoke(key, args);
        }

        return value;
    }

    /// <summary>
    /// 获取指定键对应值，不存在则创建新实例并存入字典
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="onCreated">值创建时委托，入参(键,创建值)，默认为null</param>
    /// <returns>获取值或创建值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        Action<TKey, TValue>? onCreated = null) where TKey : notnull where TValue : new()
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.GetOrAdd(key, static (keyArgs, actionArgs) =>
            {
                var created = new TValue();
                actionArgs?.Invoke(keyArgs, created);
                return created;
            }, onCreated);
        }

        if (dict.TryGetValue(key, out var value)) return value;

        dict[key] = value = new TValue();
        onCreated?.Invoke(key, value);
        return value;
    }

    /// <summary>
    /// 获取指定键对应值，不存在则创建新实例并存入字典
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="onCreated">值创建时委托，入参(键,额外参数,创建值)</param>
    /// <param name="args">额外参数</param>
    /// <returns>获取值或创建值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.UpdatedContent)]
    public static TValue GetOrCreate<TKey, TValue, TArgs>(this IDictionary<TKey, TValue> dict, TKey key,
        [RequireStaticDelegate] Action<TKey, TArgs, TValue> onCreated, TArgs args)
        where TKey : notnull where TValue : new()
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.GetOrAdd(key, static (keyArgs, extra) =>
            {
                var created = new TValue();
                extra.onCreated.Invoke(keyArgs, extra.args, created);
                return created;
            }, (onCreated, args));
        }

        if (dict.TryGetValue(key, out var value)) return value;

        dict[key] = value = new TValue();
        onCreated.Invoke(key, args, value);
        return value;
    }

    /// <summary>
    /// 交换两个键对应的值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key1">键1</param>
    /// <param name="key2">键2</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Swap<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key1, TKey key2) =>
        (dict[key1], dict[key2]) = (dict[key2], dict[key1]);

    /// <summary>
    /// 将字典中元素顺序随机打乱
    /// </summary>
    /// <param name="dict">字典</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Shuffle<TKey, TValue>(this IDictionary<TKey, TValue> dict) => RandomHelper.Shared.Shuffle(dict);
}