#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
#endif
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 字典算术扩展类
/// </summary>
[PublicAPI]
public static class DictionaryArithmeticExtensions
{
    #region 加法操作

    /// <summary>
    /// 将字典中所有值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void AllPlus<TKey>(this IDictionary<TKey, int> dict, int addend)
    {
        if (dict is not { Count: > 0 }) return;

        foreach (var key in dict.Keys)
        {
            dict.Plus(key, addend);
        }
    }

    /// <summary>
    /// 将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <returns>加法和</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Plus<TKey>(this IDictionary<TKey, int> dict, TKey key, int addend) => dict[key] += addend;

    /// <summary>
    /// 尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryPlus<TKey>(this IDictionary<TKey, int> dict, TKey key, int addend)
    {
        if (!dict.TryGetValue(key, out var got)) return false;

        dict[key] = got + addend;
        return true;
    }

    /// <summary>
    /// 尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <param name="sum">加法和</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryPlus<TKey>(this IDictionary<TKey, int> dict, TKey key, int addend, out int sum)
    {
        if (!dict.TryGetValue(key, out var got))
        {
            sum = default;
            return false;
        }

        dict[key] = sum = got + addend;
        return true;
    }

    /// <summary>
    /// 批量尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被加键值对序列</param>
    /// <returns>成功操作数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int TryPlusRange<TKey>(this IDictionary<TKey, int> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, int>> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, addend) in range)
        {
            if (dict.TryPlus(key, addend)) count++;
        }

        return count;
    }

    /// <summary>
    /// 添加键值，若键已存在，则将添加值与已存在值进行加法计算保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static int AddOrPlus<TKey>(this IDictionary<TKey, int> dict, TKey key, int addend) where TKey : notnull
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, int> concurrent)
        {
            return concurrent.AddOrUpdate(key, static (_, args) => args,
                static (_, existed, args) => existed + args, addend);
        }

        if (dict.TryGetValue(key, out var got))
        {
            addend += got;
        }

        dict[key] = addend;
        return addend;
    }

    /// <summary>
    /// 批量添加键值，若键已存在，则将添加值与已存在值进行加法计算保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被加键值对序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static void AddOrPlusRange<TKey>(this IDictionary<TKey, int> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, int>> range) where TKey : notnull
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict.AddOrPlus(key, value);
        }
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// 将字典中所有值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void AllPlus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TOther addend)
        where TValue : IAdditionOperators<TValue, TOther, TValue> where TOther : notnull
    {
        if (dict is not { Count: > 0 }) return;

        foreach (var key in dict.Keys)
        {
            dict.Plus(key, addend);
        }
    }

    /// <summary>
    /// 将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <returns>加法和</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue Plus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key, TOther addend)
        where TValue : IAdditionOperators<TValue, TOther, TValue> where TOther : notnull => dict[key] += addend;

    /// <summary>
    /// 尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryPlus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key, TOther addend)
        where TValue : IAdditionOperators<TValue, TOther, TValue> where TOther : notnull
    {
        if (!dict.TryGetValue(key, out var got)) return false;

        dict[key] = got + addend;
        return true;
    }

    /// <summary>
    /// 尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    /// <param name="sum">加法和</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryPlus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key, TOther addend,
        [MaybeNullWhen(false)] out TValue sum) where TValue : IAdditionOperators<TValue, TOther, TValue>
        where TOther : notnull
    {
        if (!dict.TryGetValue(key, out var got))
        {
            sum = default;
            return false;
        }

        dict[key] = sum = got + addend;
        return true;
    }

    /// <summary>
    /// 批量尝试将字典中指定键的值加上传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被加键值对序列</param>
    /// <returns>成功操作数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int TryPlusRange<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TOther>> range)
        where TValue : IAdditionOperators<TValue, TOther, TValue>
        where TOther : notnull
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, addend) in range)
        {
            if (dict.TryPlus(key, addend)) count++;
        }

        return count;
    }

    /// <summary>
    /// 添加键值，若键已存在，则将添加值与已存在值进行加法计算保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="addend">被加值</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static TValue AddOrPlus<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue addend)
        where TKey : notnull where TValue : IAdditionOperators<TValue, TValue, TValue>
    {
        // 并发字典调用并发方法
        if (dict is ConcurrentDictionary<TKey, TValue> concurrent)
        {
            return concurrent.AddOrUpdate(key, static (_, args) => args,
                static (_, existed, args) => existed + args, addend);
        }

        if (dict.TryGetValue(key, out var got))
        {
            addend += got;
        }

        dict[key] = addend;
        return addend;
    }

    /// <summary>
    /// 批量添加键值，若键已存在，则将添加值与已存在值进行加法计算保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被加键值对序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent | CollectionAccessType.ModifyExistingContent)]
    public static void AddOrPlusRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range) where TKey : notnull
        where TValue : IAdditionOperators<TValue, TValue, TValue>
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return;
        }

        foreach (var (key, value) in range)
        {
            dict.AddOrPlus(key, value);
        }
    }
#endif

    #endregion
}