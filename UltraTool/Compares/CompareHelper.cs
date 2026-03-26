using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool.Compares;

/// <summary>
/// 比较帮助类
/// </summary>
public static class CompareHelper
{
    /// <summary>
    /// 对两个值进行比较
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="symbol">比较符号</param>
    /// <returns>是否符合比较规则</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Compare<T>(T value1, T value2, CompareSymbol symbol) where T : IComparable<T> => symbol switch
    {
        CompareSymbol.Less => value1.CompareTo(value2) < 0,
        CompareSymbol.Greater => value1.CompareTo(value2) > 0,
        CompareSymbol.Equals => value1.CompareTo(value2) == 0,
        CompareSymbol.LessEquals => value1.CompareTo(value2) <= 0,
        CompareSymbol.GreaterEquals => value1.CompareTo(value2) >= 0,
        CompareSymbol.NotEquals => value1.CompareTo(value2) != 0,
        _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "Not defined comparison symbol")
    };

    /// <summary>
    /// 对两个值进行比较
    /// </summary>
    /// <param name="value1">值1</param>
    /// <param name="value2">值2</param>
    /// <param name="symbol">比较符号</param>
    /// <param name="comparer">比较器</param>
    /// <returns>是否符合比较规则</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Compare<T, TComparer>(T value1, T value2, CompareSymbol symbol, TComparer comparer)
        where TComparer : IComparer<T> => symbol switch
    {
        CompareSymbol.Less => comparer.Compare(value1, value2) < 0,
        CompareSymbol.Greater => comparer.Compare(value1, value2) > 0,
        CompareSymbol.Equals => comparer.Compare(value1, value2) == 0,
        CompareSymbol.LessEquals => comparer.Compare(value1, value2) <= 0,
        CompareSymbol.GreaterEquals => comparer.Compare(value1, value2) >= 0,
        CompareSymbol.NotEquals => comparer.Compare(value1, value2) != 0,
        _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, "Not defined comparison symbol")
    };

    /// <summary>
    /// 对值序列和值进行比较，判断序列中所有元素与比较值是否符合比较规则
    /// </summary>
    /// <param name="range">值序列</param>
    /// <param name="value">比较值</param>
    /// <param name="symbol">比较符号</param>
    /// <returns>是否符合比较规则</returns>
    public static bool AllCompare<T>([InstantHandle] IEnumerable<T> range, T value, CompareSymbol symbol)
        where T : IComparable<T>
    {
        foreach (var item in range)
        {
            if (!Compare(item, value, symbol)) return false;
        }

        return true;
    }

    /// <summary>
    /// 对值序列和值进行比较，判断序列中所有元素与比较值是否符合比较规则
    /// </summary>
    /// <param name="range">值序列</param>
    /// <param name="value">比较值</param>
    /// <param name="symbol">比较符号</param>
    /// <param name="comparer">比较器</param>
    /// <returns>是否符合比较规则</returns>
    public static bool AllCompare<T, TEnumerable, TComparer>([InstantHandle] TEnumerable range, T value,
        CompareSymbol symbol, TComparer comparer) where TEnumerable : IEnumerable<T> where TComparer : IComparer<T>
    {
        foreach (var item in range)
        {
            if (!Compare(item, value, symbol, comparer)) return false;
        }

        return true;
    }

    /// <summary>
    /// 判断指定值是否在区间内
    /// </summary>
    /// <param name="value">比较值</param>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间模式，默认闭区间</param>
    /// <returns>是否在区间内</returns>
    public static bool InRange<T>(in T value, in T start, in T end, RangeMode mode = RangeMode.Close)
        where T : IComparable<T>
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        return InRangeInternal(value, start, end, mode);
    }

    /// <summary>判断指定值是否在区间内，内部实现</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool InRangeInternal<T>(in T value, in T start, in T end, RangeMode mode)
        where T : IComparable<T> => mode switch
    {
        RangeMode.Open => start.CompareTo(value) < 0 && end.CompareTo(value) > 0,
        RangeMode.Close => start.CompareTo(value) <= 0 && end.CompareTo(value) >= 0,
        RangeMode.OpenClose => start.CompareTo(value) < 0 && end.CompareTo(value) >= 0,
        RangeMode.CloseOpen => start.CompareTo(value) <= 0 && end.CompareTo(value) > 0,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported range mode")
    };

    /// <summary>
    /// 判断指定值是否在区间外
    /// </summary>
    /// <param name="value">比较值</param>
    /// <param name="start">起始值</param>
    /// <param name="end">结束值</param>
    /// <param name="mode">区间模式，默认闭区间</param>
    /// <returns>是否在区间外</returns>
    public static bool OutRange<T>(in T value, in T start, in T end, in RangeMode mode = RangeMode.Close)
        where T : IComparable<T>
    {
        if (start.CompareTo(end) > 0)
        {
            throw new ArgumentException("The start value cannot be greater than the end value");
        }

        return OutRangeInternal(value, start, end, mode);
    }

    /// <summary>判断指定值是否在区间外，内部实现</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool OutRangeInternal<T>(in T value, in T start, in T end, in RangeMode mode)
        where T : IComparable<T> => mode switch
    {
        RangeMode.Open => start.CompareTo(value) >= 0 || end.CompareTo(value) <= 0,
        RangeMode.Close => start.CompareTo(value) < 0 || end.CompareTo(value) > 0,
        RangeMode.OpenClose => start.CompareTo(value) <= 0 || end.CompareTo(value) > 0,
        RangeMode.CloseOpen => start.CompareTo(value) < 0 || end.CompareTo(value) >= 0,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unsupported range mode")
    };

    /// <summary>
    /// 解析比较符号
    /// </summary>
    /// <param name="symbol">符号字符串</param>
    /// <returns>比较符号</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CompareSymbol ParseSymbol(string? symbol) => TryParseSymbol(symbol, out var parsed)
        ? parsed
        : throw new ArgumentException("Invalid comparison symbol");

    /// <summary>
    /// 尝试解析比较符号
    /// </summary>
    /// <param name="symbol">符号字符串</param>
    /// <param name="parsed">解析出的比较符号</param>
    /// <returns>是否解析成功</returns>
    public static bool TryParseSymbol([NotNullWhen(true)] string? symbol, out CompareSymbol parsed)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            parsed = default;
            return false;
        }

        switch (symbol.Trim())
        {
            case CompareSymbolConstants.Less:
            {
                parsed = CompareSymbol.Less;
                return true;
            }
            case CompareSymbolConstants.Greater:
            {
                parsed = CompareSymbol.Greater;
                return true;
            }
            case CompareSymbolConstants.Equals:
            {
                parsed = CompareSymbol.Equals;
                return true;
            }
            case CompareSymbolConstants.LessEquals:
            {
                parsed = CompareSymbol.LessEquals;
                return true;
            }
            case CompareSymbolConstants.GreaterEquals:
            {
                parsed = CompareSymbol.GreaterEquals;
                return true;
            }
            case CompareSymbolConstants.NotEquals:
            {
                parsed = CompareSymbol.NotEquals;
                return true;
            }
            default:
            {
                parsed = default;
                return false;
            }
        }
    }
}