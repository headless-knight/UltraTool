using JetBrains.Annotations;

namespace UltraTool.Randoms;

/// <summary>
/// 带权接口
/// </summary>
[PublicAPI]
public interface IWeighted
{
    /// <summary>
    /// 权重
    /// </summary>
    int Weight { get; }
}