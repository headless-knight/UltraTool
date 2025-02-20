﻿using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Collections;
using UltraTool.Helpers;

namespace UltraTool;

/// <summary>
/// 池化动态数组
/// </summary>
[PublicAPI, MustDisposeResource]
public struct PooledDynamicArray<T> : IList<T>, IReadOnlyList<T>, IDisposable
{
    /// <summary>默认数组池</summary>
    private static ArrayPool<T> DefaultPool => ArrayPool<T>.Shared;

    private readonly ArrayPool<T> _pool;
    private readonly bool _clearArray;
    private T[]? _array;

    /// <summary>
    /// 长度
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// 容量
    /// </summary>
    public readonly int Capacity => _array?.Length ?? 0;

    /// <summary>
    /// 是否为空
    /// </summary>
    public readonly bool IsEmpty => Length <= 0;

    /// <inheritdoc />
    readonly bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc />
    readonly int ICollection<T>.Count => Length;

    /// <inheritdoc />
    readonly int IReadOnlyCollection<T>.Count => Length;

    /// <inheritdoc cref="IList{T}.this"/>
    public T this[int index]
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        readonly get
        {
            ArgumentOutOfRangeHelper.ThrowIfNegative(index);
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
            return _array![index];
        }
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        set
        {
            ArgumentOutOfRangeHelper.ThrowIfNegative(index);
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
            _array![index] = value;
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    public PooledDynamicArray() : this(0, DefaultPool)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledDynamicArray([InstantHandle] IEnumerable<T> collection, bool? clearArray = null) :
        this(collection, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledDynamicArray(int capacity, bool? clearArray = null) : this(capacity, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledDynamicArray(int capacity, ArrayPool<T> pool, bool? clearArray = null)
    {
        _pool = pool;
        _clearArray = clearArray ?? RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        if (capacity > 0)
        {
            _array = _pool.Rent(capacity);
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认null</param>
    /// <remarks>若clearArray值为null则根据元素类型判断，为引用类型则等同于true，否则等同于false</remarks>
    public PooledDynamicArray([InstantHandle] IEnumerable<T> collection, ArrayPool<T> pool, bool? clearArray = null) :
        this(collection.GetCountOrZero(), pool, clearArray)
    {
        AddRange(collection);
    }

    /// <summary>
    /// 获取原始数组
    /// </summary>
    /// <returns>数组</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T[] GetRawArray() => _array.EmptyIfNull();

    /// <summary>
    /// 获取可枚举对象
    /// </summary>
    /// <returns>可枚举对象</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly IEnumerable<T> GetEnumerable() => GetRawArray().Take(Length);

    /// <summary>
    /// 获取跨度
    /// </summary>
    /// <returns>跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Span<T> GetSpan() => new(_array, 0, Length);

    /// <summary>
    /// 获取只读跨度
    /// </summary>
    /// <returns>只读跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ReadOnlySpan<T> GetReadOnlySpan() => new(_array, 0, Length);

    /// <summary>
    /// 获取内存
    /// </summary>
    /// <returns>内存</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Memory<T> GetMemory() => new(_array, 0, Length);

    /// <summary>
    /// 获取只读内存
    /// </summary>
    /// <returns>只读内存</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ReadOnlyMemory<T> GetReadOnlyMemory() => new(_array, 0, Length);

    /// <summary>
    /// 获取数组段
    /// </summary>
    /// <returns>数组段</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ArraySegment<T> GetArraySegment() => new(GetRawArray(), 0, Length);

    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="action">遍历操作，入参(元素)</param>
    public readonly void ForEach(Action<T> action)
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
    public readonly int IndexOf(T item) => Array.IndexOf(GetRawArray(), item, 0, Length);

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
        return Array.IndexOf(GetRawArray(), item, startIndex, count);
    }

    /// <summary>
    /// 从后往前查找指定元素的索引，若不存在返回-1
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LastIndexOf(T item) => Array.LastIndexOf(GetRawArray(), item, Length - 1, Length);

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
        return Array.LastIndexOf(GetRawArray(), item, startIndex, count);
    }

    /// <summary>
    /// 根据条件查找元素
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>查找结果</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly T? Find(Predicate<T> match)
    {
        if (_array is not { Length: > 0 }) return default;

        for (var i = 0; i < Length; i++)
        {
            var item = _array[i];
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
        if (_array is not { Length: > 0 }) return default;

        for (var i = Length - 1; i >= 0; i--)
        {
            var item = _array[i];
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
        if (_array is not { Length: > 0 }) return [];

        var list = new List<T>();
        for (var i = 0; i < Length; i++)
        {
            var item = _array[i];
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
    public readonly int FindIndex(Predicate<T> match) => Array.FindIndex(GetRawArray(), 0, Length, match);

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
        return Array.FindIndex(GetRawArray(), startIndex, count, match);
    }

    /// <summary>
    /// 根据条件从后往前查找元素，返回匹配到的索引，若不存在返回-1
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindLastIndex(Predicate<T> match) =>
        Array.FindLastIndex(GetRawArray(), Length - 1, Length, match);

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
        return Array.FindLastIndex(GetRawArray(), startIndex, count, match);
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
        Array.BinarySearch(GetRawArray(), 0, Length, value, comparer);

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
        return Array.BinarySearch(GetRawArray(), index, length, value, comparer);
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
    public readonly Span<T> Slice(int start, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(start + length, Length);
        return new Span<T>(GetRawArray(), start, length);
    }

    /// <summary>
    /// 获取指定范围的内容至新分配池化动态数组
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <returns>池化动态数组</returns>
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly PooledDynamicArray<T> GetRange(int start) => GetRange(start, Length - start);

    /// <summary>
    /// 获取指定范围的内容至新分配池化动态数组
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <param name="length">长度</param>
    /// <returns>池化动态数组</returns>
    [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
    public readonly PooledDynamicArray<T> GetRange(int start, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(start + length, Length);
        var result = new PooledDynamicArray<T>(length);
        result.AddRange(GetReadOnlySpan());
        return result;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Add(T item)
    {
        EnsureCapacity(Length + 1);
        GetRawArray()[Length++] = item;
    }

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">待添加序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void AddRange([InstantHandle] IEnumerable<T> range)
    {
        switch (range)
        {
            case T[] array:
            {
                AddRange(array);
                return;
            }
            case ICollection<T> coll:
            {
                EnsureCapacity(Length + coll.Count);
                coll.CopyTo(GetRawArray(), Length);
                Length += coll.Count;
                return;
            }
            case IReadOnlyCollection<T> coll:
            {
                EnsureCapacity(Length + coll.Count);
                var array = GetRawArray();
                foreach (var item in coll)
                {
                    array[Length++] = item;
                }

                return;
            }
            default:
            {
                foreach (var item in range)
                {
                    Add(item);
                }

                return;
            }
        }
    }

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">待添加数组</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddRange(T[] range) => AddRange(new ReadOnlySpan<T>(range));

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">待添加跨度</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void AddRange(ReadOnlySpan<T> range)
    {
        EnsureCapacity(Length + range.Length);
        var destination = new Span<T>(_array, Length, range.Length);
        range.CopyTo(destination);
        Length += range.Length;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Insert(int index, T item)
    {
        EnsureCapacity(Length + 1);
        var array = GetRawArray();
        for (var i = Length - 1; i >= index; i--)
        {
            array[i + 1] = array[i];
        }

        array[index] = item;
        Length++;
    }

    /// <summary>
    /// 批量插入元素
    /// </summary>
    /// <param name="index">插入索引</param>
    /// <param name="range">待插入序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void InsertRange(int index, [InstantHandle] IEnumerable<T> range)
    {
        if (!range.TryGetNonEnumeratedCount(out var size))
        {
            foreach (var item in range)
            {
                Insert(index++, item);
            }

            return;
        }

        EnsureCapacity(Length + size);
        var array = GetRawArray();
        for (var i = Length - 1; i >= index; i--)
        {
            array[i + size] = array[i];
        }

        foreach (var item in range)
        {
            array[index++] = item;
        }

        Length += size;
    }

    /// <summary>
    /// 确保容量，若当前容量小于指定容量则扩容
    /// </summary>
    /// <param name="capacity">容量</param>
    public void EnsureCapacity(int capacity)
    {
        if (capacity <= Capacity) return;

        var newCapacity = Math.Max(Capacity, 4) << 1;
        if (newCapacity < capacity)
        {
            newCapacity = capacity;
        }

        var newArray = _pool.Rent(newCapacity);
        if (!IsEmpty)
        {
            Array.Copy(GetRawArray(), newArray, Length);
        }

        if (_array != null)
        {
            _pool.Return(_array, _clearArray);
        }

        _array = newArray;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index < 0) return false;

        RemoveAt(index);
        return true;
    }

    /// <summary>
    /// 删除指定范围的元素
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="count">删除数量</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void RemoveRange(int index, int count)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + count, Length);
        var array = GetRawArray();
        Array.Copy(array, index + count, array, index, Length - index - count);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            Array.Clear(array, Length - count, count);
        }

        Length -= count;
    }

    /// <summary>
    /// 删除列表中所有满足条件的元素
    /// </summary>
    /// <param name="match">条件委托</param>
    /// <returns>删除的元素数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public int RemoveAll(Predicate<T> match)
    {
        if (_array is not { Length: > 0 }) return 0;

        var count = 0;
        for (var i = Length - 1; i >= 0; i--)
        {
            if (!match.Invoke(_array[i])) continue;

            RemoveAt(i);
            count++;
        }

        return count;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void RemoveAt(int index)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
        var array = GetRawArray();
        for (var i = index + 1; i < Length; i++)
        {
            array[i - 1] = array[i];
        }

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            array[Length - 1] = default!;
        }

        Length--;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Clear()
    {
        if (!IsEmpty && RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            Array.Clear(GetRawArray(), 0, Length);
        }

        Length = 0;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex) => Array.Copy(GetRawArray(), 0, array, arrayIndex, Length);

    /// <summary>
    /// 反转数组内容
    /// </summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reverse() => Array.Reverse(GetRawArray(), 0, Length);

    /// <summary>
    /// 反转指定范围数组内容
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="length">反转长度</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Reverse(int index, int length)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        Array.Reverse(GetRawArray(), index, length);
    }

    /// <summary>
    /// 将池化数组内容格式化为字符串
    /// </summary>
    /// <returns>字符串</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string DumpAsString() => GetReadOnlySpan().DumpAsString();

    /// <summary>
    /// 获取枚举器
    /// </summary>
    /// <returns>枚举器</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySegment<T>.Enumerator GetEnumerator() => GetArraySegment().GetEnumerator();

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Dispose()
    {
        if (_array != null)
        {
            _pool.Return(_array, _clearArray);
        }

        this = default;
    }

    /// <summary>
    /// 构建为池化数组
    /// </summary>
    /// <remarks>内部数组所有权应归于池化数组</remarks>
    /// <returns>池化数组</returns>
    [MustDisposeResource]
    internal PooledArray<T> BuildPooledArray() => new(_array!, Length, _pool, _clearArray);
}