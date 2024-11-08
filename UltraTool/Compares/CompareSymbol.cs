namespace UltraTool.Compares;

/// <summary>
/// 比较符号
/// </summary>
[Flags]
public enum CompareSymbol
{
    /// <summary>
    /// 小于
    /// </summary>
    Less = 1 << 0,

    /// <summary>
    /// 大于
    /// </summary>
    Greater = 1 << 1,

    /// <summary>
    /// 不等于
    /// </summary>
    NotEquals = Less | Greater,

    /// <summary>
    /// 等于
    /// </summary>
    Equals = 1 << 2,

    /// <summary>
    /// 小于等于
    /// </summary>
    LessEquals = Less | Equals,

    /// <summary>
    /// 大于等于
    /// </summary>
    GreaterEquals = Greater | Equals
}