using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
#if !NET6_0_OR_GREATER
using System.Numerics;
using System.Security.Cryptography;
#endif
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Helpers;

namespace UltraTool.Randoms;

/// <summary>
/// 随机拓展类
/// </summary>
public static class RandomExtensions
{
    #region 拓展类型支持

    /// <summary>
    /// 随机获取布尔值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机布尔值</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBool(this Random random) => random.Next(2) == 0;

    /// <summary>
    /// 随机获取字节，随机结果大于等于0
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机字节</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NextByte(this Random random) => (byte)random.Next(0, byte.MaxValue + 1);

    /// <summary>
    /// 随机获取字节<br/>
    /// 随机结果大于等于0，小于最大值
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>随机字节</returns>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte NextByte(this Random random, byte minValue, byte maxValue) =>
        (byte)random.Next(minValue, maxValue);

    /// <summary>
    /// 获取随机字符
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机字符</returns>
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
        var range = maxValue - minValue;
        return range * random.NextDouble() + minValue;
    }

    /// <summary>
    /// 获取随机字符串
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="length">字符串长度</param>
    /// <returns>随机字符串</returns>
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
            throw new ArgumentException("Char pool must not empty", nameof(charPool));
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue) =>
        TimeSpan.FromTicks(random.NextInt64(minValue.Ticks, maxValue.Ticks));

    /// <summary>
    /// 获取随机日期时间
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机日期时间</returns>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime NextDateTime(this Random random, in DateTime minValue, in DateTime maxValue) =>
        new(random.NextInt64(minValue.Ticks, maxValue.Ticks));

    #endregion

    #region 序列抽取

    /// <summary>洗牌抽取阈值</summary>
    private const int SampleShuffleThreshold = 1000;

    /// <summary>
    /// 随机获取序列中一个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <returns>随机元素</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Choice<T>(this Random random, [InstantHandle] IEnumerable<T> source) =>
        random.TryChoice(source, out var item) ? item : throw new ArgumentException("Collection must not empty", nameof(source));

    /// <summary>
    /// 尝试随机获取序列中一个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="item">随机元素</param>
    /// <returns>是否成功获取</returns>
    public static bool TryChoice<T>(this Random random, [InstantHandle] IEnumerable<T> source,
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

        // 否则使用蓄水池抽样算法
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            item = default;
            return false;
        }

        // 第一个元素作为初始候选
        item = enumerator.Current;
        var count = 1;
        // 随机替换候选元素
        while (enumerator.MoveNext())
        {
            count++;
            if (random.Next(count) == 0)
            {
                item = enumerator.Current;
            }
        }

        return true;
    }

    /// <summary>
    /// 放回抽取序列中多个元素
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="count">获取数量</param>
    /// <returns>多个元素序列</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Choice<T>(this Random random, [InstantHandle] IEnumerable<T> source,
        int count) => random.TryChoice(source, count, out var items)
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
    public static bool TryChoice<T>(this Random random, [InstantHandle] IEnumerable<T> source,
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

        items = new T[count];
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
    public static IList<T> Sample<T>(this Random random, [InstantHandle] IEnumerable<T> source, int count)
    {
        if (count <= 0) return Array.Empty<T>();

        if (source.TryGetNonEnumeratedCount(out var size))
        {
            // 抽取数量小于等于序列长度
            if (size <= count) return source.ToArray();

            // 序列长度小于阈值，则使用洗牌算法
            if (size <= SampleShuffleThreshold)
            {
                return random.SampleShuffle(source.ToArray(), count);
            }
        }

        var result = new List<T>(count);
        var i = 0;
        foreach (var item in source)
        {
            if (i < count)
            {
                result.Add(item);
            }
            else
            {
                var next = random.Next(i + 1);
                if (next < count)
                {
                    result[next] = item;
                }
            }

            i++;
        }

        return result;
    }

    /// <summary>
    /// 不放回多次抽取元素<br/>
    /// 当抽取数量大于或等于序列长度时，返回序列的拷贝
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">序列</param>
    /// <param name="count">抽取数量</param>
    /// <returns>抽取结果列表</returns>
    public static async Task<List<T>> SampleAsync<T>(this Random random, IAsyncEnumerable<T> source, int count)
    {
        var result = new List<T>(count);
        var i = 0;
        await foreach (var item in source.ConfigureAwait(false))
        {
            if (i < count)
            {
                result.Add(item);
            }
            else
            {
                var next = random.Next(i + 1);
                if (next < count)
                {
                    result[next] = item;
                }
            }

            i++;
        }

        return result;
    }

    /// <summary>
    /// 不放回多次抽取元素，基于列表洗牌思路实现
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="source">元素序列</param>
    /// <param name="count">抽取数量</param>
    /// <returns>抽取结果列表</returns>
    /// <remarks>
    /// 此方法会尝试将序列转为列表，若无法转换则创建并拷贝至数组<br/>
    /// 若列表长度小于等于抽取数量，会直接返回列表<br/>
    /// 若输入序列为列表，抽取过程会修改输入列表内容，若需保留原列表请使用<see cref="Sample{T}"/>
    /// </remarks>
    [MustUseReturnValue]
    public static IList<T> SampleShuffle<T>(this Random random, [InstantHandle] IEnumerable<T> source, int count)
    {
        // 抽取数量小于等于0，返回空数组
        if (count <= 0) return Array.Empty<T>();

        // 尝试将序列转为列表，否则拷贝至数组
        var list = (source as IList<T>) ?? source.ToArray();
        if (list.Count <= count) return list;

        var result = new T[count];
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
    /// <remarks>权重不能小于或等于0</remarks>
    public static int NextWeightedIndex(this Random random, [InstantHandle] IEnumerable<int> weights)
    {
        using var accumulation = WeightsAccumulation(weights);
        if (accumulation is not { Length: > 0 })
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
    /// <remarks>权重不能小于或等于0</remarks>
    public static int NextWeightedIndex<T>(this Random random, [InstantHandle] IEnumerable<T> weightItems)
        where T : IWeighted
    {
        using var accumulation = WeightsAccumulation(weightItems);
        if (accumulation is not { Length: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weightItems));
        }

        return NextWeightedIndexForAccumulation(random, accumulation);
    }

    /// <summary>
    /// 计算多个放回带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weights">权重序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>权重索引数组</returns>
    /// <remarks>权重不能小于或等于0</remarks>
    public static int[] NextWeightedIndexes(this Random random, [InstantHandle] IEnumerable<int> weights, int count)
    {
        if (count <= 0) return [];

        using var accumulation = WeightsAccumulation(weights);
        if (accumulation is not { Length: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weights));
        }

        var indexes = new WeightedIndexEnumerator<PooledDynamicArray<int>>(random, accumulation, count);
        return indexes.CalculateArray();
    }

    /// <summary>
    /// 计算多个放回带权随机索引
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightItems">权重元素序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>权重索引数组</returns>
    /// <remarks>权重不能小于或等于0</remarks>
    public static int[] NextWeightedIndexes<T>(this Random random, IEnumerable<T> weightItems, int count)
        where T : IWeighted
    {
        if (count <= 0) return [];

        using var accumulation = WeightsAccumulation(weightItems);
        if (accumulation is not { Length: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(weightItems));
        }

        var indexes = new WeightedIndexEnumerator<PooledDynamicArray<int>>(random, accumulation, count);
        return indexes.CalculateArray();
    }

    /// <summary>通过权重累和数组计算带权随机索引</summary>
    private static int NextWeightedIndexForAccumulation<T>(Random random, T accumulation)
        where T : IReadOnlyList<int>
    {
        if (accumulation is not { Count: > 0 })
        {
            throw new ArgumentException("Collection must not empty", nameof(accumulation));
        }

        return NextWeightedIndexInternal(random, accumulation);
    }

    /// <summary>通过权重累和数组计算带权随机索引，内部实现</summary>
    private static int NextWeightedIndexInternal<T>(Random random, T accumulation) where T : IReadOnlyList<int>
    {
        var next = random.Next(1, accumulation[^1] + 1);
        // 利用二分查找与权重累和数组查找索引
        var index = accumulation.BinarySearch(next);
        return index < 0 ? ~index : index;
    }

    #endregion

    #region 放回带权随机抽取

    /// <summary>
    /// 尝试带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射</param>
    /// <param name="result">随机元素结果</param>
    /// <returns>是否成功</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static bool TryChoiceWeighted<T>(this Random random,
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights, [MaybeNullWhen(false)] out T result)
    {
        if (itemWeights.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            result = default;
            return false;
        }

        var items = new List<T>(itemWeights.GetCountOrZero());
        using var accumulation = new PooledDynamicArray<int>(itemWeights.GetCountOrZero());
        var weightSum = 0;
        foreach (var (item, weight) in itemWeights)
        {
            if (weight <= 0) continue;

            items.Add(item);
            weightSum += weight;
            accumulation.Add(weightSum);
        }

        if (items.Count <= 0)
        {
            result = default;
            return false;
        }

        var index = NextWeightedIndexForAccumulation(random, accumulation);
        result = items[index];
        return true;
    }

    /// <summary>
    /// 带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射</param>
    /// <returns>随机元素结果</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ChoiceWeighted<T>(this Random random, [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights)
        => random.TryChoiceWeighted(itemWeights, out var result)
            ? result
            : throw new ArgumentException("Collection is empty or have invalid weight", nameof(itemWeights));

    /// <summary>
    /// 尝试放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射序列</param>
    /// <param name="count">随机次数</param>
    /// <param name="result">随机结果数组</param>
    /// <returns>是否成功</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static bool TryChoiceWeighted<T>(this Random random,
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights, int count, [MaybeNullWhen(false)] out T[] result)
    {
        if (count <= 0)
        {
            result = [];
            return true;
        }

        if (itemWeights.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            result = null;
            return false;
        }

        var items = new List<T>(itemWeights.GetCountOrZero());
        using var accumulation = new PooledDynamicArray<int>(itemWeights.GetCountOrZero());
        var weightSum = 0;
        foreach (var (item, weight) in itemWeights)
        {
            if (weight <= 0) continue;

            items.Add(item);
            weightSum += weight;
            accumulation.Add(weightSum);
        }

        if (items.Count <= 0)
        {
            result = null;
            return false;
        }

        result = new T[count];
        var i = 0;
        var indexes = new WeightedIndexEnumerator<PooledDynamicArray<int>>(random, accumulation, count);
        while (indexes.MoveNext())
        {
            result[i++] = items[indexes.Current];
        }

        return true;
    }

    /// <summary>
    /// 放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射序列</param>
    /// <param name="count">随机次数</param>
    /// <returns>随机结果数组</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] ChoiceWeighted<T>(this Random random,
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights, int count) =>
        random.TryChoiceWeighted(itemWeights, count, out var result)
            ? result
            : throw new ArgumentException("Collection is empty or have invalid weight", nameof(itemWeights));


    /// <summary>
    /// 尝试带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <param name="result">随机元素结果</param>
    /// <returns>是否成功</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static bool TryChoiceWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems,
        [MaybeNullWhen(false)] out T result) where T : IWeighted
    {
        if (weightedItems.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            result = default;
            return false;
        }

        var items = new List<T>(weightedItems.GetCountOrZero());
        using var accumulation = new PooledDynamicArray<int>(weightedItems.GetCountOrZero());
        var weightSum = 0;
        foreach (var item in weightedItems)
        {
            if (item.Weight <= 0) continue;

            items.Add(item);
            weightSum += item.Weight;
            accumulation.Add(weightSum);
        }

        if (items.Count <= 0)
        {
            result = default;
            return false;
        }

        var index = NextWeightedIndexForAccumulation(random, accumulation);
        result = items[index];
        return true;
    }

    /// <summary>
    /// 带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <returns>抽取元素</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static T ChoiceWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems)
        where T : IWeighted => random.TryChoiceWeighted(weightedItems, out var result)
        ? result
        : throw new ArgumentException("Collection is empty or have invalid weight", nameof(weightedItems));

    /// <summary>
    /// 尝试放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <param name="count">抽取次数</param>
    /// <param name="result">随机结果数组</param>
    /// <returns>是否成功</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static bool TryChoiceWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems,
        int count, [MaybeNullWhen(false)] out T[] result) where T : IWeighted
    {
        if (count <= 0)
        {
            result = [];
            return true;
        }

        if (weightedItems.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            result = null;
            return false;
        }

        var items = new List<T>(weightedItems.GetCountOrZero());
        using var accumulation = new PooledDynamicArray<int>(weightedItems.GetCountOrZero());
        var weightSum = 0;
        foreach (var item in weightedItems)
        {
            if (item.Weight <= 0) continue;

            items.Add(item);
            weightSum += item.Weight;
            accumulation.Add(weightSum);
        }

        if (items.Count <= 0)
        {
            result = null;
            return false;
        }

        result = new T[count];
        var i = 0;
        var indexes = new WeightedIndexEnumerator<PooledDynamicArray<int>>(random, accumulation, count);
        while (indexes.MoveNext())
        {
            result[i++] = items[indexes.Current];
        }

        return true;
    }

    /// <summary>
    /// 放回多次带权随机
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <param name="count">抽取次数</param>
    /// <returns>抽取结果数组</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] ChoiceWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems,
        int count) where T : IWeighted => random.TryChoiceWeighted(weightedItems, count, out var result)
        ? result
        : throw new ArgumentException("Collection is empty or have invalid weight", nameof(weightedItems));

    #endregion

    #region 不放回带权随机抽取

    /// <summary>
    /// 不放回多次带权随机<br/>
    /// 当抽取数量大于或等于序列长度时，返回元素序列
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="itemWeights">{元素:权重}映射</param>
    /// <param name="count">抽取次数</param>
    /// <returns>抽取结果列表</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static T[] SampleWeighted<T>(this Random random,
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights, int count)
    {
        if (count <= 0) return Array.Empty<T>();

        if (itemWeights.TryGetNonEnumeratedCount(out var size) && size <= count)
        {
            return itemWeights.Where(pair => pair.Value > 0).Select(pair => pair.Key).ToArray();
        }

        var (totalWeight, list) = ToWeightedList(itemWeights);
        return count >= list.Count
            ? list.Select(pair => pair.Key).ToArrayWithCapacity(list.Count)
            : SampleWeightedInternal(random, totalWeight, list, count);
    }

    /// <summary>
    /// 不放回多次带权随机<br/>
    /// 当抽取数量大于或等于序列长度时，返回序列拷贝
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="weightedItems">带权元素序列</param>
    /// <param name="count">抽取次数</param>
    /// <returns>抽取结果列表</returns>
    /// <remarks>权重小于等于0的元素会被忽略</remarks>
    public static IList<T> SampleWeighted<T>(this Random random, [InstantHandle] IEnumerable<T> weightedItems,
        int count)
        where T : IWeighted
    {
        if (count <= 0) return Array.Empty<T>();

        if (weightedItems.TryGetNonEnumeratedCount(out var size) && size <= count)
        {
            return weightedItems.Where(pair => pair.Weight > 0).ToArray();
        }

        var (totalWeight, list) = ToWeightedList(weightedItems);
        return count >= list.Count
            ? list
            : SampleWeightedInternal(random, totalWeight, list, count);
    }

    /// <summary>不放回多次带权随机，内部实现</summary>
    private static T[] SampleWeightedInternal<T>(Random random, int totalWeight, List<KeyValuePair<T, int>> list,
        int count)
    {
        var result = new T[count];
        var sampleCount = 0;
        var validLength = list.Count;
        while (sampleCount < count && validLength > 0)
        {
            var next = random.Next(totalWeight);
            var curWeight = 0;
            for (var i = 0; i < validLength; i++)
            {
                var pair = list[i];
                curWeight += pair.Value;
                if (next < curWeight)
                {
                    // 抽取为当前权重
                    result[sampleCount] = pair.Key;
                    // 总权重值减去当前权重值
                    totalWeight -= pair.Value;
                    // 移至尾部
                    list.Swap(i, validLength - 1);
                    validLength--;
                    break;
                }
            }

            sampleCount++;
        }

        return result;
    }

    /// <summary>不放回多次带权随机，内部实现</summary>
    private static T[] SampleWeightedInternal<T>(Random random, int totalWeight, List<T> list, int count)
        where T : IWeighted
    {
        var result = new T[count];
        var sampleCount = 0;
        var validLength = list.Count;
        while (sampleCount < count && validLength > 0)
        {
            var next = random.Next(totalWeight);
            var curWeight = 0;
            for (var i = 0; i < validLength; i++)
            {
                var item = list[i];
                curWeight += item.Weight;
                if (next < curWeight)
                {
                    // 抽取为当前权重
                    result[sampleCount] = item;
                    // 总权重值减去当前权重值
                    totalWeight -= item.Weight;
                    // 移至尾部
                    list.Swap(i, validLength - 1);
                    validLength--;
                    break;
                }
            }

            sampleCount++;
        }

        return result;
    }

    /// <summary>计算元素权重序列的总权重值，生成列表</summary>
    private static (int, List<KeyValuePair<T, int>>) ToWeightedList<T>(
        [InstantHandle] IEnumerable<KeyValuePair<T, int>> itemWeights)
    {
        var totalWeight = 0;
        var list = new List<KeyValuePair<T, int>>(itemWeights.GetCountOrZero());
        foreach (var pair in itemWeights)
        {
            if (pair.Value <= 0) continue;

            totalWeight += pair.Value;
            list.Add(pair);
        }

        return (totalWeight, list);
    }

    /// <summary>计算权重元素序列的总权重值，生成列表</summary>
    private static (int, List<T>) ToWeightedList<T>([InstantHandle] IEnumerable<T> weightedItems)
        where T : IWeighted
    {
        var totalWeight = 0;
        var list = new List<T>(weightedItems.GetCountOrZero());
        foreach (var weightedItem in weightedItems)
        {
            if (weightedItem.Weight <= 0) continue;

            totalWeight += weightedItem.Weight;
            list.Add(weightedItem);
        }

        return (totalWeight, list);
    }

    #endregion

