using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 比较帮助类
/// </summary>
[PublicAPI]
public static class CompareHelper
{
    /// <summary>
    /// 返回两个值中的最小值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最小值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Min<T>(in T value1, in T value2) where T : IComparable<T> =>
        value1.CompareTo(value2) < 0 ? value1 : value2;

    /// <summary>
    /// 返回两个值中的最大值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <returns>最大值</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Max<T>(in T value1, in T value2) where T : IComparable<T> =>
        value1.CompareTo(value2) > 0 ? value1 : value2;

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <returns>中间值</returns>
    [Pure]
    public static T Middle<T>(in T value1, in T value2, in T value3) where T : IComparable<T>
    {
        if (value1.CompareTo(value2) < 0)
        {
            if (value3.CompareTo(value2) > 0) return value2;

            return value3.CompareTo(value1) < 0 ? value1 : value3;
        }

        if (value3.CompareTo(value1) > 0) return value1;

        return value3.CompareTo(value2) < 0 ? value2 : value3;
    }

    /// <summary>
    /// 返回三个值中的中间值
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="value3">值3</param>
    /// <param name="comparer">比较器</param>
    /// <returns>中间值</returns>
    [Pure]
    public static T Middle<T>(in T value1, in T value2, in T value3, IComparer<T> comparer)
    {
        if (comparer.Compare(value1, value2) < 0)
        {
            if (comparer.Compare(value3, value2) > 0) return value2;

            return comparer.Compare(value3, value1) < 0 ? value1 : value3;
        }

        if (comparer.Compare(value3, value1) > 0) return value1;

        return comparer.Compare(value3, value2) < 0 ? value2 : value3;
    }
}