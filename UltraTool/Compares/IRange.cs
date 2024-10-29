namespace Ultra.Common.Compares;

/// <summary>
/// 区间接口
/// </summary>
public interface IRange<out T>
{
    /// <summary>
    /// 起始值
    /// </summary>
    public T Start { get; }

    /// <summary>
    /// 结束值
    /// </summary>
    public T End { get; }

    /// <summary>
    /// 区间类型
    /// </summary>
    public RangeMode Mode { get; }
}