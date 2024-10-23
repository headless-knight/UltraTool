#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using JetBrains.Annotations;

namespace UltraTool;

#if NET7_0_OR_GREATER
/// <summary>
/// 单例接口
/// </summary>
[PublicAPI]
public interface ISingleton<out T>
{
    /// <summary>
    /// 单例实例
    /// </summary>
    static abstract T Instance { get; }
}
#endif

/// <summary>
/// 单例基类
/// </summary>
[PublicAPI]
public abstract class Singleton<
#if NET5_0_OR_GREATER
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
    T>
#if NET7_0_OR_GREATER
    : ISingleton<T>
#endif
{
    /// <summary>
    /// 单例实例
    /// </summary>
    public static T Instance => Nested.Value;

    /// <summary>嵌套类</summary>
    private static class Nested
    {
        /// <summary>实例</summary>
        public static readonly T Value = Activator.CreateInstance<T>();
    }
}