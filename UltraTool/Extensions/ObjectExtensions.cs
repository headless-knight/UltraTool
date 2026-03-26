using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UltraTool.Extensions;

/// <summary>
/// 对象拓展类
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 获取成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <returns>获取值</returns>
    public static object GetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        T>(this T source, string memberName)
    {
        if (!source.TryGetMemberValue(memberName, out var value))
        {
            throw new ArgumentException($"Object not have member {memberName}", nameof(memberName));
        }

        return value;
    }

    /// <summary>
    /// 获取成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <returns>获取值</returns>
    public static TMember GetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        TSource, TMember>(this TSource source, string memberName)
    {
        if (!source.TryGetMemberValue<TSource, TMember>(memberName, out var value))
        {
            throw new ArgumentException($"Object not have member {memberName}", nameof(memberName));
        }

        return value;
    }

    /// <summary>
    /// 设置成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">设置值</param>
    public static void SetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        T>(this T source, string memberName, object value) where T : class
    {
        if (!source.TrySetMemberValue(memberName, value))
        {
            throw new ArgumentException($"Object not have member {memberName}", nameof(memberName));
        }
    }

    /// <summary>
    /// 设置成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">设置值</param>
    public static void SetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        TSource, TMember>(this TSource source, string memberName, TMember value)
        where TSource : class
    {
        if (!source.TrySetMemberValue(memberName, value))
        {
            throw new ArgumentException($"Object not have member {memberName}", nameof(memberName));
        }
    }

    /// <summary>
    /// 尝试获取成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">获取值</param>
    /// <returns>是否获取成功</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        T>(this T source, string memberName, [MaybeNullWhen(false)] out object value)
        => ObjectAccessor<T>.TryGetMember(source, memberName, out value);

    /// <summary>
    /// 尝试获取成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">获取值</param>
    /// <returns>是否获取成功</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        TSource, TMember>(this TSource source, string memberName,
        [MaybeNullWhen(false)] out TMember value) =>
        ObjectAccessor<TSource>.TryGetMember(source, memberName, out value);

    /// <summary>
    /// 尝试设置成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">设置值</param>
    /// <returns>是否设置成功</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TrySetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        T>(this T source, string memberName, object value) where T : class =>
        ObjectAccessor<T>.TrySetMember(source, memberName, value);

    /// <summary>
    /// 尝试设置成员值
    /// </summary>
    /// <param name="source">对象</param>
    /// <param name="memberName">成员名</param>
    /// <param name="value">设置值</param>
    /// <returns>是否设置成功</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TrySetMemberValue<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        TSource, TMember>(this TSource source, string memberName, TMember value)
        where TSource : class => ObjectAccessor<TSource>.TrySetMember(source, memberName, value);

    /// <summary>泛型对象静态访问器</summary>
    private static class ObjectAccessor<
#if NET5_0_OR_GREATER
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields |DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
        T>
    {
        // ReSharper disable StaticMemberInGenericType
        private static readonly Dictionary<string, MemberInfo> MemberInfos;
        private static readonly ConcurrentDictionary<(string, Type), IObjectMemberAccessor> Getters = new();
        private static readonly ConcurrentDictionary<(string, Type), IObjectMemberAccessor> Setters = new();
        // ReSharper restore StaticMemberInGenericType

        /// <summary>静态构造方法</summary>
        static ObjectAccessor()
        {
            var type = typeof(T);
            var fields = type.GetFields();
            var properties = type.GetProperties();
            MemberInfos = fields.Concat<MemberInfo>(properties).ToDictionary(member => member.Name);
        }

        /// <summary>尝试获取成员</summary>
        public static bool TryGetMember<TMember>(T source, string memberName, [MaybeNullWhen(false)] out TMember member)
        {
            var type = typeof(TMember);
            var key = (memberName, type);
            if (Getters.TryGetValue(key, out var getter))
            {
                member = ((ObjectMemberGetAccessor<T, TMember>)getter).Invoke(source);
                return true;
            }

            if (!MemberInfos.TryGetValue(memberName, out var memberInfo))
            {
                member = default;
                return false;
            }

            // 定义参数：T source
            var parameter = Expression.Parameter(typeof(T));
            // 访问成员：source.MemberName
            var expression = Expression.MakeMemberAccess(parameter, memberInfo);
            // 编译为表达式：(T source) => (TMember)source.MemberName，获取数据转换为指定类型返回
            var func = Expression.Lambda<Func<T, TMember>>(Expression.Convert(expression, type), parameter).Compile();
            member = func.Invoke(source);
            Getters.TryAdd(key, new ObjectMemberGetAccessor<T, TMember>(func));
            return true;
        }

        /// <summary>尝试设置成员</summary>
        public static bool TrySetMember<TMember>(T source, string memberName, TMember member)
        {
            var type = typeof(TMember);
            var key = (memberName, type);
            if (Setters.TryGetValue(key, out var setter))
            {
                ((ObjectMemberSetAccessor<T, TMember>)setter).Invoke(source, member);
                return true;
            }

            if (!MemberInfos.TryGetValue(memberName, out var memberInfo)) return false;

            // 定义参数：T source
            var sourceParameter = Expression.Parameter(typeof(T));
            // 访问成员：source.MemberName
            var memberAccess = Expression.MakeMemberAccess(sourceParameter, memberInfo);
            // 获取实际成员类型
            var actualMemberType = (memberInfo as FieldInfo)?.FieldType;
            actualMemberType ??= (memberInfo as PropertyInfo)?.PropertyType;
            // 定义赋值参数：TMember member
            var memberParameter = Expression.Parameter(typeof(TMember));
            // 赋值：source.MemberName = (TActualMemberType)member，赋值参数类型可能和实际成员类型不一致，因此需要转换
            var expression = Expression.Assign(memberAccess, Expression.Convert(memberParameter, actualMemberType!));
            // 编译为表达式：(T source, TMember member) => source.MemberName = (TActualMemberType)member
            var action = Expression.Lambda<Action<T, TMember>>(expression, sourceParameter, memberParameter).Compile();
            action.Invoke(source, member);
            Setters.TryAdd(key, new ObjectMemberSetAccessor<T, TMember>(action));
            return true;
        }
    }

    /// <summary>对象成员访问器接口</summary>
    private interface IObjectMemberAccessor;

    /// <summary>对象成员获取器</summary>
    private sealed class ObjectMemberGetAccessor<TSource, TMember>(Func<TSource, TMember> getter)
        : IObjectMemberAccessor
    {
        /// <summary>调用委托</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TMember Invoke(TSource source) => getter(source);
    }

    /// <summary>对象成员设置器</summary>
    private sealed class ObjectMemberSetAccessor<TSource, TMember>(Action<TSource, TMember> setter)
        : IObjectMemberAccessor
    {
        /// <summary>调用委托</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke(TSource source, TMember member) => setter(source, member);
    }
}