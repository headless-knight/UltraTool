using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 集合拓展类
/// </summary>
public static class SetExtensions
{
    /// <summary>
    /// 若为null则返回空集合，否则返回原集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>原集合或空集合</returns>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static
#if NET5_0_OR_GREATER
        IReadOnlySet<T>
#else
        IReadOnlyCollection<T>
#endif
        EmptyIfNull<T>(
#if NET5_0_OR_GREATER
            this IReadOnlySet<T>? set
#else
        this HashSet<T>? set
#endif
        ) => set != null ? set : ReadOnlySetBridge<T>.Empty;

    /// <summary>
    /// 批量添加元素
    /// </summary>
    /// <param name="set">集合</param>
    /// <param name="range">待添加序列</param>
    /// <returns>成功添加个数</returns>
    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AddRange<T>(this ISet<T> set, [InstantHandle] IEnumerable<T> range) => range.Count(set.Add);

#if !NET9_0_OR_GREATER
    /// <summary>
    /// 将集合转化为只读集合
    /// </summary>
    /// <param name="set">集合</param>
    /// <returns>只读集合</returns>
    [CollectionAccess(CollectionAccessType.Read)]
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
#endif

    /// <summary>只读集合桥接</summary>
    private sealed class ReadOnlySetBridge<T>(ISet<T> set)
#if NET5_0_OR_GREATER
        : IReadOnlySet<T>
#else
        : IReadOnlyCollection<T>
#endif
    {
        /// <summary>
        /// 空只读集合
        /// </summary>
        public static ReadOnlySetBridge<T> Empty { get; } = new(new HashSet<T>());

        /// <inheritdoc />
        public int Count => set.Count;

        /// <inheritdoc cref="ICollection{T}.Contains" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => set.Contains(item);

        /// <inheritdoc cref="ISet{T}.IsProperSubsetOf" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

        /// <inheritdoc cref="ISet{T}.IsProperSupersetOf" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);

        /// <inheritdoc cref="ISet{T}.IsSubsetOf" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);

        /// <inheritdoc cref="ISet{T}.IsSupersetOf" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);

        /// <inheritdoc cref="ISet{T}.Overlaps" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);

        /// <inheritdoc cref="ISet{T}.SetEquals" />
        [CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetEquals(IEnumerable<T> other) => set.SetEquals(other);

        /// <inheritdoc />
        [MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();

        /// <inheritdoc />
        [MustDisposeResource, CollectionAccess(CollectionAccessType.Read)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}