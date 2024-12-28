using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;

namespace UltraTool.Collections;

/// <summary>
/// 双端队列
/// </summary>
[PublicAPI]
public class Deque<T> : ICollection<T>, IReadOnlyList<T>
{
    private T[] _items;
    private int _head;
    private int _version;

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count { get; private set; }

    /// <summary>
    /// 容量
    /// </summary>
    public int Capacity
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _items.Length;
        [CollectionAccess(CollectionAccessType.UpdatedContent)]
        set
        {
            ArgumentOutOfRangeHelper.ThrowIfLessThan(value, Count);
            var newArray = AllocateArray(value);
            CopyTo(newArray, 0);
            _items = newArray;
            _head = 0;
            _version++;
        }
    }

    /// <summary>
    /// 双端队列是否为空
    /// </summary>
    public bool IsEmpty => Count <= 0;

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc />
    public T this[int index]
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        get
        {
            ArgumentOutOfRangeHelper.ThrowIfNegative(index);
            ArgumentOutOfRangeHelper.ThrowIfGreaterThanOrEqual(index, Count);
            return _items[(index + _head) % Capacity];
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    public Deque() : this(0)
    {
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    public Deque(int capacity)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(capacity);
        _items = AllocateArray(capacity);
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    public Deque([InstantHandle] IEnumerable<T> collection)
    {
        _items = AllocateArray(collection.GetCountOrZero());
        foreach (var item in collection)
        {
            EnqueueLast(item);
        }
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool Contains(T item)
    {
        var index = _head;
        var count = 0;
        while (count < Count)
        {
            if (EqualityComparer<T>.Default.Equals(_items[index], item))
            {
                return true;
            }

            count++;
            index = (index + 1) % Capacity;
        }

        return false;
    }

    /// <summary>
    /// 获取队首元素
    /// </summary>
    /// <returns>元素</returns>
    public T PeekFirst() =>
        TryPeekFirst(out var item) ? item : throw new InvalidOperationException("Deque is empty");

    /// <summary>
    /// 获取队尾元素
    /// </summary>
    /// <returns>元素</returns>
    public T PeekLast() =>
        TryPeekLast(out var item) ? item : throw new InvalidOperationException("Deque is empty");

    /// <summary>
    /// 尝试获取队首元素
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>是否成功获取</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public bool TryPeekFirst([MaybeNullWhen(false)] out T item)
    {
        if (Count <= 0)
        {
            item = default;
            return false;
        }

        item = this[0];
        return true;
    }

    /// <summary>
    /// 尝试获取队尾元素
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>是否成功获取</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public bool TryPeekLast([MaybeNullWhen(false)] out T item)
    {
        if (Count <= 0)
        {
            item = default;
            return false;
        }

        item = this[Count - 1];
        return true;
    }

    /// <summary>
    /// 添加元素至队首
    /// </summary>
    /// <param name="item">元素</param>
    public void EnqueueFirst(T item)
    {
        EnsureCapacity(Count + 1);
        _head = (_head - 1 + Capacity) % Capacity;
        _items[_head] = item;
        Count++;
        _version++;
    }

    /// <summary>
    /// 添加元素至队尾
    /// </summary>
    /// <param name="item">元素</param>
    public void EnqueueLast(T item)
    {
        EnsureCapacity(Count + 1);
        _items[(_head + Count) % Capacity] = item;
        Count++;
        _version++;
    }

    /// <summary>
    /// 删除队首元素
    /// </summary>
    /// <returns>队首元素</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T DequeueFirst() =>
        TryDequeueFirst(out var item) ? item : throw new InvalidOperationException("Deque is empty");

    /// <summary>
    /// 尝试删除队首元素
    /// </summary>
    /// <param name="item">队首元素</param>
    /// <returns>是否成功删除</returns>
    public bool TryDequeueFirst([MaybeNullWhen(false)] out T item)
    {
        if (Count <= 0)
        {
            item = default;
            return false;
        }

        item = _items[_head];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _items[_head] = default!;
        }

        _head = (_head + 1) % Capacity;
        Count--;
        _version++;
        return true;
    }

    /// <summary>
    /// 删除队尾元素
    /// </summary>
    /// <returns>队尾元素</returns>
    public T DequeueLast() =>
        TryDequeueLast(out var item) ? item : throw new InvalidOperationException("Deque is empty");

    /// <summary>
    /// 尝试删除队尾元素
    /// </summary>
    /// <param name="item">队尾元素</param>
    /// <returns>是否成功删除</returns>
    public bool TryDequeueLast([MaybeNullWhen(false)] out T item)
    {
        if (Count <= 0)
        {
            item = default;
            return false;
        }

        var tail = (_head + Count - 1) % Capacity;
        item = _items[tail];
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            _items[tail] = default!;
        }

        Count--;
        _version++;
        return true;
    }

    /// <inheritdoc />
    public void Clear()
    {
        if (Count <= 0) return;

        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            Array.Clear(_items, 0, _items.Length);
        }

        _head = Count = 0;
        _version++;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (Count <= 0) return;

        var tail = (_head + Count) % Capacity;
        if (_head <= tail)
        {
            Array.Copy(_items, _head, array, arrayIndex, Count);
        }
        else
        {
            Array.Copy(_items, _head, array, arrayIndex, Capacity - _head);
            Array.Copy(_items, 0, array, arrayIndex + Capacity - _head, tail);
        }
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public Enumerator GetEnumerator() => new(this);

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    void ICollection<T>.Add(T item) => EnqueueLast(item);

    /// <inheritdoc />
    public bool Remove(T item) => throw new NotImplementedException();

    /// <summary>确保容量</summary>
    private void EnsureCapacity(int capacity)
    {
        if (capacity <= _items.Length) return;

        var newCapacity = Math.Max(_items.Length, 4) << 1;
        if (newCapacity < capacity)
        {
            newCapacity = capacity;
        }

        Capacity = newCapacity;
    }

    /// <summary>分配数组</summary>
    private static T[] AllocateArray(int capacity) => ArrayHelper.AllocateUninitializedArray<T>(capacity);

    /// <summary>
    /// 枚举器
    /// </summary>
    /// <param name="deque">双端队列</param>
    public struct Enumerator(Deque<T> deque) : IEnumerator<T>
    {
        private readonly int _version = deque._version;
        private int _index = -1;

        /// <inheritdoc />
        public T Current => deque[_index];

        /// <inheritdoc />
        object? IEnumerator.Current => Current;

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (_version != deque._version)
            {
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            }

            return ++_index < deque.Count;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _index = -1;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}