#if !NET8_0_OR_GREATER
    /// <summary>
    /// 随机打乱跨度顺序
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <param name="span">跨度</param>
    public static void Shuffle<T>(this Random random, Span<T> span)
    {
        for (var i = span.Length - 1; i > 0; i--)
        {
            var index = random.Next(i + 1);
            if (index == i) continue;

            (span[i], span[index]) = (span[index], span[i]);
        }
    }
#endif

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
            if (index == i) continue;

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
            if (index == i) continue;

            var key1 = keys[i];
            var key2 = keys[index];
            keys.Swap(i, index);
            dict.Swap(key1, key2);
        }
    }

    /// <summary>权重累和计算，返回权重累和池化数组</summary>
    [MustDisposeResource]
    private static PooledDynamicArray<int> WeightsAccumulation([InstantHandle] IEnumerable<int> weights)
    {
        var accumulation = new PooledDynamicArray<int>(weights.GetCountOrZero());
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
    [MustDisposeResource]
    private static PooledDynamicArray<int> WeightsAccumulation<T>([InstantHandle] IEnumerable<T> weightedItems)
        where T : IWeighted
    {
        var accumulation = new PooledDynamicArray<int>(weightedItems.GetCountOrZero());
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

    /// <summary>权重索引迭代器</summary>
    private struct WeightedIndexEnumerator<T> : IEnumerator<int> where T : IReadOnlyList<int>
    {
        private readonly Random _random;
        private readonly T _accumulation;
        private readonly int _count;
        private int _generated;

        /// <inheritdoc />
        public int Current { get; private set; }

        /// <inheritdoc />
        readonly object IEnumerator.Current => Current;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="random">随机实例</param>
        /// <param name="accumulation">累和列表</param>
        /// <param name="count">随机次数</param>
        public WeightedIndexEnumerator(Random random, T accumulation, int count)
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
        public readonly int[] CalculateArray()
        {
            var result = new int[_count];
            for (var i = 0; i < _count; i++)
            {
                result[i] = NextWeightedIndexInternal(_random, _accumulation);
            }

            return result;
        }

        /// <inheritdoc />
        public readonly void Dispose()
        {
        }
    }
}