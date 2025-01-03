﻿using System.Runtime.CompilerServices;
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

    /// <summary>
    /// 将数组转为只读跨度
    /// </summary>
    /// <param name="array">数组</param>
    /// <returns>只读跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array) => new(array);

    /// <summary>
    /// 将数组转为只读跨度
    /// </summary>
    /// <param name="array">数组</param>
    /// <param name="start">起始索引</param>
    /// <param name="length">长度</param>
    /// <returns>只读跨度</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start, int length) =>
        new(array, start, length);

    /// <summary>
    /// 将数组转为只读内存
    /// </summary>
    /// <param name="array">数组</param>
    /// <returns>只读内存</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array) => new(array);

    /// <summary>
    /// 将数组转为只读内存
    /// </summary>
    /// <param name="array">数组</param>
    /// <param name="start">起始索引</param>
    /// <param name="length">长度</param>
    /// <returns>只读内存</returns>
    [Pure, CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array, int start, int length)
        => new(array, start, length);

    /// <summary>
    /// 将数组反转到目标跨度
    /// </summary>
    /// <param name="array">数组</param>
    /// <param name="destination">目标跨度</param>
    [CollectionAccess(CollectionAccessType.Read)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseTo<T>(this T[] array, Span<T> destination) =>
        new ReadOnlySpan<T>(array).ReverseTo(destination);

    /// <summary>
    /// 将数组内容输出为字符串
    /// </summary>
    /// <param name="array">数组</param>
    /// <returns>字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DumpAsString<T>(this T[] array) => new ReadOnlySpan<T>(array).DumpAsString();
}