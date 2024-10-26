using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> Get<T>(int length, bool clearArray = false) => new(length, clearArray);

    /// <summary>
    /// 获取池化数组
    /// </summary>
    /// <param name="length">初始长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    /// <returns>池化数组</returns>
    [Pure, MustDisposeResource]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PooledArray<T> Get<T>(int length, ArrayPool<T> pool, bool clearArray = false) =>
        new(length, pool, clearArray);
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
    readonly bool ICollection<T>.IsReadOnly => true;

    /// <inheritdoc />
    readonly int ICollection<T>.Count => Length;

    /// <inheritdoc />
    readonly int IReadOnlyCollection<T>.Count => Length;

    /// <summary>
    /// 原始数组
    /// </summary>
    public readonly T[] RawArray => _array.EmptyIfNull();

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
    public readonly ArraySegment<T> Segment => new(_array ?? [], 0, Length);

    /// <summary>
    /// 只读序列
    /// </summary>
    public readonly ReadOnlySequence<T> ReadOnlySequence => new(_array ?? [], 0, Length);

    /// <inheritdoc cref="IList{T}.Item"/>
    public readonly T this[int index]
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Length);
            return _array![index];
        }
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    /// <param name="clearArray">是否释放时清空数组，默认为false</param>
    public PooledArray(int length, bool clearArray = false) : this(length, DefaultPool, clearArray)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="length">有效长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否释放时清空数组</param>
    public PooledArray(int length, ArrayPool<T> pool, bool clearArray)
    {
        _array = RentArray(pool, length);
        Length = length;
        _pool = pool;
        _clearArray = clearArray;
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="array">原始数组</param>
    /// <param name="length">有效长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否释放时清空数组</param>
    public PooledArray(T[] array, int length, ArrayPool<T> pool, bool clearArray)
    {
        _array = array;
        Length = length;
        _pool = pool;
        _clearArray = clearArray;
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Contains(T item) => IndexOf(item) >= 0;

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int IndexOf(T item) => Array.IndexOf(RawArray, item, 0, Length);

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Clear() => Array.Clear(RawArray, 0, Length);

    /// <summary>
    /// 获取数组切片跨度
    /// </summary>
    /// <param name="start">起始索引</param>
    /// <returns>跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Span<T> Slice(int start) => Slice(start, Length);

    /// <summary>
    /// 获取数组切片跨度
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
    /// 反转内容
    /// </summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Reverse() => Array.Reverse(RawArray, 0, Length);

    /// <summary>
    /// 二分查找
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="comparer">比较器，默认为null</param>
    /// <returns>查找结果索引</returns>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int BinarySearch(int index, int length, T value, IComparer<T>? comparer = null)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(index + length, Length);
        return Array.BinarySearch(RawArray, index, length, value, comparer);
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
    /// 拷贝数据至指定数组
    /// </summary>
    /// <param name="array">数组</param>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(T[] array) => CopyTo(array, 0);

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(T[] array, int arrayIndex) => Array.Copy(RawArray, 0, array, arrayIndex, Length);

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
    public readonly string DumpAsString()
    {
        var sb = new StringBuilder();
        sb.Append("{ ");
        sb.AppendJoin(", ", this);
        sb.Append(" }");
        return sb.ToString();
    }

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
        var pool = _pool;
        var toReturn = _array;
        var clearArray = _clearArray;
        // 清空对象
        this = default;
        if (toReturn != null)
        {
            ReturnArray(pool, toReturn, clearArray);
        }
    }

    /// <summary>归还数组</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReturnArray(ArrayPool<T> pool, T[] array, bool clearArray) => pool.Return(array, clearArray);

    /// <summary>分配数组</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T[] RentArray(ArrayPool<T> pool, int capacity) => pool.Rent(capacity);

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