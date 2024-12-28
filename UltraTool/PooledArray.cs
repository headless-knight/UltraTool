using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Helpers;

namespace UltraTool;

/// <summary>
/// 池化数组静态类
/// </summary>
[PublicAPI]
public static class PooledArray
{
    /// <summary>
    /// 获取池化数组
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> Get<T>(int length, bool? clearArray = null) => new(length, clearArray);

    /// <summary>
    /// 获取池化数组
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> Get<T>(int length, ArrayPool<T> pool, bool? clearArray = null) =>
        new(length, pool, clearArray);

    /// <summary>
    /// 获取池化数组，并清空数组内容
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> GetCleared<T>(int length, bool? clearArray = null)
    {
        var array = Get<T>(length, clearArray);
        array.Clear();
        return array;
    }

    /// <summary>
    /// 从指定跨度拷贝数据生成池化数组
    /// </summary>
    /// <param name="span">只读跨度</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> From<T>(ReadOnlySpan<T> span, bool? clearArray = null) =>
        From(span.Length, span, ArrayPool<T>.Shared, clearArray);

    /// <summary>
    /// 从指定跨度拷贝数据生成池化数组
    /// </summary>
    /// <param name="span">只读跨度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> From<T>(ReadOnlySpan<T> span, ArrayPool<T> pool, bool? clearArray = null) =>
        From(span.Length, span, pool, clearArray);

    /// <summary>
    /// 从指定跨度拷贝数据生成池化数组
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="span">只读跨度</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> From<T>(int length, ReadOnlySpan<T> span, bool? clearArray = null) =>
        From(length, span, ArrayPool<T>.Shared, clearArray);

    /// <summary>
    /// 从指定跨度拷贝数据生成池化数组
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="span">只读跨度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    public static PooledArray<T> From<T>(int length, ReadOnlySpan<T> span, ArrayPool<T> pool, bool? clearArray = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfLessThan(length, span.Length);
        var destination = pool.Rent(length);
        span.CopyTo(new Span<T>(destination, 0, span.Length));
        return new PooledArray<T>(destination, length, pool, clearArray);
    }

    /// <summary>
    /// 序列转为池化数组
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> ToPooledArray<T>([InstantHandle] this IEnumerable<T> source, bool? clearArray = null)
        => source.ToPooledArray(ArrayPool<T>.Shared, clearArray);

    /// <summary>
    /// 序列转为池化数组
    /// </summary>
    /// <param name="source">源序列</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    public static PooledArray<T> ToPooledArray<T>([InstantHandle] this IEnumerable<T> source, ArrayPool<T> pool,
        bool? clearArray = null)
    {
        var dynamicArray = new PooledDynamicArray<T>(source.GetCountOrZero(), pool, clearArray);
        try
        {
            dynamicArray.AddRange(source);
            return dynamicArray.BuildPooledArray();
        }
        catch
        {
            dynamicArray.Dispose();
            throw;
        }
    }
}

/// <summary>
/// 池化数组
/// </summary>
[PublicAPI, MustDisposeResource]
public struct PooledArray<T> : IList<T>, IReadOnlyList<T>, IDisposable
{
    /// <summary>默认数组池</summary>
    private static ArrayPool<T> DefaultPool => ArrayPool<T>.Shared;

    private readonly ArrayPool<T> _pool;
    private readonly bool _clearArray;
    private T[]? _array;

    /// <summary>
    /// 空池化数组
    /// </summary>
    public static PooledArray<T> Empty
    {
        [MustDisposeResource] get => new(0);
    }

    /// <summary>
    /// 有效长度
    /// </summary>
    public int Length { get; }

    /// <inheritdoc />
    readonly bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc />
    readonly int ICollection<T>.Count => Length;

    /// <inheritdoc />
    readonly int IReadOnlyCollection<T>.Count => Length;

    /// <summary>
    /// 原始数组
    /// </summary>
    public readonly T[] RawArray => _array.EmptyIfNull();

    /// <summary>
    /// 可枚举对象
    /// </summary>
    public readonly IEnumerable<T> Enumerable => RawArray.Take(Length);

    /// <summary>
    /// 跨度
    /// </summary>
    public readonly Span<T> Span => new(_array, 0, Length);

    /// <summary>
    /// 只读跨度
    /// </summary>
    public readonly ReadOnlySpan<T> ReadOnlySpan => new(_array, 0, Length);

    /// <summary>
    /// 内存
    /// </summary>
    public readonly Memory<T> Memory => new(_array, 0, Length);

    /// <summary>
    /// 只读内存
    /// </summary>
    public readonly ReadOnlyMemory<T> ReadOnlyMemory => new(_array, 0, Length);

