using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using JetBrains.Annotations;
#if !NET7_0_OR_GREATER
using UltraTool.Collections;
#endif
using UltraTool.Helpers;

namespace UltraTool.Cryptography;

/// <summary>
/// RSA加密拓展类
/// </summary>
[PublicAPI]
public static class RSAExtensions
{
#if !NET8_0_OR_GREATER
    /// <summary>
    /// 获取RSA操作可以生成的最大字节数
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <returns>最大字节数</returns>
    public static int GetMaxOutputSize(this RSA rsa)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegativeOrZero(rsa.KeySize);
        return rsa.KeySize + 7 >>> 3;
    }
#endif

    /// <summary>
    /// 获取Pkcs1模式下，单个分组可输入最大长度
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <returns>分组可输入最大长度</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetPkcs1InputMaxLength(this RSA rsa) => rsa.KeySize / 8 - 11;

    /// <summary>
    /// 获取Pkcs1模式下，单个分组加密输出长度
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <returns>分组加密输出长度</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetPkcs1OutputBlockLength(this RSA rsa) => rsa.GetMaxOutputSize();

    /// <summary>
    /// 通过明文长度，计算Pkcs1模式下，分组加密的密文长度
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="plaintextLength">明文字节长度</param>
    /// <returns>密文字节长度</returns>
    [Pure]
    public static int GetPkcs1CiphertextLength(this RSA rsa, int plaintextLength)
    {
        if (plaintextLength <= 0) return 0;

        // 分组明文长度
        var blockLength = rsa.GetPkcs1InputMaxLength();
        // 明文分组数量
        var numOfBlocks = (plaintextLength + blockLength - 1) / blockLength;
        // 分组数量 * 分组密文长度
        return numOfBlocks * rsa.GetPkcs1OutputBlockLength();
    }

    /// <summary>
    /// 通过密文长度，计算Pkcs1填充模式下，明文可输入的最大长度
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="ciphertextLength">密文长度</param>
    /// <returns>明文可输入最大长度</returns>
    [Pure]
    public static int GetPkcs1PlainTextMaxLength(this RSA rsa, int ciphertextLength)
    {
        if (ciphertextLength <= 0) return 0;

        // 分组密文长度
        var blockLength = rsa.GetPkcs1OutputBlockLength();
        // 密文分组数量
        var numOfBlocks = (ciphertextLength + blockLength - 1) / blockLength;
        // 分组数量 * 分组明文最大长度
        return numOfBlocks * rsa.GetPkcs1InputMaxLength();
    }

    /// <summary>
    /// 分组加密，使用Pkcs1进行填充
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="plaintext">明文</param>
    /// <param name="pool">数组池，默认为<see cref="ArrayPool{T}.Shared"/></param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    /// <returns>密文</returns>
    [Pure, MustDisposeResource]
    public static PooledArray<byte> BlockEncryptPkcs1(this RSA rsa, ReadOnlySpan<byte> plaintext,
        ArrayPool<byte>? pool = null, bool clearArray = false)
    {
        if (plaintext.Length <= 0) return PooledArray<byte>.Empty;

        var ciphertextLength = rsa.GetPkcs1CiphertextLength(plaintext.Length);
        pool ??= ArrayPool<byte>.Shared;
        var ciphertext = PooledArray.Get<byte>(ciphertextLength, pool, clearArray);
        rsa.BlockEncryptPkcs1Internal(plaintext, ciphertext.Span);
        return ciphertext;
    }

    /// <summary>
    /// 分组加密，使用Pkcs1进行填充
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="plaintext">明文</param>
    /// <param name="destination">输出跨度</param>
    /// <returns>输出长度</returns>
    public static int BlockEncryptPkcs1(this RSA rsa, ReadOnlySpan<byte> plaintext, Span<byte> destination)
    {
        if (plaintext.Length <= 0) return 0;

        var ciphertextLength = rsa.GetPkcs1CiphertextLength(plaintext.Length);
        ArgumentOutOfRangeHelper.ThrowIfLessThan(destination.Length, ciphertextLength);
        return BlockEncryptPkcs1Internal(rsa, plaintext, destination);
    }

    /// <summary>分组加密，使用Pkcs1进行填充，内部实现</summary>
    /// <remarks>调用前须确保输出跨度长度足够</remarks>
    private static int BlockEncryptPkcs1Internal(this RSA rsa, ReadOnlySpan<byte> plaintext, Span<byte> destination)
    {
        // 分组明文长度
        var blockLength = rsa.GetPkcs1InputMaxLength();
        // 明文分组数量
        var numOfBlocks = (plaintext.Length + blockLength - 1) / blockLength;
        var written = 0;
#if !NET7_0_OR_GREATER
        var buffer = Array.Empty<byte>();
#endif
        for (var i = 0; i < numOfBlocks; i++)
        {
            var thisBlockStart = i * blockLength;
            var thisBlockLength = Math.Min(plaintext.Length - thisBlockStart, blockLength);
            var thisBlock = plaintext.Slice(thisBlockStart, thisBlockLength);
#if NET7_0_OR_GREATER
            var thisWritten = rsa.Encrypt(thisBlock, destination[written..], RSAEncryptionPadding.Pkcs1);
            written += thisWritten;
#else
            if (thisBlock.Length != buffer.Length)
            {
                buffer = ArrayHelper.AllocateUninitializedArray<byte>(thisBlock.Length);
            }

            thisBlock.CopyTo(buffer);
            var encrypted = rsa.Encrypt(buffer, RSAEncryptionPadding.Pkcs1);
            encrypted.CopyTo(destination[written..]);
            written += encrypted.Length;
#endif
        }

        return written;
    }

    /// <summary>
    /// 分组解密，使用Pkcs1进行填充
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="ciphertext">密文</param>
    /// <param name="pool">数组池，默认为<see cref="ArrayPool{T}.Shared"/></param>
    /// <param name="clearArray">是否归还时清空数组，默认false</param>
    /// <returns>明文</returns>
    [Pure, MustDisposeResource]
    public static PooledArray<byte> BlockDecryptPkcs1(this RSA rsa, ReadOnlySpan<byte> ciphertext,
        ArrayPool<byte>? pool = null, bool clearArray = false)
    {
        if (ciphertext.Length <= 0) return PooledArray<byte>.Empty;

        pool ??= ArrayPool<byte>.Shared;
        // 明文可能的最大长度
        var plainTextMaxLength = rsa.GetPkcs1PlainTextMaxLength(ciphertext.Length);
        var destination = pool.Rent(plainTextMaxLength);
        try
        {
            // 写入长度
            var written = rsa.BlockDecryptPkcs1(ciphertext, destination);
            return new PooledArray<byte>(destination, written, pool, clearArray);
        }
        catch
        {
            // 异常回收数组
            pool.Return(destination, clearArray);
            throw;
        }
    }

    /// <summary>
    /// 分组解密，使用Pkcs1进行填充
    /// </summary>
    /// <param name="rsa">RSA实例</param>
    /// <param name="ciphertext">密文</param>
    /// <param name="destination">输出跨度</param>
    /// <returns></returns>
    public static int BlockDecryptPkcs1(this RSA rsa, ReadOnlySpan<byte> ciphertext, Span<byte> destination)
    {
        if (ciphertext.Length <= 0) return 0;

        // 分组密文长度
        var blockLength = rsa.GetPkcs1OutputBlockLength();
        // 密文分组数量
        var numOfBlocks = (ciphertext.Length + blockLength - 1) / blockLength;
        var written = 0;
#if !NET7_0_OR_GREATER
        var buffer = Array.Empty<byte>();
#endif
        for (var i = 0; i < numOfBlocks; i++)
        {
            var thisBlockStart = i * blockLength;
            var thisBlockLength = Math.Min(ciphertext.Length - thisBlockStart, blockLength);
            var thisBlock = ciphertext.Slice(thisBlockStart, thisBlockLength);
#if NET7_0_OR_GREATER
            var thisWritten = rsa.Decrypt(thisBlock, destination[written..], RSAEncryptionPadding.Pkcs1);
            written += thisWritten;
#else
            if (thisBlock.Length != buffer.Length)
            {
                buffer = ArrayHelper.AllocateUninitializedArray<byte>(thisBlock.Length);
            }

            thisBlock.CopyTo(buffer);
            var decrypted = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);
            decrypted.CopyTo(destination[written..]);
            written += decrypted.Length;
#endif
        }

        return written;
    }
}