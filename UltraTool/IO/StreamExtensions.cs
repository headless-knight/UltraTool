using System.Buffers;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.IO;

/// <summary>
/// 流拓展类
/// </summary>
[PublicAPI]
public static class StreamExtensions
{
    /// <summary>默认缓冲大小</summary>
    private const int DefaultBufferSize = 1024 * 8 * 1024;

    /// <summary>
    /// 以文件流的形式复制大文件
    /// </summary>
    /// <param name="stream">源</param>
    /// <param name="outputPath">目标地址</param>
    /// <param name="bufferSize">缓冲区大小，默认8MB</param>
    public static void CopyToFile(this Stream stream, string outputPath, int bufferSize = DefaultBufferSize)
    {
        using var fileStream = FileHelper.WriteStream(outputPath);
        using var owner = MemoryPool<byte>.Shared.Rent(bufferSize);
        int len;
        while ((len = stream.Read(owner.Memory.Span)) != 0)
        {
            fileStream.Write(owner.Memory.Span[..len]);
        }
    }

    /// <summary>
    /// 以文件流的形式复制大文件
    /// </summary>
    /// <param name="stream">源</param>
    /// <param name="outputPath">目标地址</param>
    /// <param name="bufferSize">缓冲区大小，默认8MB</param>
    /// <param name="token">取消令牌，默认为None</param>
    public static async ValueTask CopyToFileAsync(this Stream stream, string outputPath,
        int bufferSize = DefaultBufferSize, CancellationToken token = default)
    {
        await using var fileStream = FileHelper.WriteStream(outputPath);
        using var owner = MemoryPool<byte>.Shared.Rent(bufferSize);
        int len;
        while ((len = await stream.ReadAsync(owner.Memory, token).ConfigureAwait(false)) != 0)
        {
            await fileStream.WriteAsync(owner.Memory[..len], token).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 从流中获取全部字节数组数据
    /// </summary>
    /// <param name="stream">流</param>
    /// <returns>字节数据</returns>
    public static byte[] GetBytes(this Stream stream)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var destination = new MemoryStream();
        stream.CopyTo(stream);
        return destination.ToArray();
    }

    /// <summary>
    /// 从流中获取全部字节数组数据，异步方法
    /// </summary>
    /// <param name="stream">流</param>
    /// <param name="token">取消令牌，默认为None</param>
    /// <returns>字节数据</returns>
    public static async Task<byte[]> GetBytesAsync(this Stream stream, CancellationToken token = default)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var destination = new MemoryStream();
        await stream.CopyToAsync(stream, token).ConfigureAwait(false);
        return destination.ToArray();
    }

    /// <summary>
    /// 将只读字节序列包装为流
    /// </summary>
    /// <param name="sequence">只读字节序列</param>
    /// <returns>流</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Stream WrapStream(this ReadOnlySequence<byte> sequence) =>
        new ReadOnlySequenceStreamBridge(sequence);

    /// <summary>只读序列流桥接</summary>
    private sealed class ReadOnlySequenceStreamBridge(ReadOnlySequence<byte> sequence) : Stream
    {
        /// <inheritdoc />
        public override bool CanRead => Position < Length;

        /// <inheritdoc />
        public override bool CanSeek => true;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length => sequence.Length;

        /// <inheritdoc />
        public override long Position { get; set; }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            var slice = sequence.Slice(Position, count);
            slice.CopyTo(buffer.AsSpan(offset, count));
            Position += count;
            return count;
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                {
                    Position = offset;
                    break;
                }
                case SeekOrigin.Current:
                {
                    Position += offset;
                    break;
                }
                case SeekOrigin.End:
                {
                    Position = sequence.Length - offset;
                    break;
                }
                default: throw new ArgumentOutOfRangeException(nameof(origin), origin, "Unsupported SeekOrigin");
            }

            return Position;
        }

        #region 不支持操作

        /// <inheritdoc />
        public override void SetLength(long value) => throw new NotImplementedException();

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        /// <inheritdoc />
        public override void Flush() => throw new NotImplementedException();

        #endregion
    }
}