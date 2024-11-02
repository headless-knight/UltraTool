using System.Security.Cryptography;
using UltraTool.Cryptography;

namespace UltraTool.Tests;

public class CryptoTest
{
    [Fact]
    public void TestAES()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);
        var iv = new byte[16];
        RandomNumberGenerator.Fill(iv);
        var plaintext = new byte[128];
        RandomNumberGenerator.Fill(plaintext);
        var encryptedCbc = CryptoHelper.AESEncryptCbc(plaintext, key, iv);
        var decryptedCbc = CryptoHelper.AESDecryptCbc(encryptedCbc, key, iv);
        Assert.Equal(plaintext, decryptedCbc);
        var encryptedEcb = CryptoHelper.AESEncryptEcb(plaintext, key);
        var decryptedEcb = CryptoHelper.AESDecryptEcb(encryptedEcb, key);
        Assert.Equal(plaintext, decryptedEcb);
    }

    [Fact]
    public void TestRSA()
    {
        var rsa = RSA.Create();
        rsa.KeySize = 2048;
        var plaintext = new byte[300];
        RandomNumberGenerator.Fill(plaintext);
        using var encrypted = rsa.BlockEncryptPkcs1(plaintext);
        using var decrypted = rsa.BlockDecryptPkcs1(encrypted.ReadOnlySpan);
        Assert.True(decrypted.ReadOnlySpan.SequenceEqual(plaintext));
    }
}