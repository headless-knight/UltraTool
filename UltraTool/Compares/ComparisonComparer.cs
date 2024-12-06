using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 比较表达式比较器
/// </summary>
/// <param name="comparison">比较表达式</param>
[PublicAPI]
public class ComparisonComparer<T>(Comparison<T> comparison) : IComparer<T>
{
    /// <summary>
    /// 比较表达式
    /// </summary>
    public Comparison<T> Comparison => comparison;

    /// <inheritdoc />
    public int Compare(T? x, T? y)
    {
        if (x == null && y == null) return 0;

        if (x == null) return -1;

        return y == null ? 1 : comparison.Invoke(x, y);
    }
}