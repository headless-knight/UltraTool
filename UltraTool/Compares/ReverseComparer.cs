using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 反转比较器
/// </summary>
[PublicAPI]
public class ReverseComparer<T> : IComparer<T>
{
    /// <summary>
    /// 原始比较器
    /// </summary>
    public IComparer<T> RawComparer { get; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="comparer">比较器</param>
    public ReverseComparer(IComparer<T> comparer)
    {
        RawComparer = comparer;
    }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="comparison">比较表达式</param>
    public ReverseComparer(Comparison<T> comparison) : this(new ComparisonComparer<T>(comparison))
    {
    }

    /// <inheritdoc />
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(T? x, T? y) => -RawComparer.Compare(y, x);
}