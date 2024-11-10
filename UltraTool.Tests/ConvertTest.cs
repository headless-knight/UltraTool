using System.Security.Cryptography;
using UltraTool.Helpers;
using Xunit.Abstractions;

namespace UltraTool.Tests;

public class ConvertTest(ITestOutputHelper output)
{
    [Fact]
    public void ToHexTest()
    {
        var array = new byte[16];
        RandomNumberGenerator.Fill(array);
        var lowerHex = ConvertHelper.ToHexString(array, true);
        output.WriteLine(lowerHex);
        var upperHex = ConvertHelper.ToHexString(array);
        output.WriteLine(upperHex);
        Assert.Equal(32, lowerHex.Length);
        Assert.Equal(32, upperHex.Length);
        Assert.Equal(lowerHex.ToUpper(), upperHex);
        Assert.Equal(Convert.ToHexString(array), upperHex);
    }

    [Fact]
    public void FromHexTest()
    {
        var array = new byte[16];
        RandomNumberGenerator.Fill(array);
        var upperHex = Convert.ToHexString(array);
        var bytes = ConvertHelper.FromHexString(upperHex);
        Assert.True(array.SequenceEqual(bytes));
        var lowerHex = upperHex.ToLower();
        bytes = ConvertHelper.FromHexString(lowerHex);
        Assert.True(array.SequenceEqual(bytes));
    }

    [Fact]
    public void ToBase62Test()
    {
        var num = Random.Shared.NextInt64();
        output.WriteLine(num.ToString());
        var array = BitConverter.GetBytes(num);
        RandomNumberGenerator.Fill(array);
        var base62 = ConvertHelper.ToBase62String(array);
        output.WriteLine(base62);
    }
}