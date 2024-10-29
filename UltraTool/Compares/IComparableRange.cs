using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Ultra.Common.Compares;

namespace UltraTool.Compares;

/// <summary>
/// 可比较区间接口
/// </summary>
[PublicAPI]
public interface IComparableRange<T> : IRange<T> where T : IComparable<T>
{
    /// <summary>
    /// 判断指定值是否在范围内
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>是否在范围内</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsIn(T value) => CompareHelper.InRangeInternal(value, Start, End, Mode);

    /// <summary>
    /// 判断指定值是否在范围外
    /// </summary>
    /// <param name="value">值</param>
    /// <returns>是否在范围外</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsOut(T value) => CompareHelper.OutRangeInternal(value, Start, End, Mode);
}