#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 字典帮助类
/// </summary>
[PublicAPI]
public static class DictionaryHelper
{
    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值加上传入值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    /// <returns>新字典</returns>
    [Pure]
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

#if NET7_0_OR_GREATER
    /// <summary>
    /// 从传入字典构造新字典，并将字典中的值加上传入值
    /// </summary>
    /// <param name="dict">字典</param>
    /// <param name="addend">被加值</param>
    /// <returns>新字典</returns>
    [Pure]
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
#endif
}