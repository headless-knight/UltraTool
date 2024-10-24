using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UltraTool.Collections;

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
    /// </summary>
    /// <param name="random">随机实例</param>
    /// <returns>随机64位整型</returns>
    public static long NextInt64(this Random random)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        RandomNumberGenerator.Fill(bytes);
        return Math.Abs(BitConverter.ToInt64(bytes));
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
#endif

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
        random.TryNextItem(source, out var item) ? item : throw new ArgumentException("序列长度必须大于0", nameof(source));

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
        : throw new ArgumentException("序列长度必须大于0", nameof(source));

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
}