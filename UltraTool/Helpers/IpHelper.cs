using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if !NET6_0_OR_GREATER
using UltraTool.Collections;
#endif

namespace UltraTool.Helpers;

/// <summary>
/// Ip地址帮助类
/// </summary>
[PublicAPI]
public static class IpHelper
{
    /// <summary>
    /// 判断是否为私有Ipv4地址
    /// </summary>
    /// <param name="ip">Ip地址</param>
    /// <returns>是否为私有Ipv4地址</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrivateIpv4(IPAddress ip) => ip.GetAddressBytes() switch
    {
        var bytes when bytes[0] == 10 => true,
        var bytes when bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31 => true,
        var bytes when bytes[0] == 192 && bytes[1] == 168 => true,
        _ => false
    };

    /// <summary>
    /// 判断是否为私有Ipv4地址
    /// </summary>
    /// <param name="ip">Ip地址</param>
    /// <returns>是否为私有Ipv4地址</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrivateIpv4(string ip) => IsPrivateIpv4(IPAddress.Parse(ip));

    /// <summary>
    /// 获取本机Ipv4地址序列
    /// </summary>
    /// <returns>本机Ipv4地址序列</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<IPAddress> GetLocalIpv4() =>
        Dns.GetHostAddresses(Dns.GetHostName()).Where(static ip => ip.AddressFamily == AddressFamily.InterNetwork);

    /// <summary>
    /// 获取本机Ipv4地址字符串序列
    /// </summary>
    /// <returns>本机Ipv4地址字符串序列</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<string> GetLocalIpv4String() =>
        GetLocalIpv4().Select(static ip => ip.IsIPv4MappedToIPv6 ? ip.MapToIPv4().ToString() : ip.ToString());

    /// <summary>
    /// 第一个本机Ipv4地址
    /// </summary>
    /// <returns>第一个本机Ipv4地址</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IPAddress? FirstLocalIpv4() => GetLocalIpv4().FirstOrDefault();

    /// <summary>
    /// 第一个本机Ipv4地址字符串
    /// </summary>
    /// <returns>第一个本机Ipv4地址字符串</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? FirstLocalIpv4String() => GetLocalIpv4String().FirstOrDefault();

    /// <summary>
    /// 第一个本机Ipv4地址
    /// </summary>
    /// <param name="defaultValue">缺省值</param>
    /// <returns>第一个本机Ipv4地址</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FirstLocalIpv4String(string defaultValue) => GetLocalIpv4String().FirstOrDefault(defaultValue);

    /// <summary>
    /// 判断端口是否被占用
    /// </summary>
    /// <param name="port">端口</param>
    /// <returns>是否被占用</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUsedPort(int port) => IsUsedTcpPort(port) || IsUsedUdpPort(port);

    /// <summary>
    /// 判断端口是否被Tcp占用
    /// </summary>
    /// <param name="port">端口</param>
    /// <returns>是否被占用</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUsedTcpPort(int port) => IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
        .Select(static point => point.Port).Contains(port);

    /// <summary>
    /// 判断端口是否被Udp占用
    /// </summary>
    /// <param name="port">端口</param>
    /// <returns>是否被占用</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUsedUdpPort(int port) => IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
        .Select(static point => point.Port).Contains(port);
}