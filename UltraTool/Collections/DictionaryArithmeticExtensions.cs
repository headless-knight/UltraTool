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

    #region 减法操作

#if NET7_0_OR_GREATER
    /// <summary>
    /// 将字典中所有键的值减去传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="minuend">被减值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void AllMinus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TOther minuend)
        where TValue : ISubtractionOperators<TValue, TOther, TValue> where TOther : notnull
    {
        if (dict is not { Count: > 0 }) return;

        foreach (var key in dict.Keys)
        {
            dict.Minus(key, minuend);
        }
    }

    /// <summary>
    /// 将字典中指定键的值减去传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <returns>差值</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue Minus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key,
        TOther minuend) where TValue : ISubtractionOperators<TValue, TOther, TValue> where TOther : notnull =>
        dict[key] -= minuend;

    /// <summary>
    /// 尝试将字典中指定键的值减去传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryMinus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key,
        TOther minuend) where TValue : ISubtractionOperators<TValue, TOther, TValue> where TOther : notnull
    {
        if (!dict.TryGetValue(key, out var got)) return false;

        dict[key] = got - minuend;
        return true;
    }

    /// <summary>
    /// 尝试将字典中指定键的值减去传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <param name="diff">差值</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryMinus<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict, TKey key,
        TOther minuend, [MaybeNullWhen(false)] out TValue diff)
        where TValue : ISubtractionOperators<TValue, TOther, TValue>
        where TOther : notnull
    {
        if (!dict.TryGetValue(key, out var got))
        {
            diff = default;
            return false;
        }

        dict[key] = diff = got - minuend;
        return true;
    }

    /// <summary>
    /// 尝试批量将字典中指定键的值减去传入值并保存
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被减键值对序列</param>
    /// <returns>成功操作数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int TryMinusRange<TKey, TValue, TOther>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TOther>> range)
        where TValue : ISubtractionOperators<TValue, TOther, TValue> where TOther : notnull
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, value) in range)
        {
            if (dict.TryMinus(key, value)) count++;
        }

        return count;
    }

    /// <summary>
    /// 将字典中所有键的值减去传入值并保存，若计算结果小于或等于0则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="minuend">被减值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void AllMinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue minuend)
        where TValue : INumber<TValue>
    {
        if (dict is not { Count: > 0 }) return;

        foreach (var key in dict.Keys)
        {
            dict.MinusOrRemove(key, minuend);
        }
    }

    /// <summary>
    /// 将字典中所有键的值减去传入值并保存，若计算结果满足条件则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="minuend">被减值</param>
    /// <param name="condition">删除条件</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void AllMinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue minuend,
        Func<TValue, bool> condition) where TValue : ISubtractionOperators<TValue, TValue, TValue>
    {
        if (dict is not { Count: > 0 }) return;

        foreach (var key in dict.Keys)
        {
            dict.MinusOrRemove(key, minuend, condition);
        }
    }

    /// <summary>
    /// 将字典中指定键的值减去传入值并保存，若计算结果小于或等于0则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue minuend)
        where TValue : INumber<TValue>
    {
        if (!dict.TryMinusOrRemove(key, minuend))
        {
            throw new KeyNotFoundException($"指定键：{key}在字典中不存在");
        }
    }

    /// <summary>
    /// 将字典中指定键的值减去传入值并保存，若计算结果满足条件则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <param name="condition">删除条件</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static void MinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue minuend,
        Func<TValue, bool> condition) where TValue : ISubtractionOperators<TValue, TValue, TValue>
    {
        if (!dict.TryMinusOrRemove(key, minuend, condition))
        {
            throw new KeyNotFoundException($"指定键：{key}在字典中不存在");
        }
    }

    /// <summary>
    /// 将字典中指定键的值减去传入值并保存，若计算结果小于或等于0则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryMinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        TValue minuend) where TValue : INumber<TValue>
    {
        if (!dict.TryGetValue(key, out var got)) return false;

        var result = got - minuend;
        if (result <= TValue.Zero)
        {
            dict.Remove(key);
            return true;
        }

        dict[key] = result;
        return true;
    }

    /// <summary>
    /// 将字典中指定键的值减去传入值并保存，若计算结果满足条件则删除该键
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">被减值</param>
    /// <param name="condition">删除条件</param>
    /// <returns>是否操作成功</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static bool TryMinusOrRemove<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        TValue minuend, Func<TValue, bool> condition) where TValue : ISubtractionOperators<TValue, TValue, TValue>
    {
        if (!dict.TryGetValue(key, out var got)) return false;

        var result = got - minuend;
        if (condition.Invoke(result))
        {
            dict.Remove(key);
            return true;
        }

        dict[key] = result;
        return true;
    }

    /// <summary>
    /// 遍历键值集合中的键值，对字典中对应键值数据进行减法计算保存，若计算结果小于或等于0则删除
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被减{键值对序列</param>
    /// <returns>成功操作数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int TryMinusOrRemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range) where TValue : INumber<TValue>
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, value) in range)
        {
            if (dict.TryMinusOrRemove(key, value)) count++;
        }

        return count;
    }

    /// <summary>
    /// 遍历键值集合中的键值，对字典中对应键值数据进行减法计算保存，若计算结果满足条件则删除
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">被减键值对序列</param>
    /// <param name="condition">删除条件</param>
    /// <returns>成功操作数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public static int TryMinusOrRemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> range, Func<TValue, bool> condition)
        where TValue : ISubtractionOperators<TValue, TValue, TValue>
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        foreach (var (key, value) in range)
        {
            if (dict.TryMinusOrRemove(key, value, condition)) count++;
        }

        return count;
    }

    /// <summary>
    /// 从字典减去指定值或添加指定值的负值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <param name="minuend">值</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent | CollectionAccessType.UpdatedContent)]
    public static TValue MinusOrAddNegative<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        TValue minuend)
        where TValue : ISubtractionOperators<TValue, TValue, TValue>, IUnaryNegationOperators<TValue, TValue>
    {
        if (dict.TryMinus(key, minuend, out var diff)) return diff;

        return dict[key] = -minuend;
    }

    /// <summary>
    /// 批量从字典减去指定值或添加指定值的负值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="range">键值对序列</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent | CollectionAccessType.UpdatedContent)]
    public static void MinusOrAddNegativeRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
        IEnumerable<KeyValuePair<TKey, TValue>> range)
        where TValue : ISubtractionOperators<TValue, TValue, TValue>, IUnaryNegationOperators<TValue, TValue>
    {
        foreach (var (key, value) in range)
        {
            dict.MinusOrAddNegative(key, value);
        }
    }
#endif

    #endregion
}