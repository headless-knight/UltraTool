using System.Buffers;
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
    public PooledDynamicArray() : this(0, DefaultPool)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    public PooledDynamicArray([InstantHandle] IEnumerable<T> collection, bool clearArray = false) :
        this(collection, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    public PooledDynamicArray(int capacity, bool clearArray = false) : this(capacity, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    public PooledDynamicArray(int capacity, ArrayPool<T> pool, bool clearArray = false)
    {
        _pool = pool;
        _clearArray = clearArray;
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
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    public PooledDynamicArray([InstantHandle] IEnumerable<T> collection, ArrayPool<T> pool, bool clearArray = false) :
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
    /// 根据条件查找元素，若不存在则返回-1
    /// </summary>
    /// <param name="match">匹配条件</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int FindIndex(Predicate<T> match) => Array.FindIndex(GetRawArray(), 0, Length, match);

    /// <summary>
    /// 根据条件查找元素，若不存在则返回-1
    /// </summary>
    /// <param name="startIndex">起始索引</param>
    /// <param name="match">匹配条件</param>
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
    /// <param name="match">匹配条件</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public readonly int FindIndex(int startIndex, int count, Predicate<T> match)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(startIndex + count, Length);
        return Array.FindIndex(GetRawArray(), startIndex, count, match);
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

    /// <inheritdoc />
    public void Add(T item)
    {
        if (Length >= Capacity)
        {
            EnsureCapacity(Length + 1);
        }

        _array![Length++] = item;
    }

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">元素序列</param>
    public void AddRange([InstantHandle] IEnumerable<T> range)
    {
        var size = range.GetCountOrZero();
        if (Length + size > Capacity)
        {
            EnsureCapacity(Length + size);
        }

        foreach (var item in range)
        {
            Add(item);
        }
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
        if (Length >= Capacity)
        {
            EnsureCapacity(Length + 1);
        }

        for (var i = Length - 1; i >= index; i--)
        {
            _array![i + 1] = _array![i];
        }

        _array![index] = item;
        Length++;
    }

    /// <summary>
    /// 批量插入元素
    /// </summary>
    /// <param name="index">插入索引</param>
    /// <param name="range">元素序列</param>
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
        for (var i = Length - 1; i >= index; i--)
        {
            _array![i + size] = _array![i];
        }

        foreach (var item in range)
        {
            _array![index++] = item;
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
        if (Length > 0)
        {
            Array.Copy(_array!, newArray, Length);
        }

        if (_array != null)
        {
            _pool.Return(_array, _clearArray);
        }

        _array = newArray;
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
        var index = IndexOf(item);
        if (index < 0) return false;

        RemoveAt(index);
        return true;
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(index);
        ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
        for (var i = index + 1; i < Length; i++)
        {
            _array![i - 1] = _array![i];
        }

        _array![Length--] = default!;
    }

    /// <summary>
    /// 删除列表中所有满足条件的元素
    /// </summary>
    /// <param name="match">条件委托，入参(元素)</param>
    /// <returns>删除的元素数量</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public int RemoveAll(Func<T, bool> match)
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
    public void CopyTo(T[] array, int arrayIndex) => Array.Copy(GetRawArray(), 0, array, arrayIndex, Length);

    /// <inheritdoc />
    public void Clear()
    {
        if (_array != null)
        {
            Array.Clear(_array, 0, Length);
        }

        Length = 0;
    }

    /// <summary>
    /// 获取枚举器
    /// </summary>
    /// <returns>枚举器</returns>
    public ArraySegment<T>.Enumerator GetEnumerator() => GetArraySegment().GetEnumerator();

    /// <inheritdoc />
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
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