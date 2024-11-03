using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Randoms;

namespace UltraTool.Collections.Concurrent;

/// <summary>
/// 线程安全列表，基于列表与读写锁
/// </summary>
[PublicAPI]
public class ConcurrentList<T> : IList<T>, IReadOnlyList<T>
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);
    private readonly List<T> _list;

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
    public int Count
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Count;
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }
    }

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc cref="IList{T}.this[int]" />
    public T this[int index]
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _list[index];
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }
        [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
        set
        {
            _lock.EnterWriteLock();
            try
            {
                _list[index] = value;
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    public ConcurrentList()
    {
        _list = [];
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="capacity">初始容量</param>
    public ConcurrentList(int capacity)
    {
        _list = new List<T>(capacity);
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    public ConcurrentList(IEnumerable<T> collection)
    {
        _list = [..collection];
    }

    /// <summary>
    /// 遍历列表
    /// </summary>
    /// <param name="action">遍历委托，入参(遍历值)</param>
    [CollectionAccess(CollectionAccessType.Read)]
    public void ForEach(Action<T> action)
    {
        _lock.EnterReadLock();
        try
        {
            foreach (var item in _list)
            {
                action.Invoke(item);
            }
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// 遍历列表
    /// </summary>
    /// <param name="action">遍历委托，入参(遍历值，索引)</param>
    [CollectionAccess(CollectionAccessType.Read)]
    public void ForEach(Action<T, int> action)
    {
        _lock.EnterReadLock();
        try
        {
            for (var i = 0; i < _list.Count; i++)
            {
                action.Invoke(_list[i], i);
            }
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool Contains(T item)
    {
        _lock.EnterReadLock();
        try
        {
            return _list.Contains(item);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public int IndexOf(T item)
    {
        _lock.EnterReadLock();
        try
        {
            return _list.IndexOf(item);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// 获取并更新值
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="updateValue">更新值</param>
    /// <returns>获取值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.ModifyExistingContent)]
    public T GetAndUpdate(int index, T updateValue)
    {
        _lock.EnterWriteLock();
        try
        {
            var got = _list[index];
            _list[index] = updateValue;
            return got;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 获取并更新值
    /// </summary>
    /// <param name="index">索引</param>
    /// <param name="updater">更新委托，入参(当前值)</param>
    /// <returns>获取值</returns>
    [CollectionAccess(CollectionAccessType.Read | CollectionAccessType.ModifyExistingContent)]
    public T GetAndUpdate(int index, Func<T, T> updater)
    {
        _lock.EnterWriteLock();
        try
        {
            var got = _list[index];
            _list[index] = updater.Invoke(got);
            return got;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Add(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Add(item);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">元素序列</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void AddRange([InstantHandle] IEnumerable<T> range)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.AddRange(range);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Insert(int index, T item)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Insert(index, item);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public bool Remove(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            return _list.Remove(item);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void RemoveAt(int index)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.RemoveAt(index);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 删除列表第一个元素
    /// </summary>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public T RemoveFirst()
    {
        _lock.EnterWriteLock();
        try
        {
            var removed = _list[0];
            _list.RemoveAt(0);
            return removed;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 删除列表中匹配到的第一个元素
    /// </summary>
    /// <param name="match">匹配条件</param>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public T? RemoveFirst(Func<T, bool> match)
    {
        _lock.EnterWriteLock();
        try
        {
            for (var i = 0; i < _list.Count; i++)
            {
                if (!match.Invoke(_list[i])) continue;

                var removed = _list[i];
                _list.RemoveAt(i);
                return removed;
            }

            return default;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 删除列表最后一个元素
    /// </summary>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public T RemoveLast()
    {
        _lock.EnterWriteLock();
        try
        {
            var removed = _list[^1];
            _list.RemoveAt(_list.Count - 1);
            return removed;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 删除列表中匹配到的最后一个元素
    /// </summary>
    /// <param name="match">匹配条件</param>
    /// <returns>删除的元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public T? RemoveLast(Func<T, bool> match)
    {
        _lock.EnterWriteLock();
        try
        {
            for (var i = _list.Count - 1; i >= 0; i--)
            {
                if (!match.Invoke(_list[i])) continue;

                var removed = _list[i];
                _list.RemoveAt(i);
                return removed;
            }

            return default;
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Clear();
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    public void CopyTo(T[] array, int arrayIndex)
    {
        _lock.EnterReadLock();
        try
        {
            _list.CopyTo(array, arrayIndex);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    /// <remarks>获取的枚举器范围为调用时元素的拷贝</remarks>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public List<T>.Enumerator GetEnumerator()
    {
        _lock.EnterReadLock();
        try
        {
            return _list.ToList().GetEnumerator();
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// 二分查找指定元素索引
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public int BinarySearch(T item)
    {
        _lock.EnterReadLock();
        try
        {
            return _list.BinarySearch(item);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// 二分查找指定元素索引
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="count">数量</param>
    /// <param name="item">元素</param>
    /// <param name="comparer">比较器</param>
    /// <returns>索引</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
        _lock.EnterReadLock();
        try
        {
            return _list.BinarySearch(index, count, item, comparer);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// 排序列表
    /// </summary>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Sort()
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Sort();
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 排序列表
    /// </summary>
    /// <param name="comparer">比较器</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Sort(IComparer<T>? comparer)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Sort(comparer);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 排序列表指定范围
    /// </summary>
    /// <param name="index">起始索引</param>
    /// <param name="count">范围</param>
    /// <param name="comparer">比较器</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Sort(int index, int count, IComparer<T>? comparer)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Sort(index, count, comparer);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// 随机打乱列表顺序
    /// </summary>
    /// <param name="random">随机对象，默认为null</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Shuffle(Random? random = null)
    {
        _lock.EnterWriteLock();
        try
        {
            random ??= RandomHelper.Shared;
            for (var i = _list.Count - 1; i > 0; i--)
            {
                var index = random.Next(i);
                (_list[i], _list[index]) = (_list[index], _list[i]);
            }
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }
}