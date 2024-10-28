using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.IO;

/// <summary>
/// 目录帮助类
/// </summary>
[PublicAPI]
public static class DirectoryHelper
{
    /// <summary>
    /// 获取目录下的文件
    /// </summary>
    /// <param name="dir">目录</param>
    /// <param name="pattern">匹配模式，默认全匹配</param>
    /// <param name="recursive">是否遍历子目录，默认true</param>
    /// <returns>文件路径数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetFiles(string dir, string pattern = "*", bool recursive = true) =>
        Directory.GetFiles(dir, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

    /// <summary>
    /// 获取目录下的子目录
    /// </summary>
    /// <param name="dir">目录</param>
    /// <param name="pattern">匹配模式，默认全匹配</param>
    /// <param name="recursive">是否遍历子目录，默认true</param>
    /// <returns>目录路径数组</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string[] GetDirectories(string dir, string pattern = "*", bool recursive = true) =>
        Directory.GetDirectories(dir, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
}