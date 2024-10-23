using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Collections;

/// <summary>
/// 数组拓展类
/// </summary>
[PublicAPI]
public static class ArrayExtensions
{
    /// <summary>
    /// 若为null则返回空数组，否则返回原数组
    /// </summary>
    /// <param name="array">数组</param>
    /// <returns>原数组或空数组</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] EmptyIfNull<T>(this T[]? array) => array ?? [];
}