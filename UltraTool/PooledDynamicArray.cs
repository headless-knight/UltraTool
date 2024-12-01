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
    public readonly int IndexOf(T item) => Array.IndexOf(_array!, item, Length);

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
        var count = range.GetCountOrZero();
        if (Length + count > Capacity)
        {
            EnsureCapacity(Length + count);
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
    internal PooledArray<T> BuildPooledArray() => new(_array!, Length, _pool, _clearArray);
}