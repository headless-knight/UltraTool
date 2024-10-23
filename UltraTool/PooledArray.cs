using System.Buffers;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace UltraTool;

/// <summary>
/// 池化数组
/// </summary>
[MustDisposeResource]
public struct PooledArray<T> : IDisposable
{
    /// <summary>默认数组池</summary>
    private static ArrayPool<T> DefaultPool => ArrayPool<T>.Shared;

    private readonly ArrayPool<T> _pool;
    private readonly bool _clearArray;
    private T[] _array;
    private int _length;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="array">原始数组</param>
    /// <param name="length">有效长度</param>
    /// <param name="pool">数组池</param>
    /// <param name="clearArray">是否释放时清空数组</param>
    public PooledArray(T[] array, int length, ArrayPool<T> pool, bool clearArray)
    {
        _array = array;
        _length = length;
        _pool = pool;
        _clearArray = clearArray;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        var toReturn = _array;
        // 清空对象
        this = default;
        if (toReturn != null)
        {
            ReturnArray(toReturn);
        }
    }

    /// <summary>归还数组</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly void ReturnArray(T[] array) => _pool.Return(array, _clearArray);

    /// <summary>分配数组</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private readonly T[] RentArray(int capacity) => _pool.Rent(capacity);
}