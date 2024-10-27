#if NET5_0_OR_GREATER
using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 集合拓展类
/// </summary>
[PublicAPI]
public static class SetExtensions
{
#if NET5_0_OR_GREATER
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlySet<T> EmptyIfNull<T>(this IReadOnlySet<T>? set) => set ?? ImmutableHashSet<T>.Empty;
#endif

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="set">集合</param>
    /// <param name="range">待添加序列</param>
    /// <returns>成功添加个数</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public static int AddRange<T>(this ISet<T> set, [InstantHandle] IEnumerable<T> range)
    {
        if (range.TryGetNonEnumeratedCount(out var size) && size <= 0)
        {
            return 0;
        }

        var count = 0;
        // 不使用Linq，避免闭包
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var item in range)
        {
            if (set.Add(item)) count++;
        }

        return count;
    }

#if NET5_0_OR_GREATER
    /// <summary>
    /// 将集合转化为只读集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>只读集合</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlySet<T> AsReadOnly<T>(this ISet<T> set) =>
        set as IReadOnlySet<T> ?? new ReadOnlySetBridge<T>(set);

    /// <summary>只读集合桥接</summary>
    private sealed class ReadOnlySetBridge<T>(ISet<T> set) : IReadOnlySet<T>
    {
        /// <inheritdoc />
        public int Count => set.Count;

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => set.Contains(item);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);

        /// <inheritdoc />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetEquals(IEnumerable<T> other) => set.SetEquals(other);

        /// <inheritdoc />
        [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();

        /// <inheritdoc />
        [Pure, MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
#endif
}