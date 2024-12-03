namespace UltraTool.Compares;

/// <summary>
/// 区间接口
/// </summary>
public interface IRange<out T>
{
    /// <summary>
    /// 起始值
    /// </summary>
    T Start { get; }

    /// <summary>
    /// 结束值
    /// </summary>
    T End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    RangeMode Mode { get; }
}