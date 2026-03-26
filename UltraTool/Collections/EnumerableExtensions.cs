using System.Diagnostics.CodeAnalysis;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;
using UltraTool.Randoms;

namespace UltraTool.Collections;

/// <summary>
/// 序列拓展类
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// 判断序列中是否有为null的元素
    /// </summary>
    /// <param name="source">序列</param>
    /// <returns>是否有为null的元素</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAnyNull<T>([InstantHandle] this IEnumerable<T?> source) =>
        source.Any(static item => item == null);

    /// <summary>
    /// 判断序列中是否全为null的元素
    /// </summary>
    /// <param name="source">序列</param>
    /// <returns>是否全为null的元素</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAllNull<T>([InstantHandle] this IEnumerable<T?> source) =>
        source.All(static item => item == null);

    /// <summary>
    /// 判断序列是否有序，空序列与单元素序列视为有序
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparison">比较表达式</param>
    /// <returns>是否已排序</returns>
    public static bool IsOrdered<T>([InstantHandle] this IEnumerable<T> source, Comparison<T> comparison)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 1)
        {
            return true;
        }

        return IsOrderedInternal(source, new ValueComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// 判断序列是否有序，空序列与单元素序列视为有序
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>是否已排序</returns>
    public static bool IsOrdered<T>([InstantHandle] this IEnumerable<T> source, IComparer<T>? comparer = null)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 1)
        {
            return true;
        }

        return IsOrderedInternal(source, comparer ?? Comparer<T>.Default);
    }

    /// <summary>判断序列是否有序，内部实现</summary>
    internal static bool IsOrderedInternal<T, TComparer>([InstantHandle] IEnumerable<T> source, TComparer comparer)
        where TComparer : IComparer<T>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return true;

        var prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            if (comparer.Compare(prev, enumerator.Current) > 0)
            {
                return false;
            }

            prev = enumerator.Current;
        }

        return true;
    }

    /// <summary>
    /// 判断序列是否降序有序，空序列与单元素序列视为有序
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparison">比较表达式</param>
    /// <returns>是否降序有序</returns>
    public static bool IsOrderedDescending<T>([InstantHandle] this IEnumerable<T> source, Comparison<T> comparison)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 1)
        {
            return true;
        }

        return IsOrderedDescendingInternal(source, new ValueComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// 判断序列是否降序有序，空序列与单元素序列视为有序
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>是否降序有序</returns>
    public static bool IsOrderedDescending<T>([InstantHandle] this IEnumerable<T> source, IComparer<T>? comparer = null)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 1)
        {
            return true;
        }

        return IsOrderedDescendingInternal(source, comparer ?? Comparer<T>.Default);
    }

    /// <summary>判断序列是否降序有序，内部实现</summary>
    private static bool IsOrderedDescendingInternal<T, TComparer>([InstantHandle] IEnumerable<T> source,
        TComparer comparer) where TComparer : IComparer<T>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return true;

        var prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            if (comparer.Compare(enumerator.Current, prev) > 0)
            {
                return false;
            }

            prev = enumerator.Current;
        }

        return true;
    }

    /// <summary>
    /// 获取序列中的最小值与最大值
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparison">比较表达式</param>
    /// <returns>(最小值,最大值)</returns>
    public static (T, T) MinMax<T>([InstantHandle] this IEnumerable<T> source, Comparison<T> comparison)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            throw new InvalidOperationException("Enumerable is empty");
        }

        return MinMaxInternal(source, new ValueComparisonComparer<T>(comparison));
    }

    /// <summary>
    /// 获取序列中的最小值与最大值
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>(最小值,最大值)</returns>
    public static (T, T) MinMax<T>([InstantHandle] this IEnumerable<T> source, IComparer<T>? comparer = null)
    {
        if (source.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            throw new InvalidOperationException("Enumerable is empty");
        }

        return MinMaxInternal(source, comparer ?? Comparer<T>.Default);
    }

    /// <summary>获取序列中的最小值与最大值，内部实现</summary>
    private static (T, T) MinMaxInternal<T, TComparer>([InstantHandle] IEnumerable<T> source, TComparer comparer)
        where TComparer : IComparer<T>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("Enumerable is empty");

        var min = enumerator.Current;
        var max = enumerator.Current;
        while (enumerator.MoveNext())
        {
            if (comparer.Compare(min, enumerator.Current) > 0)
            {
                min = enumerator.Current;
            }

            if (comparer.Compare(max, enumerator.Current) < 0)
            {
                max = enumerator.Current;
            }
        }

        return (min, max);
    }

    /// <summary>
    /// 获取序列中的最小值与最大值，若序列为空则返回(default(T),default(T))
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>(最小值,最大值)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (T?, T?) MinMaxOrDefault<T>([InstantHandle] this IEnumerable<T> source,
        IComparer<T>? comparer = null) => source.MinMaxOrDefault(default!, comparer);

    /// <summary>
    /// 获取序列中的最小值与最大值，若序列为空则返回(默认值,默认值)
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="defaultValue">默认值</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>(最小值,最大值)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (T, T) MinMaxOrDefault<T>([InstantHandle] this IEnumerable<T> source, T defaultValue,
        IComparer<T>? comparer = null) => source.MinMaxOrDefault(defaultValue, defaultValue, comparer);

    /// <summary>
    /// 获取序列中的最小值与最大值，若序列为空则返回(默认最小值,默认最大值)
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="defaultMin">默认最小值</param>
    /// <param name="defaultMax">默认最大值</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>(最小值,最大值)</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (T, T) MinMaxOrDefault<T, TEnumerable>([InstantHandle] this TEnumerable source, T defaultMin,
        T defaultMax, IComparer<T>? comparer = null) where TEnumerable : IEnumerable<T> =>
        MinMaxOrDefaultInternal(source, defaultMin, defaultMax, comparer ?? Comparer<T>.Default);

    /// <summary>获取序列中的最小值与最大值，内部实现</summary>
    private static (T, T) MinMaxOrDefaultInternal<T, TEnumerable, TComparer>([InstantHandle] TEnumerable source,
        T defaultMin, T defaultMax, TComparer comparer)
        where TEnumerable : IEnumerable<T> where TComparer : IComparer<T>
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return (defaultMin, defaultMax);

        var min = enumerator.Current;
        var max = min;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (comparer.Compare(min, current) > 0)
            {
                min = current;
            }

            if (comparer.Compare(max, current) < 0)
            {
                max = current;
            }
        }

        return (min, max);
    }

    /// <summary>
    /// 遍历序列
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="action">遍历操作，入参(元素)</param>
    public static void ForEach<T>([InstantHandle] this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action.Invoke(item);
        }
    }

    /// <summary>
    /// 遍历序列
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="action">遍历操作，入参(元素,索引)</param>
    public static void ForEach<T>([InstantHandle] this IEnumerable<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in source)
        {
            action.Invoke(item, index++);
        }
    }

    /// <summary>
    /// 返回一个带索引的序列
    /// </summary>
    /// <param name="source">序列</param>
    /// <returns>(元素,索引)序列</returns>
    [LinqTunnel]
    public static IEnumerable<(int Index, T Item)> Index<T>(this IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source) yield return (index++, item);
    }

    /// <summary>
    /// 源序列每两个元素之间插入分隔元素，返回新序列
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="separator">分隔元素</param>
    /// <returns>操作后序列</returns>
    [LinqTunnel]
    public static IEnumerable<T> Join<T, TEnumerable>(this TEnumerable source, T separator)
        where TEnumerable : IEnumerable<T>
    {
        using var enumerator = source.GetEnumerator();
        var moveNext = enumerator.MoveNext();
        while (moveNext)
        {
            yield return enumerator.Current;
            moveNext = enumerator.MoveNext();
            if (moveNext) yield return separator;
        }
    }

    /// <summary>
    /// 替换源序列中指定值
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="oldValue">旧值</param>
    /// <param name="newValue">新值</param>
    /// <param name="comparer">相等比较器，默认为<see cref="EqualityComparer{T}.Default"/></param>
    /// <returns>操作后序列</returns>
    [LinqTunnel]
    public static IEnumerable<T> Replace<T, TEnumerable>(this TEnumerable source, T oldValue, T newValue,
        IEqualityComparer<T>? comparer = null) where TEnumerable : IEnumerable<T>
    {
        comparer ??= EqualityComparer<T>.Default;
        foreach (var item in source)
        {
            if (comparer.Equals(item, oldValue)) yield return newValue;

            yield return item;
        }
    }

    /// <summary>
    /// 替换源序列中指定值
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="oldValueSelector">旧值选择器,入参(遍历值)</param>
    /// <param name="newValueCreator">新值创建器,入参(待替换值)</param>
    /// <returns>操作后序列</returns>
    [LinqTunnel]
    public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, Func<T, bool> oldValueSelector,
        Func<T, T> newValueCreator)
    {
        foreach (var item in source)
        {
            if (!oldValueSelector.Invoke(item)) yield return item;

            yield return newValueCreator.Invoke(item);
        }
    }

    /// <summary>
    /// 判断源序列是否以指定序列开头，若指定序列为空，则返回true
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="values">指定序列</param>
    /// <param name="comparer">相等比较器，默认为<see cref="EqualityComparer{T}.Default"/></param>
    /// <returns>是否以指定序列开头</returns>
    public static bool StartsWith<T>([InstantHandle] this IEnumerable<T> source, [InstantHandle] IEnumerable<T> values,
        IEqualityComparer<T>? comparer = null)
    {
        if (values.TryGetNonEnumeratedCount(out var valuesSize))
        {
            if (valuesSize <= 0) return true;

            if (source.TryGetNonEnumeratedCount(out var sourceSize) && sourceSize < valuesSize)
            {
                return false;
            }
        }

        comparer ??= EqualityComparer<T>.Default;
        using var sourceEnumerator = source.GetEnumerator();
        using var valuesEnumerator = values.GetEnumerator();
        while (valuesEnumerator.MoveNext())
        {
            if (!sourceEnumerator.MoveNext()) return false;

            if (!comparer.Equals(sourceEnumerator.Current, valuesEnumerator.Current)) return false;
        }

        return true;
    }

    /// <summary>
    /// 判断源序列是否以指定序列结尾，若指定序列为空，则返回true
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="values">指定序列</param>
    /// <param name="comparer">相等比较器，默认为<see cref="EqualityComparer{T}.Default"/></param>
    /// <returns>是否以指定序列结尾</returns>
    public static bool EndsWith<T>([InstantHandle] this IEnumerable<T> source, [InstantHandle] IEnumerable<T> values,
        IEqualityComparer<T>? comparer = null)
    {
        if (values.TryGetNonEnumeratedCount(out var valuesSize))
        {
            if (valuesSize <= 0) return true;

            if (source.TryGetNonEnumeratedCount(out var sourceSize) && sourceSize < valuesSize)
            {
                return false;
            }
        }

        return source.Reverse().StartsWith(values.Reverse(), comparer);
    }

    /// <summary>
    /// 根据元素出现次数，返回一个{元素:个数}的字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <returns>{元素:个数}字典</returns>
    public static Dictionary<T, int> CountMap<T>([InstantHandle] this IEnumerable<T> source) where T : notnull
    {
        var dict = new Dictionary<T, int>();
        foreach (var item in source)
        {
            dict[item] = dict.GetValueOrDefault(item, 0) + 1;
        }

        return dict;
    }

    /// <summary>
    /// 根据元素出现次数，返回一个{元素:个数}的字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="result">{元素:个数}字典</param>
    public static void CountMap<T, TDictionary>([InstantHandle] this IEnumerable<T> source,
        out TDictionary result) where T : notnull where TDictionary : IDictionary<T, int>, new()
    {
        result = new TDictionary();
        foreach (var item in source)
        {
            if (result.TryGetValue(item, out var count))
            {
                result[item] = count + 1;
            }
            else
            {
                result.Add(item, 1);
            }
        }
    }

    /// <summary>
    /// 尝试从序列中查找符合条件的元素
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="match">条件委托</param>
    /// <param name="found">找到的元素</param>
    /// <returns>是否成功找到</returns>
    public static bool TryFind<T>([InstantHandle] this IEnumerable<T> source, Predicate<T> match,
        [MaybeNullWhen(false)] out T found)
    {
        foreach (var item in source)
        {
            if (!match.Invoke(item)) continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }

    #region 合并字典

#if NET7_0_OR_GREATER
    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则将值相加保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器，入参(遍历值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, TValue> ToMergedDictionary<TKey, TValue>(
        [InstantHandle] this IEnumerable<TValue> source, Func<TValue, TKey> keySelector) where TKey : notnull
        where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        source.ToMergedDictionary(keySelector, static (_, value, addValue) => value + addValue);

    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则将值相加保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, TValue> ToMergedDictionary<TSource, TKey, TValue>(
        [InstantHandle] this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        Func<TSource, TValue> valueSelector) where TKey : notnull
        where TValue : IAdditionOperators<TValue, TValue, TValue> =>
        source.ToMergedDictionary(keySelector, valueSelector, static (_, value, addValue) => value + addValue);
#else
    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则将值相加保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, int> ToMergedDictionary<TSource, TKey>(
        [InstantHandle] this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        Func<TSource, int> valueSelector) where TKey : notnull =>
        source.ToMergedDictionary(keySelector, valueSelector, static (_, value, addValue) => value + addValue);

    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则将值相加保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, long> ToMergedDictionary<TSource, TKey>(
        [InstantHandle] this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        Func<TSource, long> valueSelector) where TKey : notnull =>
        source.ToMergedDictionary(keySelector, valueSelector, static (_, value, addValue) => value + addValue);
#endif

    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则通过合并器合并后保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="merger">合并器，入参(键,已添加值,待添加值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, TValue> ToMergedDictionary<TKey, TValue>(
        [InstantHandle] this IEnumerable<TValue> source, Func<TValue, TKey> keySelector,
        Func<TKey, TValue, TValue, TValue> merger) where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(source.GetCountOrZero());
        foreach (var value in source)
        {
            var key = keySelector.Invoke(value);
            if (!result.TryGetValue(key, out var got))
            {
                result.Add(key, value);
                continue;
            }

            result[key] = merger.Invoke(key, got, value);
        }

        return result;
    }

    /// <summary>
    /// 将序列转换为字典，构建字典过程中若存在重复的键，则通过合并器合并后保存
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <param name="merger">合并器，入参(键,已添加值,待添加值)</param>
    /// <returns>字典</returns>
    public static Dictionary<TKey, TValue> ToMergedDictionary<TSource, TKey, TValue>(
        [InstantHandle] this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
        Func<TSource, TValue> valueSelector, Func<TKey, TValue, TValue, TValue> merger) where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(source.GetCountOrZero());
        foreach (var item in source)
        {
            var key = keySelector.Invoke(item);
            var value = valueSelector.Invoke(item);
            if (!result.TryGetValue(key, out var got))
            {
                result.Add(key, value);
                continue;
            }

            result[key] = merger.Invoke(key, got, value);
        }

        return result;
    }

    #endregion

    #region 嵌套字典

    /// <summary>
    /// 将序列转为指定两个键的嵌套字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键1选择器，入参(遍历值)</param>
    /// <param name="key2Selector">键2选择器，入参(遍历值)</param>
    /// <returns>{键1:{键2：值}}</returns>
    public static Dictionary<TKey1, Dictionary<TKey2, TValue>> ToNestedDictionary<TKey1, TKey2, TValue>(
        [InstantHandle] this IEnumerable<TValue> source, Func<TValue, TKey1> keySelector,
        Func<TValue, TKey2> key2Selector) where TKey1 : notnull where TKey2 : notnull
    {
        var dict = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
        foreach (var item in source)
        {
            var key1 = keySelector.Invoke(item);
            var nested = dict.GetOrCreate(key1);
            var key2 = key2Selector.Invoke(item);
            nested.Add(key2, item);
        }

        return dict;
    }

    /// <summary>
    /// 键序列转为指定两个键嵌套的字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="key1Selector">键1选择器，入参(遍历值)</param>
    /// <param name="key2Selector">键2选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <returns>{键1:{键2：值}字典}字典</returns>
    public static Dictionary<TKey1, Dictionary<TKey2, TValue>> ToNestedDictionary<TSource, TKey1, TKey2, TValue>(
        [InstantHandle] this IEnumerable<TSource> source, Func<TSource, TKey1> key1Selector,
        Func<TSource, TKey2> key2Selector, Func<TSource, TValue> valueSelector)
        where TKey1 : notnull where TKey2 : notnull
    {
        var dict = new Dictionary<TKey1, Dictionary<TKey2, TValue>>();
        foreach (var item in source)
        {
            var key1 = key1Selector.Invoke(item);
            var nested = dict.GetOrCreate(key1);
            var key2 = key2Selector.Invoke(item);
            nested.Add(key2, valueSelector.Invoke(item));
        }

        return dict;
    }

    /// <summary>
    /// 将序列转为指定两个键的嵌套只读字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="keySelector">键1选择器，入参(遍历值)</param>
    /// <param name="key2Selector">键2选择器，入参(遍历值)</param>
    /// <returns>{键1:{键2：值}}</returns>
    public static IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>> ToNestedReadOnlyDictionary<
        TKey1, TKey2, TValue>([InstantHandle] this IEnumerable<TValue> source, Func<TValue, TKey1> keySelector,
        Func<TValue, TKey2> key2Selector) where TKey1 : notnull where TKey2 : notnull
    {
        var dict = new Dictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>();
        foreach (var item in source)
        {
            var key1 = keySelector.Invoke(item);
            if (!dict.TryGetValue(key1, out var nested))
            {
                nested = new Dictionary<TKey2, TValue>();
                dict.Add(key1, nested);
            }

            var key2 = key2Selector.Invoke(item);
            ((Dictionary<TKey2, TValue>)nested).Add(key2, item);
        }

        return dict;
    }

    /// <summary>
    /// 键序列转为指定两个键嵌套的字典
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="key1Selector">键1选择器，入参(遍历值)</param>
    /// <param name="key2Selector">键2选择器，入参(遍历值)</param>
    /// <param name="valueSelector">值选择器，入参(遍历值)</param>
    /// <returns>{键1:{键2：值}字典}字典</returns>
    public static IReadOnlyDictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>> ToNestedReadOnlyDictionary<
        TSource, TKey1, TKey2, TValue>([InstantHandle] this IEnumerable<TSource> source,
        Func<TSource, TKey1> key1Selector, Func<TSource, TKey2> key2Selector,
        Func<TSource, TValue> valueSelector) where TKey1 : notnull where TKey2 : notnull
    {
        var dict = new Dictionary<TKey1, IReadOnlyDictionary<TKey2, TValue>>();
        foreach (var item in source)
        {
            var key1 = key1Selector.Invoke(item);
            if (!dict.TryGetValue(key1, out var nested))
            {
                nested = new Dictionary<TKey2, TValue>();
                dict.Add(key1, nested);
            }

            var key2 = key2Selector.Invoke(item);
            ((Dictionary<TKey2, TValue>)nested).Add(key2, valueSelector.Invoke(item));
        }

        return dict;
    }

    #endregion

    /// <summary>
    /// 从源序列中随机位置开始取指定数量的元素
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="count">获取数量</param>
    /// <returns>结果序列</returns>
    [LinqTunnel]
    public static IEnumerable<T> RandomSlice<T>([InstantHandle] this IEnumerable<T> source, int count)
    {
        if (!source.TryGetNonEnumeratedCount(out var size)) return RandomSliceInternal(source, count);

        if (count >= size) return source;

        var canSkip = size - count;
        var skip = RandomHelper.Shared.Next(0, canSkip + 1);
        return source.Skip(skip).Take(count);
    }

    /// <summary>从源序列中随机位置开始取指定数量的元素，内部实现</summary>
    private static IEnumerable<T> RandomSliceInternal<T>([InstantHandle] IEnumerable<T> source, int count)
    {
        var array = source.ToArray();
        if (count >= array.Length)
        {
            foreach (var item in array)
            {
                yield return item;
            }

            yield break;
        }

        var canSkip = array.Length - count;
        var skip = RandomHelper.Shared.Next(0, canSkip + 1);
        for (var i = skip; i < array.Length; i++)
        {
            yield return array[i];
        }
    }

    /// <summary>
    /// 尝试在不使用枚举器的情况下获取数量，否则返回0
    /// </summary>
    /// <param name="source">源序列</param>
    /// <returns>元素数量或0</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetCountOrZero<T>([NoEnumeration] this IEnumerable<T> source) =>
        source.TryGetNonEnumeratedCount(out var count) ? count : 0;

    /// <summary>指定数组容量，将序列转为数组</summary>
    internal static T[] ToArrayWithCapacity<T>([InstantHandle] this IEnumerable<T> source, int capacity)
    {
        var array = new T[capacity];
        var index = 0;
        foreach (var item in source)
        {
            array[index++] = item;
        }

        return array;
    }

#if !NET6_0_OR_GREATER
    /// <summary>
    /// 获取序列第一个元素，若序列为空则返回默认值
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>第一个元素或默认值</returns>
    public static T FirstOrDefault<T>([InstantHandle] this IEnumerable<T> source, T defaultValue)
    {
        using var enumerator = source.GetEnumerator();
        return enumerator.MoveNext() ? enumerator.Current : defaultValue;
    }

    /// <summary>
    /// 尝试不使用枚举器获取序列元素数量
    /// </summary>
    /// <param name="source">序列</param>
    /// <param name="count">元素数量</param>
    /// <returns>是否成功获取</returns>
    public static bool TryGetNonEnumeratedCount<T>([NoEnumeration] this IEnumerable<T> source, out int count)
    {
        switch (source)
        {
            case ICollection<T> coll:
            {
                count = coll.Count;
                return true;
            }
            case IReadOnlyCollection<T> coll:
            {
                count = coll.Count;
                return true;
            }
            default:
            {
                count = 0;
                return false;
            }
        }
    }
#endif
}