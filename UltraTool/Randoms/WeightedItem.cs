using JetBrains.Annotations;

namespace UltraTool.Randoms;

/// <summary>
/// 带权元素
/// </summary>
/// <param name="item">元素</param>
/// <param name="weight">权重</param>
[PublicAPI]
public class WeightedItem<T>(T item, int weight) : IWeighted
{
    /// <summary>
    /// 元素
    /// </summary>
    public T Item { get; } = item;

    /// <inheritdoc />
    public int Weight { get; } = weight;
}