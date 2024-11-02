using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
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
    /// 向字典批量添加集合元素
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">待添加键值对序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict.Add(key, value);
        }
    }

    /// <summary>
    /// 尝试批量添加集合可添加元素
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">待添加键值对序列</param>
    /// <returns>成功添加个数</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TryAddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, value) in range)
        {
            if (dict.TryAdd(key, value)) count++;
        }

        return count;
    }

    /// <summary>
    /// 批量设置字典键值对
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">待设置键值对序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static void PutAll<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict[key] = value;
        }
    }

    /// <summary>
    /// 添加或更新值，若键不存在则添加值，否则通过委托更新已存在值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="value">待添加值</param>
    /// <param name="updater">值更新委托，入参(键,已存在值)</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value,
        Func<TKey, TValue, TValue> updater) where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.AddOrUpdate(key, value, updater);
        }

        if (dict.TryGetValue(key, out var got))
        {
            var updateValue = updater.Invoke(key, got);
            dict[key] = updateValue;
            return updateValue;
        }

        dict.Add(key, value);
        return value;
    }

    /// <summary>
    /// 添加或更新值，若键不存在则添加值，否则通过委托更新已存在值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="value">待添加值</param>
    /// <param name="updater">值更新委托，入参(键,已存在值,待添加值)</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value,
        Func<TKey, TValue, TValue, TValue> updater) where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.AddOrUpdate(key, static (_, args) => args, updater, value);
        }

        if (dict.TryGetValue(key, out var got))
        {
            var updateValue = updater.Invoke(key, got, value);
            dict[key] = updateValue;
            return updateValue;
        }

        dict.Add(key, value);
        return value;
    }

    /// <summary>
    /// 添加或更新值，若键不存在则创建并添加值，否则通过委托更新已存在值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="creator">值生成委托，入参(键,额外参数)</param>
    /// <param name="updater">值更新委托，入参(键,已存在值,额外参数)</param>
    /// <param name="args">额外参数</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static TValue AddOrUpdate<TKey, TValue, TArgs>(this IDictionary<TKey, TValue> dict, TKey key,
        [RequireStaticDelegate] Func<TKey, TArgs, TValue> creator, Func<TKey, TValue, TArgs, TValue> updater,
        TArgs args)
        where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.AddOrUpdate(key, creator, updater, args);
        }

        if (dict.TryGetValue(key, out var got))
        {
            var updateValue = updater.Invoke(key, got, args);
            dict[key] = updateValue;
            return updateValue;
        }

        var created = creator.Invoke(key, args);
        dict.Add(key, created);
        return created;
    }

    /// <summary>
    /// 批量添加或更新值，若键不存在则添加值，否则通过委托更新已存在值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">待添加键值对序列</param>
    /// <param name="updater">值更新委托，入参(键,已存在值)</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range, Func<TKey, TValue, TValue> updater)
        where TKey : notnull
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict.AddOrUpdate(key, value, updater);
        }
    }

    /// <summary>
    /// 批量添加或更新值，若键不存在则添加值，否则通过委托更新已存在值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">待添加键值对序列</param>
    /// <param name="updater">值更新委托，入参(键,已存在值,待添加值)</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range, Func<TKey, TValue, TValue, TValue> updater)
        where TKey : notnull
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict.AddOrUpdate(key, static (_, args) => args, updater, value);
        }
    }

    /// <summary>
    /// 删除全部指定键集合对应的键值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="keys">待删除键序列</param>
    /// <returns>成功删除数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int RemoveKeys<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<TKey> keys)
    {
        if (dict is not { Count: > 0 }) return 0;

        if (keys.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        // 不使用Linq，避免闭包
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var key in keys)
        {
            if (dict.Remove(key)) count++;
        }

        return count;
    }

    /// <summary>
    /// 删除全部指定键集合对应的键值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="keys">待删除键序列</param>
    /// <param name="removed">被删除元素字典</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void RemoveKeys<TKey, TValue, TDictionary>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<TKey> keys, out TDictionary? removed) where TKey : notnull
        where TDictionary : IDictionary<TKey, TValue>, new()
    {
        removed = default;
        if (dict is not { Count: > 0 }) return;

        if (keys.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var key in keys)
        {
            removed ??= new TDictionary();
            if (!dict.Remove(key, out var value)) continue;

            removed.Add(key, value);
        }
    }

    /// <summary>
    /// 尝试从嵌套字典中获取指定数据
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key1">键1</param>
    /// <param name="key2">键2</param>
    /// <param name="value">获取值</param>
    /// <returns>是否成功获取</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public static bool TryGetNestedValue<TKey1, TKey2, TValue>(
        this IReadOnlyDictionary<TKey1, Dictionary<TKey2, TValue>> dict,
        TKey1 key1, TKey2 key2, [MaybeNullWhen(false)] out TValue value) where TKey1 : notnull where TKey2 : notnull
    {
        if (dict.TryGetValue(key1, out var nested))
        {
            return nested.TryGetValue(key2, out value);
        }

        value = default;
        return false;
    }

    /// <summary>
    /// 获取嵌套字典的所有键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <returns>(键1,键2)序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    public static IEnumerable<(TKey1, TKey2)> NestedKeys<TKey1, TKey2, TValue>(
        this IReadOnlyDictionary<TKey1, Dictionary<TKey2, TValue>> dict) where TKey1 : notnull where TKey2 : notnull
    {
        foreach (var (key1, nested) in dict)
        {
            foreach (var key2 in nested.Keys)
            {
                yield return (key1, key2);
            }
        }
    }

    /// <summary>
    /// 获取嵌套字典所有值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <returns>值序列</returns>
    [Pure, LinqTunnel, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TValue> NestedValues<TKey1, TKey2, TValue>(
        this IReadOnlyDictionary<TKey1, Dictionary<TKey2, TValue>> dict) where TKey1 : notnull where TKey2 : notnull =>
        dict.Values.SelectMany(nested => nested.Values);

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

    /// <summary>
    /// 将字典转为只读字典
    /// </summary>
    /// <param name="dict">字典</param>
    /// <returns>只读字典</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dict) =>
        dict as IReadOnlyDictionary<TKey, TValue> ?? new ReadOnlyDictionaryBridge<TKey, TValue>(dict);

    /// <summary>只读字典桥接</summary>
    private sealed class ReadOnlyDictionaryBridge<TKey, TValue>(IDictionary<TKey, TValue> dict)
        : IReadOnlyDictionary<TKey, TValue>
    {
        /// <inheritdoc />
        public int Count => dict.Count;

        /// <inheritdoc />
        public IEnumerable<TKey> Keys => dict.Keys;

        /// <inheritdoc />
        public IEnumerable<TValue> Values => dict.Values;

        /// <inheritdoc />
        public TValue this[TKey key] => dict[key];

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

#pragma warning disable CS8767
        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => dict.TryGetValue(key, out value);
#pragma warning restore CS8767

        /// <inheritdoc />
        [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();

        /// <inheritdoc />
        [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}