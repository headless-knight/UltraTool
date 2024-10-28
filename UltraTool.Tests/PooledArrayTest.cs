using System.Buffers;

namespace UltraTool.Tests;

public class PooledArrayTest
{
    [Fact]
    public void Test()
    {
        using (var array = PooledArray.Get<int>(5))
        {
            array[0] = 100;
            Assert.Equal(100, array[0]);
            Assert.Equal(5, array.Length);
        }

        var pool = ArrayPool<int>.Create();
        using (var array = PooledArray.Get(5, pool, true))
        {
            array[0] = 100;
            Assert.Equal(100, array[0]);
            Assert.Equal(5, array.Length);
        }
    }

    [Fact]
    public void EmptyTest()
    {
        var array = PooledArray<int>.Empty;
        Assert.Equal(0, array.Length);
        array.Dispose();
        Assert.Equal(0, array.Length);
        array.Dispose();
        Assert.Equal(0, array.Length);
    }
}