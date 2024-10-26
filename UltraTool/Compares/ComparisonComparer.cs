namespace UltraTool.Compares;

/// <summary>
/// 比较表达式比较器
/// </summary>
/// <param name="comparison">比较表达式</param>
public sealed class ComparisonComparer<T>(Comparison<T> comparison) : IComparer<T>
{
    /// <inheritdoc />
    public int Compare(T? x, T? y)
    {
        if (x == null && y == null) return 0;

        if (x == null) return -1;

        return y == null ? 1 : comparison(x, y);
    }
}