using System.Runtime.CompilerServices;

namespace UltraTool.Compares;

/// <summary>
/// 比较符号拓展类
/// </summary>
public static class CompareSymbolExtensions
{
    /// <summary>
    /// 转为相反的符号
    /// </summary>
    /// <param name="symbol">比较符号</param>
    /// <returns>相反的符号</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CompareSymbol ToOpposite(this CompareSymbol symbol) => symbol switch
    {
        CompareSymbol.Less => CompareSymbol.GreaterEquals,
        CompareSymbol.Greater => CompareSymbol.LessEquals,
        CompareSymbol.Equals => CompareSymbol.NotEquals,
        CompareSymbol.LessEquals => CompareSymbol.Greater,
        CompareSymbol.GreaterEquals => CompareSymbol.Less,
        CompareSymbol.NotEquals => CompareSymbol.Equals,
        _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "未定义的比较符号")
    };

    /// <summary>
    /// 转为相反的符号字符串
    /// </summary>
    /// <param name="symbol">比较符号</param>
    /// <returns>相反的符号字符串</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToOppositeString(this CompareSymbol symbol) => symbol.ToOpposite().ToSymbolString();

    /// <summary>
    /// 转为符号字符串
    /// </summary>
    /// <param name="symbol">比较符号</param>
    /// <returns>符号字符串</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToSymbolString(this CompareSymbol symbol) => symbol switch
    {
        CompareSymbol.Less => CompareSymbolConstants.Less,
        CompareSymbol.Greater => CompareSymbolConstants.Greater,
        CompareSymbol.Equals => CompareSymbolConstants.Equals,
        CompareSymbol.LessEquals => CompareSymbolConstants.LessEquals,
        CompareSymbol.GreaterEquals => CompareSymbolConstants.GreaterEquals,
        CompareSymbol.NotEquals => CompareSymbolConstants.NotEquals,
        _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "未定义的比较符号")
    };
}