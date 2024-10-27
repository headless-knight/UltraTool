using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Helpers;

namespace UltraTool.Randoms;

/// <summary>
/// 随机拓展类
/// </summary>
[PublicAPI]
public static class RandomExtensions
{
    #region 拓展类型支持

    /// <summary>
    /// 随机获取布尔值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机布尔值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(this Random random) => random.Next(2) == 0;

    /// <summary>
    /// 随机获取字节，随机结果大于等于0
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机字节</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NextByte(this Random random) => (byte)random.Next(0, byte.MaxValue + 1);

    /// <summary>
    /// 随机获取字节<br/>
    /// 随机结果大于等于0，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机字节</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NextByte(this Random random, byte maxValue) => random.NextByte(0, maxValue);

    /// <summary>
    /// 随机获取字节<br/>
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机字节</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NextByte(this Random random, byte minValue, byte maxValue) =>
        (byte)random.Next(minValue, maxValue);

    /// <summary>
    /// 获取随机字符
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机字符</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char NextChar(this Random random) =>
        (char)random.Next(char.MinValue, char.MaxValue + 1);

    /// <summary>
    /// 获取随机字符<br/>
    /// 随机结果大于等于0，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机字符</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char NextChar(this Random random, char maxValue) => random.NextChar(char.MinValue, maxValue);

    /// <summary>
    /// 获取随机字符<br/>
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机字符</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char NextChar(this Random random, char minValue, char maxValue) =>
        (char)random.Next(minValue, maxValue);

#if !NET6_0_OR_GREATER
    /// <summary>
    /// 获取随机64位整型<br/>
    /// 随机结果大于等于0，小于<see cref="long.MaxValue"/>
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机64位整型</returns>
    public static long NextInt64(this Random random)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        var next = long.MaxValue;
        while (next == long.MaxValue)
        {
            RandomNumberGenerator.Fill(bytes);
            next = BitConverter.ToInt64(bytes);
        }

        return Math.Abs(next);
    }

    /// <summary>
    /// 获取随机64位整型<br/>
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机64位整型</returns>
    public static long NextInt64(this Random random, long minValue, long maxValue)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long) * 2];
        RandomNumberGenerator.Fill(bytes);
        var randValue = new BigInteger(bytes);
        var minBigInt = new BigInteger(minValue);
        var maxBigInt = new BigInteger(maxValue);
        return (long)(randValue % (maxBigInt - minBigInt) + minBigInt);
    }

    /// <summary>
    /// 获取随机单精度浮点数
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机单精度浮点数</returns>
    public static float NextSingle(this Random random) =>
        (float)random.NextDouble();
