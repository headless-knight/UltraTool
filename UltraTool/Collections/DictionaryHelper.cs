using JetBrains.Annotations;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace UltraTool.Collections;

/// <summary>
/// 字典帮助类
/// </summary>
public static class DictionaryHelper
{
    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值加上传入值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, int> PlusOf<TKey>(IReadOnlyDictionary<TKey, int> dict, int addend)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, int>(dict.Count);
        if (dict is not { Count: > 0 }) return result;

        foreach (var (key, value) in dict)
        {
            result.Add(key, value + addend);
        }

        return result;
    }

    /// <summary>
    /// 将两个键值对序列合并为新字典，如果键已存在则将值相加保存
    /// </summary>
    /// <param name="pairs1">键值对序列1</param>
    /// <param name="pairs2">键值对序列2</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, int> AddOrPlusOf<TKey>(
        [InstantHandle] IEnumerable<KeyValuePair<TKey, int>> pairs1,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, int>> pairs2) where TKey : notnull
    {
        var result = new Dictionary<TKey, int>();
        result.AddOrPlusRange(pairs1);
        result.AddOrPlusRange(pairs2);
        return result;
    }

    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值减去传入的减值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="minuend">被减值</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, int> MinusOf<TKey>(IReadOnlyDictionary<TKey, int> dict, int minuend)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, int>(dict.Count);
        if (dict is not { Count: > 0 }) return result;

        foreach (var (key, value) in dict)
        {
            result.Add(key, value - minuend);
        }

        return result;
    }

    /// <summary>
    /// 从传入字典构造新字典，并遍历键值对序列将字典中相同键的值减去键值对值或加上负值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="pairs">键值对序列</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, int> MinusOrAddNegative<TKey>(IReadOnlyDictionary<TKey, int> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, int>> pairs) where TKey : notnull
    {
        var result = new Dictionary<TKey, int>(dict);
        result.MinusOrAddNegativeRange(pairs);
        return result;
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值加上传入值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, TValue> PlusOf<TKey, TValue, TOther>(IReadOnlyDictionary<TKey, TValue> dict,
        TOther addend) where TKey : notnull
        where TValue : IAdditionOperators<TValue, TOther, TValue>
        where TOther : notnull
    {
        var result = new Dictionary<TKey, TValue>(dict.Count);
        if (dict is not { Count: > 0 }) return result;

        foreach (var (key, value) in dict)
        {
            result.Add(key, value + addend);
        }

        return result;
    }

    /// <summary>
    /// 将两个键值对序列合并为新字典，如果键已存在则将值相加保存
    /// </summary>
    /// <param name="pairs1">键值对序列1</param>
    /// <param name="pairs2">键值对序列2</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, TValue> AddOrPlusOf<TKey, TValue>(
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> pairs1,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> pairs2) where TKey : notnull
        where TValue : IAdditionOperators<TValue, TValue, TValue>
    {
        var result = new Dictionary<TKey, TValue>();
        result.AddOrPlusRange(pairs1);
        result.AddOrPlusRange(pairs2);
        return result;
    }

    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值减去传入的减值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="minuend">被减值</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, TValue> MinusOf<TKey, TValue, TOther>(IReadOnlyDictionary<TKey, TValue> dict,
        TOther minuend) where TKey : notnull
        where TValue : ISubtractionOperators<TValue, TOther, TValue>
        where TOther : notnull
    {
        var result = new Dictionary<TKey, TValue>(dict.Count);
        if (dict is not { Count: > 0 }) return result;

        foreach (var (key, value) in dict)
        {
            result.Add(key, value - minuend);
        }

        return result;
    }

    /// <summary>
    /// 从传入字典构造新字典，并遍历键值对序列将字典中相同键的值减去键值对值或加上负值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="pairs">键值对序列</param>
    /// <returns>新字典</returns>
    public static Dictionary<TKey, TValue> MinusOrAddNegative<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> dict,
        [InstantHandle] IEnumerable<KeyValuePair<TKey, TValue>> pairs) where TKey : notnull
        where TValue : ISubtractionOperators<TValue, TValue, TValue>, IUnaryNegationOperators<TValue, TValue>
    {
        var result = new Dictionary<TKey, TValue>(dict);
        result.MinusOrAddNegativeRange(pairs);
        return result;
    }
#endif
}