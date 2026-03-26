using System.Net;
using UltraTool.Helpers;

namespace UltraTool.Tests.Helpers;

/// <summary>
/// IpHelper 单元测试
/// </summary>
public class IpHelperTests
{
    #region IsPrivateIpv4 测试

    [Theory]
    // 10.x.x.x 私有地址
    [InlineData("10.0.0.1", true)]
    [InlineData("10.255.255.255", true)]
    [InlineData("10.100.50.25", true)]
    // 172.16-31.x.x 私有地址
    [InlineData("172.16.0.1", true)]
    [InlineData("172.31.255.255", true)]
    [InlineData("172.20.10.5", true)]
    // 172 非私有范围
    [InlineData("172.15.0.1", false)]
    [InlineData("172.32.0.1", false)]
    // 192.168.x.x 私有地址
    [InlineData("192.168.0.1", true)]
    [InlineData("192.168.255.255", true)]
    [InlineData("192.168.1.100", true)]
    // 公网地址
    [InlineData("8.8.8.8", false)]
    [InlineData("1.1.1.1", false)]
    [InlineData("192.169.0.1", false)]
    [InlineData("11.0.0.1", false)]
    [InlineData("172.15.255.255", false)]
    public void IsPrivateIpv4_String_ReturnsCorrectResult(string ip, bool expected)
    {
        Assert.Equal(expected, IpHelper.IsPrivateIpv4(ip));
    }

    [Fact]
    public void IsPrivateIpv4_IPAddressObject_PrivateAddress_ReturnsTrue()
    {
        Assert.True(IpHelper.IsPrivateIpv4(IPAddress.Parse("10.0.0.1")));
        Assert.True(IpHelper.IsPrivateIpv4(IPAddress.Parse("172.16.0.1")));
        Assert.True(IpHelper.IsPrivateIpv4(IPAddress.Parse("192.168.1.1")));
    }

    [Fact]
    public void IsPrivateIpv4_IPAddressObject_PublicAddress_ReturnsFalse()
    {
        Assert.False(IpHelper.IsPrivateIpv4(IPAddress.Parse("8.8.8.8")));
        Assert.False(IpHelper.IsPrivateIpv4(IPAddress.Parse("172.32.0.1")));
    }

    #endregion

    #region GetLocalIpv4 测试

    [Fact]
    public void GetLocalIpv4_ReturnsNonNullSequence()
    {
        var ips = IpHelper.GetLocalIpv4().ToList();
        Assert.NotNull(ips);
    }

    [Fact]
    public void GetLocalIpv4String_ReturnsNonNullSequence()
    {
        var ips = IpHelper.GetLocalIpv4String().ToList();
        Assert.NotNull(ips);
    }

    #endregion

    #region IsUsedPort 测试

    [Fact]
    public void IsUsedTcpPort_ReturnsBoolValue()
    {
        var result = IpHelper.IsUsedTcpPort(0);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void IsUsedUdpPort_ReturnsBoolValue()
    {
        var result = IpHelper.IsUsedUdpPort(0);
        Assert.IsType<bool>(result);
    }

    #endregion
}
