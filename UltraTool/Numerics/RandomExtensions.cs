using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Helpers;
#if !NET6_0_OR_GREATER
using UltraTool.Randoms;
#endif

namespace UltraTool.Numerics;

/// <summary>
/// 随机拓展类
/// </summary>
[PublicAPI]
public static class RandomExtensions
{
    /// <summary>
    /// 在方形内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">方形中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y) NextInSquare(this Random random, in (double X, double Y) center,
        [NonNegativeValue] double size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        var pointX = center.X - size / 2 + size * random.NextDouble();
        var pointY = center.Y - size / 2 + size * random.NextDouble();
        return (pointX, pointY);
    }

    /// <summary>
    /// 在矩形内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">矩形中心点</param>
    /// <param name="size">矩形尺寸(X边长,Y边长)</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y) NextInRectangle(this Random random, in (double X, double Y) center,
        in (double X, double Y) size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        var pointX = center.X - size.X / 2 + size.X * random.NextDouble();
        var pointY = center.Y - size.Y / 2 + size.Y * random.NextDouble();
        return (pointX, pointY);
    }

    /// <summary>
    /// 在圆内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">圆中心点</param>
    /// <param name="radius">圆半径</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y) NextInCircle(this Random random, in (double X, double Y) center,
        [NonNegativeValue] double radius)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var u = random.NextDouble();
        var theta = random.NextDouble() * 2 * Math.PI;
        var r = Math.Sqrt(u);
        var pointX = center.X + r * Math.Cos(theta) * radius;
        var pointY = center.Y + r * Math.Sin(theta) * radius;
        return (pointX, pointY);
    }

    /// <summary>
    /// 在方形内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">方形中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector2 NextInSquare(this Random random, in Vector2 center, [NonNegativeValue] float size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        var pointX = center.X - size / 2 + size * random.NextSingle();
        var pointY = center.Y - size / 2 + size * random.NextSingle();
        return new Vector2(pointX, pointY);
    }

    /// <summary>
    /// 在矩形内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">矩形中心点</param>
    /// <param name="size">矩形尺寸(X边长,Y边长)</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector2 NextInRectangle(this Random random, in Vector2 center, in Vector2 size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        var pointX = center.X - size.X / 2 + size.X * random.NextSingle();
        var pointY = center.Y - size.Y / 2 + size.Y * random.NextSingle();
        return new Vector2(pointX, pointY);
    }

    /// <summary>
    /// 在圆内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">圆中心点</param>
    /// <param name="radius">圆半径</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector2 NextInCircle(this Random random, in Vector2 center, [NonNegativeValue] float radius)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var u = random.NextDouble();
        var theta = random.NextDouble() * 2 * Math.PI;
        var r = Math.Sqrt(u);
        var pointX = (float)(center.X + r * Math.Cos(theta) * radius);
        var pointY = (float)(center.Y + r * Math.Sin(theta) * radius);
        return new Vector2(pointX, pointY);
    }

    /// <summary>
    /// 在立方体内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">立方体中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y, double Z) NextInCube(this Random random,
        in (double X, double Y, double Z) center, [NonNegativeValue] double size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        var pointX = center.X - size / 2 + size * random.NextDouble();
        var pointY = center.Y - size / 2 + size * random.NextDouble();
        var pointZ = center.Z - size / 2 + size * random.NextDouble();
        return (pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在矩形体内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">矩形体中心点</param>
    /// <param name="size">矩形体尺寸(X边长,Y边长,Z边长)</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y, double Z) NextInCuboid(this Random random,
        in (double X, double Y, double Z) center, in (double X, double Y, double Z) size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Z);
        var pointX = center.X - size.X / 2 + size.X * random.NextDouble();
        var pointY = center.Y - size.Y / 2 + size.Y * random.NextDouble();
        var pointZ = center.Z - size.Z / 2 + size.Z * random.NextDouble();
        return (pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在球内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">球中心点</param>
    /// <param name="radius">球半径</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y, double Z) NextInSphere(this Random random,
        in (double X, double Y, double Z) center, [NonNegativeValue] double radius)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var u = random.NextDouble();
        var v = random.NextDouble();
        var theta = 2 * Math.PI * u;
        var phi = Math.Acos(2 * v - 1);
        var r = radius * Math.Pow(random.NextDouble(), 1.0 / 3.0);
        var pointX = center.X + r * Math.Sin(phi) * Math.Cos(theta);
        var pointY = center.Y + r * Math.Sin(phi) * Math.Sin(theta);
        var pointZ = center.Z + r * Math.Cos(phi);
        return (pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在立方体内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">立方体中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector3 NextInCube(this Random random, in Vector3 center, [NonNegativeValue] float size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        var pointX = center.X - size / 2 + size * random.NextSingle();
        var pointY = center.Y - size / 2 + size * random.NextSingle();
        var pointZ = center.Z - size / 2 + size * random.NextSingle();
        return new Vector3(pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在矩形体内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">矩形体中心点</param>
    /// <param name="size">矩形体尺寸(X边长,Y边长,Z边长)</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector3 NextInCuboid(this Random random, in Vector3 center, in Vector3 size)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Z);
        var pointX = center.X - size.X / 2 + size.X * random.NextSingle();
        var pointY = center.Y - size.Y / 2 + size.Y * random.NextSingle();
        var pointZ = center.Z - size.Z / 2 + size.Z * random.NextSingle();
        return new Vector3(pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在球内随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="center">球中心点</param>
    /// <param name="radius">球半径</param>
    /// <returns>随机点</returns>
    [Pure]
    public static Vector3 NextInSphere(this Random random, in Vector3 center, [NonNegativeValue] float radius)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var u = random.NextDouble();
        var v = random.NextDouble();
        var theta = 2 * Math.PI * u;
        var phi = Math.Acos(2 * v - 1);
        var r = radius * Math.Pow(random.NextDouble(), 1.0 / 3.0);
        var pointX = (float)(center.X + r * Math.Sin(phi) * Math.Cos(theta));
        var pointY = (float)(center.Y + r * Math.Sin(phi) * Math.Sin(theta));
        var pointZ = (float)(center.Z + r * Math.Cos(phi));
        return new Vector3(pointX, pointY, pointZ);
    }

    /// <summary>
    /// 在线段上随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="endpointA">端点A</param>
    /// <param name="endpointB">端点B</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y) NextOnLineSegment(this Random random, in (double X, double Y) endpointA,
        in (double X, double Y) endpointB)
    {
        var x = endpointA.X + random.NextDouble() * (endpointB.X - endpointA.X);
        var y = endpointA.Y + random.NextDouble() * (endpointB.Y - endpointA.Y);
        return (x, y);
    }

    /// <summary>
    /// 在线段上随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="endpointA">端点A</param>
    /// <param name="endpointB">端点B</param>
    /// <returns>随机点</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 NextOnLineSegment(this Random random, in Vector2 endpointA, in Vector2 endpointB) =>
        endpointA + (endpointB - endpointA) * random.NextSingle();

    /// <summary>
    /// 在线段上随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="endpointA">端点A</param>
    /// <param name="endpointB">端点B</param>
    /// <returns>随机点</returns>
    [Pure]
    public static (double X, double Y, double Z) NextOnLineSegment(this Random random,
        in (double X, double Y, double Z) endpointA, in (double X, double Y, double Z) endpointB)
    {
        var x = endpointA.X + random.NextDouble() * (endpointB.X - endpointA.X);
        var y = endpointA.Y + random.NextDouble() * (endpointB.Y - endpointA.Y);
        var z = endpointA.Z + random.NextDouble() * (endpointB.Z - endpointA.Z);
        return (x, y, z);
    }

    /// <summary>
    /// 在线段上随机生成一点
    /// </summary>
    /// <param name="random">随机对象</param>
    /// <param name="endpointA">端点A</param>
    /// <param name="endpointB">端点B</param>
    /// <returns>随机点</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 NextOnLineSegment(this Random random, in Vector3 endpointA, in Vector3 endpointB) =>
        endpointA + (endpointB - endpointA) * random.NextSingle();
}