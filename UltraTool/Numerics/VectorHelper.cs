using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UltraTool.Compares;
using UltraTool.Helpers;

namespace UltraTool.Numerics;

/// <summary>
/// 向量帮助类
/// </summary>
[PublicAPI]
public static class VectorHelper
{
    #region 范围判断

    /// <summary>
    /// 判断指定点是否在方形内
    /// </summary>
    /// <param name="center">方形中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在方形内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInSquare(in Vector2 center, [NonNegativeValue] float size, in Vector2 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        return point.X >= center.X - size / 2 && point.X <= center.X + size &&
               point.Y >= center.Y - size / 2 && point.Y <= center.Y + size;
    }

    /// <summary>
    /// 判断指定点是否在矩形内
    /// </summary>
    /// <param name="center">矩形中心点</param>
    /// <param name="size">矩形尺寸(X边长,Y边长)</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在矩形内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRectangle(in Vector2 center, in Vector2 size, in Vector2 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        return point.X >= center.X - size.X / 2 && point.X <= center.X + size.X &&
               point.Y >= center.Y - size.Y / 2 && point.Y <= center.Y + size.Y;
    }

    /// <summary>
    /// 判断指定点是否在圆内
    /// </summary>
    /// <param name="center">圆中心点</param>
    /// <param name="radius">圆半径</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在圆内</returns>
    [Pure]
    public static bool IsInCircle(in Vector2 center, [NonNegativeValue] float radius, in Vector2 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var dx = (double)point.X - center.X;
        var dy = (double)point.Y - center.Y;
        var dist = (double)radius;
        return dx * dx + dy * dy <= dist * dist;
    }

    /// <summary>
    /// 判断指定点是否在立方体内
    /// </summary>
    /// <param name="center">立方体中心点</param>
    /// <param name="size">边长尺寸</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在立方体内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInCube(in Vector3 center, [NonNegativeValue] float size, in Vector3 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size);
        return point.X >= center.X - size / 2 && point.X <= center.X + size &&
               point.Y >= center.Y - size / 2 && point.Y <= center.Y + size &&
               point.Z >= center.Z - size / 2 && point.Z <= center.Z + size;
    }

    /// <summary>
    /// 判断一个点是否在矩形体内
    /// </summary>
    /// <param name="center">矩形体中心点</param>
    /// <param name="size">矩形体尺寸(X边长,Y边长,Z边长)</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在矩形体内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInCuboid(in Vector3 center, in Vector3 size, in Vector3 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.X);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Y);
        ArgumentOutOfRangeHelper.ThrowIfNegative(size.Z);
        return point.X >= center.X - size.X / 2 && point.X <= center.X + size.X &&
               point.Y >= center.Y - size.Y / 2 && point.Y <= center.Y + size.Y &&
               point.Z >= center.Z - size.Z / 2 && point.Z <= center.Z + size.Z;
    }

    /// <summary>
    /// 判断指定点是否在球内
    /// </summary>
    /// <param name="center">球中心点</param>
    /// <param name="radius">球半径</param>
    /// <param name="point">待判断点</param>
    /// <returns>是否在球内</returns>
    [Pure]
    public static bool IsInSphere(in Vector3 center, [NonNegativeValue] float radius, Vector3 point)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(radius);
        var dx = (double)point.X - center.X;
        var dy = (double)point.Y - center.Y;
        var dz = (double)point.Z - center.Z;
        var dist = (double)radius;
        return dx * dx + dy * dy + dz * dz <= dist * dist;
    }

    #endregion

    #region 距离判断

    /// <summary>
    /// 判断两点是否小于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否小于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessDistance(in Vector2 pointA, in Vector2 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.Less);

    /// <summary>
    /// 判断两点是否小于等于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否小于等于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessEqualsDistance(in Vector2 pointA, in Vector2 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.LessEquals);

    /// <summary>
    /// 判断两点是否大于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否大于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterDistance(in Vector2 pointA, in Vector2 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.Greater);

    /// <summary>
    /// 判断两点是否大于等于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否大于等于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterEqualsDistance(in Vector2 pointA, in Vector2 pointB, [NonNegativeValue] float distance)
        => CompareDistance(pointA, pointB, distance, CompareSymbol.GreaterEquals);

    /// <summary>
    /// 计算两点距离与指定距离进行比较
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <param name="symbol">比较符号</param>
    /// <returns>是否符合比较规则</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareDistance(in Vector2 pointA, in Vector2 pointB, [NonNegativeValue] float distance,
        CompareSymbol symbol)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(distance);
        return DoubleCompare(Vector2.DistanceSquared(pointA, pointB), (double)distance * distance, symbol);
    }

    /// <summary>
    /// 判断两点是否小于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否小于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessDistance(in Vector3 pointA, in Vector3 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.Less);

    /// <summary>
    /// 判断两点是否小于等于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否小于等于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLessEqualsDistance(in Vector3 pointA, in Vector3 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.LessEquals);

    /// <summary>
    /// 判断两点是否大于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否大于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterDistance(in Vector3 pointA, in Vector3 pointB, [NonNegativeValue] float distance) =>
        CompareDistance(pointA, pointB, distance, CompareSymbol.Greater);

    /// <summary>
    /// 判断两点是否大于等于指定距离
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <returns>是否大于等于指定距离</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsGreaterEqualsDistance(in Vector3 pointA, in Vector3 pointB, [NonNegativeValue] float distance)
        => CompareDistance(pointA, pointB, distance, CompareSymbol.GreaterEquals);

    /// <summary>
    /// 计算两点距离与指定距离进行比较
    /// </summary>
    /// <param name="pointA">点A</param>
    /// <param name="pointB">点B</param>
    /// <param name="distance">距离</param>
    /// <param name="symbol">比较符号</param>
    /// <returns>是否符合比较规则</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CompareDistance(in Vector3 pointA, in Vector3 pointB, [NonNegativeValue] float distance,
        CompareSymbol symbol)
    {
        ArgumentOutOfRangeHelper.ThrowIfNegative(distance);
        return DoubleCompare(Vector3.DistanceSquared(pointA, pointB), (double)distance * distance, symbol);
    }

    #endregion

    #region 旋转操作

    /// <summary>
    /// 计算坐标以(0,0)为原点逆时针旋转后的位置
    /// </summary>
    /// <param name="vector">坐标</param>
    /// <param name="radian">弧度</param>
    /// <returns>旋转后坐标</returns>
    [Pure]
    public static Vector2 Rotate(in Vector2 vector, double radian)
    {
        // 计算新的向量坐标
        var newX = (float)(vector.X * Math.Cos(radian) - vector.Y * Math.Sin(radian));
        var newY = (float)(vector.X * Math.Sin(radian) + vector.Y * Math.Cos(radian));
        return new Vector2(newX, newY);
    }

    /// <summary>
    /// 计算坐标以指定坐标为原点逆时针旋转后的位置
    /// </summary>
    /// <param name="vector">坐标</param>
    /// <param name="origin">原点坐标</param>
    /// <param name="radian">弧度</param>
    /// <returns>旋转后坐标</returns>
    [Pure]
    public static Vector2 Rotate(in Vector2 vector, in Vector2 origin, double radian)
    {
        // 将原始向量相对于原点平移
        var offset = vector - origin;
        // 计算新的向量坐标
        var newX = (float)(offset.X * Math.Cos(radian) - offset.Y * Math.Sin(radian));
        var newY = (float)(offset.X * Math.Sin(radian) + offset.Y * Math.Cos(radian));
        // 将结果向量平移回原始位置
        return new Vector2(newX, newY) + origin;
    }

    /// <summary>
    /// 计算坐标以(0,0)为原点顺时针旋转后的位置
    /// </summary>
    /// <param name="vector">坐标</param>
    /// <param name="radian">弧度</param>
    /// <returns>旋转后坐标</returns>
    [Pure]
    public static Vector2 RotateClockwise(in Vector2 vector, double radian)
    {
        // 计算新的向量坐标
        var newX = (float)(vector.X * Math.Cos(radian) + vector.Y * Math.Sin(radian));
        var newY = (float)(-vector.X * Math.Sin(radian) + vector.Y * Math.Cos(radian));
        return new Vector2(newX, newY);
    }

    /// <summary>
    /// 计算坐标以指定坐标为原点顺时针旋转后的位置
    /// </summary>
    /// <param name="vector">坐标</param>
    /// <param name="origin">原点坐标</param>
    /// <param name="radian">弧度</param>
    /// <returns>旋转后坐标</returns>
    [Pure]
    public static Vector2 RotateClockwise(in Vector2 vector, in Vector2 origin, double radian)
    {
        // 将原始向量相对于原点平移
        var offset = vector - origin;
        // 计算新的向量坐标
        var newX = (float)(offset.X * Math.Cos(radian) + offset.Y * Math.Sin(radian));
        var newY = (float)(-offset.X * Math.Sin(radian) + offset.Y * Math.Cos(radian));
        // 将结果向量平移回原始位置
        return new Vector2(newX, newY) + origin;
    }

    #endregion

    /// <summary>对两个双精度浮点值进行比较</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool DoubleCompare(double value1, double value2, CompareSymbol symbol) => symbol switch
    {
        CompareSymbol.Less => value1.CompareTo(value2) < 0,
        CompareSymbol.Greater => value1.CompareTo(value2) > 0,
        CompareSymbol.Equals => value1.CompareTo(value2) == 0,
        CompareSymbol.LessEquals => value1.CompareTo(value2) <= 0,
        CompareSymbol.GreaterEquals => value1.CompareTo(value2) >= 0,
        CompareSymbol.NotEquals => value1.CompareTo(value2) != 0,
        _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "Undefined comparison symbols")
    };
}