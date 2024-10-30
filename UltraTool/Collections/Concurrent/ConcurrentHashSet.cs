using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections.Concurrent;

/// <summary>
/// 线程安全哈希集合，基于哈希集合与读写锁
/// </summary>
[PublicAPI]
public sealed class ConcurrentHashSet<T> : ISet<T>
#if NET5_0_OR_GREATER
    , IReadOnlySet<T>
#endif
{
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);
    private readonly HashSet<T> _set;

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc cref="ISet{T}.Count" />
    public int Count
    {
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _set.Count;
            }
            finally
            {
                if (_lock.IsReadLockHeld)
                {
                    _lock.ExitReadLock();
                }
            }
        }
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    public ConcurrentHashSet()
    {
        _set = [];
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="comparer">比较器</param>
    public ConcurrentHashSet(IEqualityComparer<T> comparer)
    {
        _set = new HashSet<T>(comparer);
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    public ConcurrentHashSet(IEnumerable<T> collection)
    {
        _set = [..collection];
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="collection">初始集合</param>
    /// <param name="comparer">比较器</param>
    public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
    {
        _set = new HashSet<T>(collection, comparer);
    }

    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <param name="action">遍历操作，入参(遍历元素)</param>
    public void ForEach(Action<T> action)
    {
        _lock.EnterReadLock();
        try
        {
            foreach (var item in _set)
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
    /// 判断元素是否在集合中
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>是否在集合中</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool Contains(T item)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.Contains(item);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public bool Add(T item)
    {
        _lock.EnterWriteLock();
        try
        {
            return _set.Add(item);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ICollection<T>.Add(T item) => Add(item);

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="range">待添加元素序列</param>
    /// <returns>成功添加数量</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public int AddRange([InstantHandle] IEnumerable<T> range)
    {
        _lock.EnterWriteLock();
        try
        {
            var count = 0;
            // 不使用Linq，避免闭包
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in range)
            {
                if (_set.Add(item))
                {
                    count++;
                }
            }

            return count;
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
            return _set.Remove(item);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.IsProperSubsetOf" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool IsSubsetOf([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.IsSubsetOf(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.IsSupersetOf" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool IsSupersetOf([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.IsSupersetOf(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.IsProperSubsetOf" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool IsProperSubsetOf([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.IsProperSubsetOf(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.IsProperSupersetOf" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool IsProperSupersetOf([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.IsProperSupersetOf(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.Overlaps" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool Overlaps([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.Overlaps(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.SetEquals" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public bool SetEquals([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterReadLock();
        try
        {
            return _set.SetEquals(other);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void UnionWith([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterWriteLock();
        try
        {
            _set.UnionWith(other);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void IntersectWith([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterWriteLock();
        try
        {
            _set.IntersectWith(other);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void ExceptWith([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterWriteLock();
        try
        {
            _set.ExceptWith(other);
        }
        finally
        {
            if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void SymmetricExceptWith([InstantHandle] IEnumerable<T> other)
    {
        _lock.EnterWriteLock();
        try
        {
            _set.SymmetricExceptWith(other);
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
            _set.Clear();
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
            _set.CopyTo(array, arrayIndex);
        }
        finally
        {
            if (_lock.IsReadLockHeld) _lock.ExitReadLock();
        }
    }

    /// <inheritdoc cref="ISet{T}.GetEnumerator" />
    /// <remarks>获取的枚举器范围为调用时元素的拷贝</remarks>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    public List<T>.Enumerator GetEnumerator()
    {
        _lock.EnterReadLock();
        try
        {
            return _set.ToList().GetEnumerator();
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
}