#endif

    /// <summary>
    /// 获取随机单精度浮点数
    /// 随机结果大于等于0，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机单精度浮点数</returns>
    public static float NextSingle(this Random random, float maxValue)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(maxValue);
        return random.NextSingle() * maxValue;
    }

    /// <summary>
    /// 获取随机单精度浮点数
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机单精度浮点数</returns>
    public static float NextSingle(this Random random, float minValue, float maxValue)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(minValue, maxValue);
        var range = (double)maxValue - minValue;
        return (float)(range * random.NextSingle() + minValue);
    }

    /// <summary>
    /// 获取随机双精度浮点数
    /// 随机结果大于等于0，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机单精度浮点数</returns>
    public static double NextDouble(this Random random, double maxValue)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(maxValue);
        return random.NextDouble() * maxValue;
    }

    /// <summary>
    /// 获取随机双精度浮点数
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机单精度浮点数</returns>
    public static double NextDouble(this Random random, double minValue, double maxValue)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(minValue, maxValue);
        var range = (decimal)maxValue - (decimal)minValue;
        return (double)(range * (decimal)random.NextDouble() + (decimal)minValue);
    }

    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="length">字符串长度</param>
    /// <returns>随机字符串</returns>
    [Pure]
    public static string NextString(this Random random, int length)
    {
        if (length <= 0) return string.Empty;

        using var array = PooledArray.Get<char>(length, true);
        for (var i = 0; i < length; i++)
        {
            array[i] = random.NextChar();
        }

        return new string(array.ReadOnlySpan);
    }

    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="length">字符串长度</param>
    /// <param name="charPool">字符池</param>
    /// <returns>随机字符串</returns>
    public static string NextString(this Random random, int length, ReadOnlySpan<char> charPool)
    {
        if (length <= 0) return string.Empty;

        if (charPool.Length <= 0)
        {
            throw new ArgumentException("字符池不能为空", nameof(charPool));
        }

        using var array = PooledArray.Get<char>(length, true);
        for (var i = 0; i < length; i++)
        {
            array[i] = charPool[random.Next(charPool.Length)];
        }

        return new string(array.ReadOnlySpan);
    }

    /// <summary>
    /// 获取随机时间量，随机结果大于等于0
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机时间量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan NextTimeSpan(this Random random) =>
        TimeSpan.FromTicks(random.NextInt64());

    /// <summary>
    /// 获取随机时间量<br/>
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机时间量</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue) =>
        TimeSpan.FromTicks(random.NextInt64(minValue.Ticks, maxValue.Ticks));

    /// <summary>
    /// 获取随机日期时间
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机日期时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime NextDateTime(this Random random) =>
        random.NextDateTime(DateTime.MinValue, DateTime.MaxValue);

    /// <summary>
    /// 获取随机日期时间<br/>
    /// 随机结果大于等于最小值，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机日期时间</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime NextDateTime(this Random random, in DateTime minValue, in DateTime maxValue) =>
        new(random.NextInt64(minValue.Ticks, maxValue.Ticks));

    #endregion

    #region 序列抽取

    /// <summary>
    /// 随机获取序列中一个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <returns>随机元素</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NextItem<T>(this Random random, [InstantHandle] IEnumerable<T> source) =>
        random.TryNextItem(source, out var item)
            ? item
            : throw new ArgumentException("Collection must not empty", nameof(source));

    /// <summary>
    /// 尝试随机获取序列中一个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="item">随机元素</param>
    /// <returns>是否成功获取</returns>
    public static bool TryNextItem<T>(this Random random, [InstantHandle] IEnumerable<T> source,
        [MaybeNullWhen(false)] out T item)
    {
        // 可以获取到长度
        if (source.TryGetNonEnumeratedCount(out var size))
        {
            if (size <= 0)
            {
                item = default;
                return false;
            }

            item = source.ElementAt(random.Next(size));
            return true;
        }

        // 否则将序列转换为数组
        var array = source.ToArray();
        if (array is not { Length: > 0 })
        {
            item = default;
            return false;
        }

        var next = random.Next(array.Length);
        item = array[next];
        return true;
    }

    /// <summary>
    /// 放回抽取序列中多个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="count">获取数量</param>
    /// <returns>多个元素序列</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] NextItemsSelection<T>(this Random random, [InstantHandle] IEnumerable<T> source,
        int count) => random.TryNextItemsSelection(source, count, out var items)
        ? items
        : throw new ArgumentException("Collection must not empty", nameof(source));

    /// <summary>
    /// 尝试放回抽取序列中多个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="count">获取数量</param>
    /// <param name="items">随机元素序列</param>
    /// <returns>是否成功获取</returns>
    [Pure]
    public static bool TryNextItemsSelection<T>(this Random random, [InstantHandle] IEnumerable<T> source,
        int count, [MaybeNullWhen(false)] out T[] items)
    {
        if (count <= 0)
        {
            items = [];
            return true;
        }

        if (source.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            items = null;
            return false;
        }

        items = ArrayHelper.AllocateUninitializedArray<T>(count);
        switch (source)
        {
            case IList<T> list:
            {
                if (list.Count <= 0) return false;

                for (var i = 0; i < count; i++)
                {
                    var next = random.Next(list.Count);
                    items[i] = list[next];
                }

                return true;
            }
            case IReadOnlyList<T> list:
            {
                if (list.Count <= 0) return false;

                for (var i = 0; i < count; i++)
                {
                    var next = random.Next(list.Count);
                    items[i] = list[next];
                }

                return true;
            }
            default:
            {
                var array = source.ToArray();
                if (array.Length <= 0) return false;

                for (var i = 0; i < count; i++)
                {
                    var next = random.Next(array.Length);
                    items[i] = array[next];
                }

                return true;
            }
        }
    }

    /// <summary>
    /// 不放回多次抽取元素<br/>
    /// 当抽取数量大于或等于序列长度时，返回序列的拷贝
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="count">抽取数量</param>
    /// <returns>抽取结果列表</returns>
    [Pure]
    public static T[] NextItemsSample<T>(this Random random, [InstantHandle] IEnumerable<T> source, int count)
    {
        // 无法获取长度
        if (!source.TryGetNonEnumeratedCount(out var size))
        {
            // 由于无法获取长度，则在内部调用一定会创建数组返回
            return (T[])random.NextItemsSampleRaw(source, count);
        }

        // 长度大于或等于抽取数量，返回序列拷贝
        if (size <= count) return source.ToArray();

        var array = source.ToArray();
        // 抽取长度小于列表长度，因此内部调用一定会创建数组保存结果返回
        return (T[])random.NextItemsSampleRaw(array, count);
    }

    /// <summary>
    /// 不放回多次抽取元素原始实现
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">元素序列</param>
    /// <param name="count">抽取数量</param>
    /// <returns>抽取结果列表</returns>
    /// <remarks>
    /// 此方法会尝试将序列转为列表，若无法转换则创建并拷贝至数组<br/>
    /// 若列表长度小于等于抽取数量，会直接返回列表<br/>
    /// 若输入序列为列表，抽取过程会修改输入列表内容，若需保留原列表请使用<see cref="NextItemsSample{T}"/>
    /// </remarks>
    [MustUseReturnValue]
    public static IList<T> NextItemsSampleRaw<T>(this Random random, [InstantHandle] IEnumerable<T> source, int count)
    {
        // 抽取数量小于等于0，返回空数组
        if (count <= 0) return Array.Empty<T>();

        // 尝试将序列转为列表，否则拷贝至数组
        var list = (source as IList<T>) ?? source.ToArray();
        if (list.Count <= count) return list;

        var result = ArrayHelper.AllocateUninitializedArray<T>(count);
        var index = 0;
        while (index < count)
        {
            var lastIndex = list.Count - 1 - index;
            var next = random.Next(lastIndex + 1);
            // 利用Shuffle类似逻辑实现不放回抽取
            (result[index], list[next]) = (list[next], list[lastIndex]);
            index++;
        }

        return result;
    }

    #endregion

    #region 带权随机索引

    /// <summary>
    /// 计算带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weights">权重序列</param>
    /// <returns>权重索引</returns>
    [Pure]
    public static int NextWeightedIndex(this Random random, [InstantHandle] IEnumerable<int> weights)
    {
        var accumulation = WeightsAccumulation(weights);
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weights));
        }

        return NextWeightedIndexForAccumulation(random, accumulation);
    }

    /// <summary>
    /// 计算带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightItems">权重元素序列</param>
    /// <returns>权重索引</returns>
    [Pure]
    public static int NextWeightedIndex<T>(this Random random, [InstantHandle] IEnumerable<T> weightItems)
        where T : IWeighted
    {
        var accumulation = WeightsAccumulation(weightItems);
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weightItems));
        }

        return NextWeightedIndexForAccumulation(random, accumulation);
    }

    /// <summary>
    /// 计算多个不放回带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weights">权重序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>权重索引数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] NextWeightedIndexes(this Random random, [InstantHandle] IEnumerable<int> weights, int count)
    {
        if (count <= 0) return [];

        var accumulation = WeightsAccumulation(weights);
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weights));
        }

        var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
        return indexes.CalculateArray();
    }

    /// <summary>
    /// 计算多个不放回带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightItems">权重元素序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>权重索引数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int[] NextWeightedIndexes<T>(this Random random, IEnumerable<T> weightItems, int count)
        where T : IWeighted
    {
        if (count <= 0) return [];

        var accumulation = WeightsAccumulation(weightItems);
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weightItems));
        }

        var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
        return indexes.CalculateArray();
    }

    /// <summary>通过权重累和数组计算带权随机索引</summary>
    private static int NextWeightedIndexForAccumulation(this Random random, IReadOnlyList<int> accumulation)
    {
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(accumulation));
        }

        return NextWeightedIndexInternal(random, accumulation);
    }

    /// <summary>通过权重累和数组计算带权随机索引，内部实现</summary>
    private static int NextWeightedIndexInternal(Random random, IReadOnlyList<int> accumulation)
    {
        var next = random.Next(1, accumulation[^1] + 1);
        // 利用二分查找与权重累和数组查找索引
        var index = accumulation.BinarySearch(next);
        return index < 0 ? ~index : index;
    }

    #endregion

    #region 放回带权随机抽取

    /// <summary>
    /// 带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射</param>
    /// <returns>随机元素结果</returns>
    [Pure]
    public static T NextWeighted<T>(this Random random, [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights)
    {
        var items = new List<T>(itemWeights.GetCountOrZero());
        var accumulation = new List<int>(itemWeights.GetCountOrZero());
        var weightSum = 0;
        foreach (var (item, weight) in itemWeights)
        {
            items.Add(item);
            if (weight <= 0)
            {
                throw new ArgumentException("Weight must greater than 0", nameof(itemWeights));
            }

            weightSum += weight;
            accumulation.Add(weightSum);
        }

        var index = random.NextWeightedIndexForAccumulation(accumulation);
        return items[index];
    }

    /// <summary>
    /// 放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>随机结果数组</returns>
    [Pure]
    public static T[] NextWeightedSelection<T>(this Random random,
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights, int count)
    {
        if (count <= 0) return [];

        var result = ArrayHelper.AllocateUninitializedArray<T>(count);
        var items = new List<T>(itemWeights.GetCountOrZero());
        var accumulation = new List<int>(itemWeights.GetCountOrZero());
        var weightSum = 0;
        foreach (var (item, weight) in itemWeights)
        {
            items.Add(item);
            if (weight <= 0)
            {
                throw new ArgumentException("Weight must greater than 0", nameof(itemWeights));
            }

            weightSum += weight;
            accumulation.Add(weightSum);
        }

        var i = 0;
        var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
        while (indexes.MoveNext())
        {
            result[i++] = items[indexes.Current];
        }

        return result;
    }

    /// <summary>
    /// 带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <returns>抽取元素</returns>
    [Pure]
    public static T NextWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems)
        where T : IWeighted
    {
        if (weightedItems.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            throw new ArgumentException("Collection must not empty", nameof(weightedItems));
        }

        switch (weightedItems)
        {
            case IList<T> weightedList:
            {
                var index = random.NextWeightedIndex(weightedList);
                return weightedList[index];
            }
            case IReadOnlyList<T> weightedList:
            {
                var index = random.NextWeightedIndex(weightedList);
                return weightedList[index];
            }
            default:
            {
                var weightItemArray = weightedItems.ToArray();
                var index = random.NextWeightedIndex(weightItemArray);
                return weightItemArray[index];
            }
        }
    }

    /// <summary>
    /// 放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <param name="count">抽取次数</param>
    /// <returns>抽取结果数组</returns>
    [Pure]
    public static T[] NextWeightedSelection<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems,
        int count) where T : IWeighted
    {
        if (count <= 0) return [];

        if (weightedItems.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            throw new ArgumentException("Collection must not empty", nameof(weightedItems));
        }

        var result = ArrayHelper.AllocateUninitializedArray<T>(count);
        switch (weightedItems)
        {
            case IList<T> weightedList:
            {
                var accumulation = WeightsAccumulation(weightedList);
                var i = 0;
                using var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
                while (indexes.MoveNext())
                {
                    result[i++] = weightedList[indexes.Current];
                }

                return result;
            }
            case IReadOnlyList<T> weightedList:
            {
                var accumulation = WeightsAccumulation(weightedList);
                var i = 0;
                using var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
                while (indexes.MoveNext())
                {
                    result[i++] = weightedList[indexes.Current];
                }

                return result;
            }
            default:
            {
                var weightedArray = weightedItems.ToArray();
                var accumulation = WeightsAccumulation(weightedArray);
                var i = 0;
                using var indexes = new AccumulationWeightedIndexEnumerator(random, accumulation, count);
                while (indexes.MoveNext())
                {
                    result[i++] = weightedArray[indexes.Current];
                }

                return result;
            }
        }
    }

    #endregion

    /// <summary>
    /// 随机打乱列表顺序
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="list">列表</param>
    public static void Shuffle<T>(this Random random, IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var index = random.Next(i + 1);
            list.Swap(i, index);
        }
    }

    /// <summary>
    /// 随机打乱字典键值对映射
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="dict">字典</param>
    public static void Shuffle<TKey, TValue>(this Random random, IDictionary<TKey, TValue> dict)
    {
        var keys = dict.Keys.ToArray();
        for (var i = keys.Length - 1; i > 0; i--)
        {
            var index = random.Next(i + 1);
            var key1 = keys[i];
            var key2 = keys[index];
            keys.Swap(i, index);
            dict.Swap(key1, key2);
        }
    }

    /// <summary>权重累和计算，返回权重累和池化数组</summary>
    private static List<int> WeightsAccumulation([InstantHandle] IEnumerable<int> weights)
    {
        var accumulation = new List<int>(weights.GetCountOrZero());
        var sum = 0;
        foreach (var weight in weights)
        {
            if (weight <= 0)
            {
                throw new ArgumentException("Weight must greater than 0", nameof(weights));
            }

            sum += weight;
            accumulation.Add(sum);
        }

        return accumulation;
    }

    /// <summary>权重累和计算，返回权重累和池化数组</summary>
    private static List<int> WeightsAccumulation<T>([InstantHandle] IEnumerable<T> weightedItems) where T : IWeighted
    {
        var accumulation = new List<int>(weightedItems.GetCountOrZero());
        var sum = 0;
        foreach (var item in weightedItems)
        {
            var weight = item.Weight;
            if (weight <= 0)
            {
                throw new ArgumentException("Weight must greater than 0", nameof(weightedItems));
            }

            sum += weight;
            accumulation.Add(sum);
        }

        return accumulation;
    }

    /// <summary>累和权重索引迭代器</summary>
    private struct AccumulationWeightedIndexEnumerator : IEnumerator<int>
    {
        private readonly Random _random;
        private readonly IReadOnlyList<int> _accumulation;
        private readonly int _count;
        private int _generated;

        /// <inheritdoc />
        public int Current { get; private set; }

        /// <inheritdoc />
        object IEnumerator.Current => Current;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="random">随机实例</param>
        /// <param name="accumulation">累和数组</param>
        /// <param name="count">随机次数</param>
        public AccumulationWeightedIndexEnumerator(Random random, IReadOnlyList<int> accumulation, int count)
        {
            _random = random;
            _accumulation = accumulation;
            _count = count;
            if (count > 0 && accumulation.Count <= 0)
            {
                throw new ArgumentException("Collection must not empty", nameof(accumulation));
            }
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (_generated >= _count) return false;

            Current = NextWeightedIndexInternal(_random, _accumulation);
            _generated++;
            return true;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset() => _generated = 0;

        /// <summary>
        /// 按权重累和计算，返回权重索引数组
        /// </summary>
        /// <returns>权重索引数组</returns>
        public int[] CalculateArray()
        {
            var result = ArrayHelper.AllocateUninitializedArray<int>(_count);
            for (var i = 0; i < _count; i++)
            {
                result[i] = NextWeightedIndexInternal(_random, _accumulation);
            }

            return result;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}