namespace UltraTool.Collections;

/// <summary>
/// 数组帮助类
/// </summary>
public static class ArrayHelper
{
    /// <summary>
    /// 分配未初始化的数组
    /// </summary>
    /// <param name="length">长度</param>
    /// <returns>数组</returns>
    /// <remarks>此API在.NET5之后实际有效，此版本前效果等同直接分配数组</remarks>
    internal static T[] AllocateUninitializedArray<T>(int length) =>
#if NET5_0_OR_GREATER
        GC.AllocateUninitializedArray<T>(length);
#else
        new T[length];
#endif
}