    /// <summary>
    /// 数组段
    /// </summary>
    public readonly ArraySegment<T> Segment => new(_array.EmptyIfNull(), 0, Length);

    /// <summary>
    /// 只读序列
    /// </summary>
    public readonly ReadOnlySequence<T> ReadOnlySequence => new(_array.EmptyIfNull(), 0, Length);

    /// <inheritdoc cref="IList{T}.this"/>
    public readonly T this[int index]
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        get
        {
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
            return _array![index];
        }
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        set
        {
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
            _array![index] = value;
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="length">有效长度</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledArray(int length, bool? clearArray = null) : this(length, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="length">有效长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否释放时清空数组</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledArray(int length, ArrayPool<T> pool, bool? clearArray)
    {
        _array = pool.Rent(length);
        Length = length;
        _pool = pool;
        _clearArray = clearArray ?? RuntimeHelpers.IsReferenceOrContainsReferences<T>();
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="array">原始数组</param>
    /// <param name="length">有效长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否释放时清空数组</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledArray(T[] array, int length, ArrayPool<T> pool, bool? clearArray)
    {
        _array = array;
        Length = length;
        _pool = pool;
        _clearArray = clearArray ?? RuntimeHelpers.IsReferenceOrContainsReferences<T>();
    }

    /// <summary>
    /// 遍历序列
    /// </summary>
    /// <param name="action">遍历操作，入参(元素)</param>
    public void ForEach(Action<T> action)
    {
        if (_array is not { Length: > 0 }) return;

        for (var i = 0; i < Length; i++)
        {
            action.Invoke(_array[i]);
        }
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Contains(T item) => IndexOf(item) >= 0;

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int IndexOf(T item) => Array.IndexOf(RawArray, item, 0, Length);

    /// <summary>
    /// 查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <param name="startIndex">起始索引</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int IndexOf(T item, int startIndex) => IndexOf(item, startIndex, Length - startIndex);

    /// <summary>
    /// 查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int IndexOf(T item, int startIndex, int count)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, Length);
        return Array.IndexOf(RawArray, item, startIndex, count);
    }

    /// <summary>
    /// 从后往前查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LastIndexOf(T item) => Array.LastIndexOf(RawArray, item, Length - 1, Length);

    /// <summary>
    /// 从后往前查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <param name="startIndex">起始索引</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LastIndexOf(T item, int startIndex) => LastIndexOf(item, startIndex, startIndex + 1);

    /// <summary>
    /// 从后往前查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">数量</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int LastIndexOf(T item, int startIndex, int count)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(startIndex, Length);
        return Array.LastIndexOf(RawArray, item, startIndex, count);
    }

    /// <summary>
    /// 根据条件查找元素
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>查找结果</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly T? Find(Predicate<T> match)
    {
        if (Length <= 0) return default;

        var array = RawArray;
        for (var i = 0; i < Length; i++)
        {
            var item = array[i];
            if (!match.Invoke(item)) continue;

            return item;
        }

        return default;
    }

    /// <summary>
    /// 根据条件从后往前查找元素
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>查找结果</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly T? FindLast(Predicate<T> match)
    {
        if (Length <= 0) return default;

        var array = RawArray;
        for (var i = Length - 1; i >= 0; i--)
        {
            var item = array[i];
            if (!match.Invoke(item)) continue;

            return item;
        }

        return default;
    }

    /// <summary>
    /// 根据条件查找元素，返回包含所有条件委托的元素
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>元素列表</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly List<T> FindAll(Predicate<T> match)
    {
        var list = new List<T>();
        var array = RawArray;
        for (var i = 0; i < Length; i++)
        {
            var item = array[i];
            if (!match.Invoke(item)) continue;

            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// 根据条件查找元素，若不存在则返回-1
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindIndex(Predicate<T> match) => Array.FindIndex(RawArray, 0, Length, match);

    /// <summary>
    /// 根据条件查找元素，若不存在则返回-1
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindIndex(int startIndex, Predicate<T> match) =>
        FindIndex(startIndex, Length - startIndex, match);

    /// <summary>
    /// 根据条件查找元素，若不存在则返回-1
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">查找数量</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, Length);
        return Array.FindIndex(RawArray, startIndex, count, match);
    }

    /// <summary>
    /// 根据条件从后往前查找元素，返回匹配到的索引，若不存在返回-1
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindLastIndex(Predicate<T> match) => Array.FindLastIndex(RawArray, Length - 1, Length, match);

    /// <summary>
    /// 根据条件从后往前查找元素，返回匹配到的索引，若不存在返回-1
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindLastIndex(int startIndex, Predicate<T> match) =>
        FindLastIndex(startIndex, startIndex + 1, match);

    /// <summary>
    /// 根据条件从后往前查找元素，返回匹配到的索引，若不存在返回-1
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="count">数量</param>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, Length);
        return Array.FindLastIndex(RawArray, startIndex, count, match);
    }

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int BinarySearch(T value, IComparer<T>? comparer = null) =>
        Array.BinarySearch(RawArray, 0, Length, value, comparer);

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="length">查找长度</param>
    /// <param name="value">值</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>查找结果索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int BinarySearch(int index, int length, T value, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        return Array.BinarySearch(RawArray, index, length, value, comparer);
    }

