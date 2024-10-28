using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.IO;

/// <summary>
/// 文件拓展类
/// </summary>
[PublicAPI]
public static class FileExtensions
{
    /// <summary>
    /// 判断文件信息是否不存在或为空
    /// </summary>
    /// <param name="fileInfo">文件信息</param>
    /// <returns>是否不存在或为空</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this FileInfo fileInfo) => !fileInfo.Exists || fileInfo.Length <= 0;

    /// <summary>
    /// 判断文件信息是否存在且不为空
    /// </summary>
    /// <param name="fileInfo">文件信息</param>
    /// <returns>是否存在且不为空</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty(this FileInfo fileInfo) => !fileInfo.IsEmpty();

    /// <summary>
    /// 将文件转为非只读
    /// </summary>
    /// <param name="fileInfo">文件信息</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToNotReadOnly(this FileInfo fileInfo) => fileInfo.IsReadOnly = false;
}