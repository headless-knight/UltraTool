using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 集合拓展类
/// </summary>
[PublicAPI]
public static class SetExtensions
{
#if NET5_0_OR_GREATER
    /// <summary>
    /// 若为null则返回空集合，否则返回原集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>原集合或空集合</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlySet<T> EmptyIfNull<T>(this IReadOnlySet<T>? set) => set ?? ImmutableHashSet<T>.Empty;
#else
    /// <summary>
    /// 若为null则返回空集合，否则返回原集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>原集合或空集合</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyCollection<T> EmptyIfNull<T>(this HashSet<T>? set) =>
        set != null ? set : ImmutableHashSet<T>.Empty;
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

    /// <summary>
    /// 将集合转化为只读集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>只读集合</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static
#if NET5_0_OR_GREATER
        IReadOnlySet<T>
#else
        IReadOnlyCollection<T>
#endif
        AsReadOnly<T>(this ISet<T> set) =>
        set as
#if NET5_0_OR_GREATER
            IReadOnlySet<T>
#else
            IReadOnlyCollection<T>
#endif
        ?? new ReadOnlySetBridge<T>(set);

    /// <summary>只读集合桥接</summary>
    private sealed class ReadOnlySetBridge<T>(ISet<T> set)
#if NET5_0_OR_GREATER
        : IReadOnlySet<T>
#else
        : IReadOnlyCollection<T>
#endif
    {
        /// <inheritdoc />
        public int Count => set.Count;

        /// <inheritdoc cref="ICollection{T}.Contains" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => set.Contains(item);

        /// <inheritdoc cref="ISet{T}.IsProperSubsetOf" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

        /// <inheritdoc cref="ISet{T}.IsProperSupersetOf" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);

        /// <inheritdoc cref="ISet{T}.IsSubsetOf" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);

        /// <inheritdoc cref="ISet{T}.IsSupersetOf" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);

        /// <inheritdoc cref="ISet{T}.Overlaps" />
        [Pure, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);

        /// <inheritdoc cref="ISet{T}.SetEquals" />
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
}