using System.Security.Cryptography;
using JetBrains.Annotations;

namespace UltraTool.Cryptography;

/// <summary>
/// 加密帮助类
/// </summary>
[PublicAPI]
public static class CryptoHelper
{
    /// <summary>
    /// AES加密，ECB模式
    /// </summary>
    /// <param name="plaintext">明文</param>
    /// <param name="key">密钥</param>
    /// <param name="padding">填充模式，默认为PKCS7</param>
    /// <returns>密文</returns>
    public static byte[] AESEncryptEcb(ReadOnlySpan<byte> plaintext, byte[] key,
        PaddingMode padding = PaddingMode.PKCS7)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Key = key;
        aes.Padding = padding;
#if NET6_0_OR_GREATER
        return aes.EncryptEcb(plaintext, padding);
#else
        using var encryptor = aes.CreateEncryptor();
        using var stream = new MemoryStream();
        using var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plaintext);
        cryptoStream.FlushFinalBlock();
        return stream.ToArray();
#endif
    }

    /// <summary>
    /// AES加密，CBC模式
    /// </summary>
    /// <param name="plaintext">明文</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">初始向量</param>
    /// <param name="padding">填充模式，默认为PKCS7</param>
    /// <returns>密文</returns>
    public static byte[] AESEncryptCbc(ReadOnlySpan<byte> plaintext, byte[] key, byte[] iv,
        PaddingMode padding = PaddingMode.PKCS7)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.IV = iv;
        aes.Padding = padding;
#if NET6_0_OR_GREATER
        return aes.EncryptCbc(plaintext, iv, padding);
#else
        using var transform = aes.CreateEncryptor();
        using var stream = new MemoryStream();
        using var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
        cryptoStream.Write(plaintext);
        cryptoStream.FlushFinalBlock();
        return stream.ToArray();
#endif
    }

    /// <summary>
    /// AES解密，ECB模式
    /// </summary>
    /// <param name="ciphertext">密文</param>
    /// <param name="key">密钥</param>
    /// <param name="padding">填充模式，默认为PKCS7</param>
    /// <returns>明文</returns>
    public static byte[] AESDecryptEcb(ReadOnlySpan<byte> ciphertext, byte[] key,
        PaddingMode padding = PaddingMode.PKCS7)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Key = key;
        aes.Padding = padding;
#if NET6_0_OR_GREATER
        return aes.DecryptEcb(ciphertext, padding);
#else
        using var transform = aes.CreateDecryptor();
        using var stream = new MemoryStream();
        using var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
        cryptoStream.Write(ciphertext);
        cryptoStream.FlushFinalBlock();
        return stream.ToArray();
#endif
    }

    /// <summary>
    /// AES解密，CBC模式
    /// </summary>
    /// <param name="ciphertext">密文</param>
    /// <param name="key">密钥</param>
    /// <param name="iv">初始向量</param>
    /// <param name="padding">填充模式，默认为PKCS7</param>
    /// <returns>明文</returns>
    public static byte[] AESDecryptCbc(ReadOnlySpan<byte> ciphertext, byte[] key, byte[] iv,
        PaddingMode padding = PaddingMode.PKCS7)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Key = key;
        aes.IV = iv;
        aes.Padding = padding;
#if NET6_0_OR_GREATER
        return aes.DecryptCbc(ciphertext, iv, padding);
#else
        using var transform = aes.CreateDecryptor();
        using var stream = new MemoryStream();
        using var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);
        cryptoStream.Write(ciphertext);
        cryptoStream.FlushFinalBlock();
        return stream.ToArray();
#endif
    }
}