    /// <summary>
    /// 获取切片跨度
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <returns>跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Span<T> Slice(int start) => Slice(start, Length - start);

    /// <summary>
    /// 获取切片跨度
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <param name="length">长度</param>
    /// <returns>跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Span<T> Slice(int start, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(start + length, Length);
        return new Span<T>(RawArray, start, length);
    }

    /// <summary>
    /// 获取指定范围的内容至新分配池化数组
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly PooledArray<T> GetRange(int start) => GetRange(start, Length - start);

    /// <summary>
    /// 获取指定范围的内容至新分配池化数组
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <param name="length">长度</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    public readonly PooledArray<T> GetRange(int start, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(start + length, Length);
        var result = new PooledArray<T>(length);
        var source = new ReadOnlySpan<T>(_array, start, length);
        source.CopyTo(result.Span);
        return result;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Clear() => Array.Clear(RawArray, 0, Length);

    /// <summary>
    /// 反转内容
    /// </summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Reverse() => Array.Reverse(RawArray, 0, Length);

    /// <summary>
    /// 反转内容
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="length">反转长度</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public readonly void Reverse(int index, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        Array.Reverse(RawArray, index, length);
    }

    /// <summary>
    /// 排序数组
    /// </summary>
    /// <param name="comparer">比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Sort(IComparer<T>? comparer = null) => Array.Sort(RawArray, 0, Length, comparer);

    /// <summary>
    /// 排序数组
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="length">排序长度</param>
    /// <param name="comparer">比较器，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Sort(int index, int length, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        Array.Sort(RawArray, index, length, comparer);
    }

    /// <summary>
    /// 交换两个元素
    /// </summary>
    /// <param name="index1">索引1</param>
    /// <param name="index2">索引2</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public readonly void Swap(int index1, int index2)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index1, Length);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index2, Length);
        (_array![index1], _array![index2]) = (_array![index2], _array![index1]);
    }

    /// <summary>
    /// 拷贝数据到指定数组
    /// </summary>
    /// <param name="array">目标数组</param>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(T[] array) => Array.Copy(RawArray, 0, array, 0, Length);

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(T[] array, int arrayIndex) => Array.Copy(RawArray, 0, array, arrayIndex, Length);

    /// <summary>
    /// 拷贝数据到指定数组
    /// </summary>
    /// <param name="array">目标数组</param>
    /// <param name="arrayIndex">目标起始索引</param>
    /// <param name="length">拷贝长度</param>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(T[] array, int arrayIndex, int length) => CopyTo(0, array, arrayIndex, length);

    /// <summary>
    /// 拷贝数据到指定数组
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="array">目标数组</param>
    /// <param name="arrayIndex">目标起始索引</param>
    /// <param name="length">拷贝长度</param>
    [CollectionAccess(CollectionAccessType.Read)]
    public readonly void CopyTo(int index, T[] array, int arrayIndex, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(arrayIndex + length, array.Length);
        Array.Copy(RawArray, index, array, arrayIndex, length);
    }

    /// <summary>
    /// 从池化数组拷贝到数组
    /// </summary>
    /// <returns>数组</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly T[] ToArray()
    {
        var array = ArrayHelper.AllocateUninitializedArray<T>(Length);
        Array.Copy(RawArray, 0, array, 0, Length);
        return array;
    }

    /// <summary>
    /// 将池化数组内容格式化为字符串
    /// </summary>
    /// <returns>字符串</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string DumpAsString() => ReadOnlySpan.DumpAsString();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ArraySegment<T>.Enumerator GetEnumerator() => Segment.GetEnumerator();

    /// <inheritdoc />
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Dispose()
    {
        if (_array != null)
        {
            _pool.Return(_array, _clearArray);
        }

        this = default;
    }

    #region 不受支持的方法

    /// <inheritdoc />
    void ICollection<T>.Add(T item) => throw new NotImplementedException();

    /// <inheritdoc />
    void IList<T>.Insert(int index, T item) => throw new NotImplementedException();

    /// <inheritdoc />
    bool ICollection<T>.Remove(T item) => throw new NotImplementedException();

    /// <inheritdoc />
    void IList<T>.RemoveAt(int index) => throw new NotImplementedException();

    #endregion
}