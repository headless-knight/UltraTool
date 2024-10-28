using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace UltraTool.IO;

/// <summary>
/// 文件帮助类
/// </summary>
[PublicAPI]
public static class FileHelper
{
    /// <summary>Utf8无Bom</summary>
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(false, true);

    /// <summary>
    /// 打开文件流，对其他程序共享读
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <returns>文件流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream OpenStream(string filepath) =>
        new(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);

    /// <summary>
    /// 读取文件流，对其他程序共享读写
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <returns>文件流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream ReadStream(string filepath) =>
        new(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

    /// <summary>
    /// 写入文件流，对其他程序共享读
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <returns>文件流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FileStream WriteStream(string filepath) =>
        new(filepath, FileMode.Create, FileAccess.Write, FileShare.Read);

    /// <summary>
    /// 文本读取流，对其他程序共享读写
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <param name="encoding">文件编码</param>
    /// <returns>读取流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StreamReader ReadReader(string filepath, Encoding? encoding = null) =>
        new(ReadStream(filepath), encoding ?? Utf8NoBom);

    /// <summary>
    /// 文本写入流，对其他程序共享读
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <param name="encoding">文件编码</param>
    /// <returns>写入流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StreamWriter WriteWriter(string filepath, Encoding? encoding = null) =>
        new(WriteStream(filepath), encoding ?? Utf8NoBom);

    /// <summary>
    /// 文本写入流，始终创建新文件，若文件已存在则删除，对其他程序共享读
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <param name="encoding">文件编码</param>
    /// <returns>写入流</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StreamWriter CreateNewWriter(string filepath, Encoding? encoding = null)
    {
        DeleteIfExists(filepath, true);
        return WriteWriter(filepath, encoding);
    }

    /// <summary>
    /// 若文件路径存在这删除文件
    /// </summary>
    /// <param name="filepath">文件路径</param>
    /// <param name="toNotReadOnly">取消文件只读</param>
    public static void DeleteIfExists(string filepath, bool toNotReadOnly = false)
    {
        var fileInfo = new FileInfo(filepath);
        if (!fileInfo.Exists) return;

        if (toNotReadOnly)
        {
            fileInfo.ToNotReadOnly();
        }

        fileInfo.Delete();
    }

    /// <summary>
    /// 取消文件只读状态
    /// </summary>
    /// <param name="filepath">文件路径</param>
    public static void ToNotReadOnly(string filepath)
    {
        var fileInfo = new FileInfo(filepath);
        fileInfo.ToNotReadOnly();
    }
}