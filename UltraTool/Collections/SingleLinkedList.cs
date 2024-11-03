using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;

namespace UltraTool.Collections;

/// <summary>
/// 单链表
/// </summary>
[PublicAPI]
public sealed class SingleLinkedList<T> : ICollection<T>, IReadOnlyCollection<T>
{
    private int _version;

    /// <summary>
    /// 头节点
    /// </summary>
    public SingleLinkedListNode<T>? First { get; private set; }

    /// <summary>
    /// 尾节点
    /// </summary>
    /// <remarks>从头遍历至尾节点</remarks>
    public SingleLinkedListNode<T>? Last
    {
        [CollectionAccess(CollectionAccessType.Read)]
        get
        {
            var node = First;
            while (node is { Next: not null })
            {
                node = node.Next;
            }

            return node;
        }
    }

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count { get; private set; }

    /// <summary>
    /// 单链表是否为空
    /// </summary>
#if NET6_0_OR_GREATER
    [MemberNotNullWhen(false, nameof(First), nameof(Last))]
#endif
    public bool IsEmpty => Count <= 0;

    /// <inheritdoc />
    bool ICollection<T>.IsReadOnly => false;

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item) => IndexOf(item) >= 0;

    /// <summary>
    /// 计算元素在链表中的索引
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>索引</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    public int IndexOf(T item)
    {
        if (Count <= 0) return -1;

        var index = 0;
        var node = First;
        while (node != null)
        {
            if (EqualityComparer<T>.Default.Equals(item, node.Value))
            {
                return index;
            }

            node = node.Next;
            index++;
        }

        return -1;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void Add(T item)
    {
        var last = Last;
        // 空链表添加元素
        if (last == null)
        {
            AddForEmptyList(item);
            return;
        }

        AddAfterInternal(last, item);
    }

    /// <summary>
    /// 添加元素为头节点
    /// </summary>
    /// <param name="item">元素</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void AddFirst(T item)
    {
        // 空链表添加元素
        if (First == null)
        {
            AddForEmptyList(item);
            return;
        }

        var newNode = new SingleLinkedListNode<T>(item)
        {
            List = this,
            Next = First
        };
        First = newNode;
        Count++;
        _version++;
    }

    /// <summary>
    /// 在指定节点之后添加元素
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="item">元素</param>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public void AddAfter(SingleLinkedListNode<T> node, T item)
    {
        if (node.List != this)
        {
            throw new ArgumentException("The node does not belong to the current list", nameof(node));
        }

        AddAfterInternal(node, item);
    }

    /// <summary>空链表添加元素</summary>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    private void AddForEmptyList(T item)
    {
        First = new SingleLinkedListNode<T>(item)
        {
            List = this
        };
        Count++;
        _version++;
    }

    /// <summary>在指定节点后添加元素，内部实现</summary>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    private void AddAfterInternal(SingleLinkedListNode<T> node, T item)
    {
        var newNode = new SingleLinkedListNode<T>(item)
        {
            List = this,
            Next = node.Next
        };
        node.Next = newNode;
        Count++;
        _version++;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public bool Remove(T item)
    {
        SingleLinkedListNode<T>? prev = null;
        var node = First;
        while (node != null)
        {
            // 找到元素
            if (EqualityComparer<T>.Default.Equals(item, node.Value))
            {
                // 上一个节点为空，说明链表只有一个元素
                if (prev == null)
                {
                    First = null;
                    Count--;
                    _version++;
                    return true;
                }

                prev.Next = node.Next;
                Count--;
                _version++;
                return true;
            }

            prev = node;
            node = node.Next;
        }

        return false;
    }

    /// <summary>
    /// 删除头节点
    /// </summary>
    /// <returns>头节点元素</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T RemoveFirst() => TryRemoveFirst(out var item)
        ? item
        : throw new InvalidOperationException("The linked list is empty");

    /// <summary>
    /// 尝试删除头节点
    /// </summary>
    /// <param name="item">元素</param>
    /// <returns>是否成功删除</returns>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public bool TryRemoveFirst([MaybeNullWhen(false)] out T item)
    {
        if (First == null)
        {
            item = default;
            return false;
        }

        item = First.Value;
        First = First.Next;
        Count--;
        _version++;
        return true;
    }

    /// <summary>
    /// 删除指定节点之后一个节点
    /// </summary>
    /// <param name="node">指定节点</param>
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void RemoveAfter(SingleLinkedListNode<T> node)
    {
        if (node.List != this)
        {
            throw new ArgumentException("The node does not belong to the current list", nameof(node));
        }

        // 节点为尾节点
        if (node.Next == null) return;

        node.Next = node.Next.Next;
        Count--;
        _version++;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.ModifyExistingContent)]
    public void Clear()
    {
        var node = First;
        while (node != null)
        {
            var next = node.Next;
            node.Next = null;
            node.List = null;
            node = next;
        }

        First = null;
        _version++;
    }

    /// <inheritdoc />
    [CollectionAccess(CollectionAccessType.Read)]
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentOutOfRangeHelper.ThrowIfGreaterThan(arrayIndex + Count, array.Length);
        foreach (var item in this)
        {
            array[arrayIndex++] = item;
        }
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new(First);

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// 枚举器
    /// </summary>
    public struct Enumerator : IEnumerator<T>
    {
        private readonly int? _version;

        private readonly SingleLinkedListNode<T> _beforeFirst;

        private SingleLinkedListNode<T>? _current;

        /// <inheritdoc />
        public T Current => _current != null ? _current.Value : default!;

        /// <inheritdoc />
        object IEnumerator.Current => Current!;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="first">头节点</param>
        public Enumerator(SingleLinkedListNode<T>? first)
        {
            _version = first?.List?._version;
            _beforeFirst = new SingleLinkedListNode<T>(default!)
            {
                Next = first
            };
            _current = _beforeFirst;
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (_version != _beforeFirst.Next?.List?._version)
            {
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            }

            if (_current?.Next == null) return false;

            _current = _current.Next;
            return true;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _current = _beforeFirst;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}