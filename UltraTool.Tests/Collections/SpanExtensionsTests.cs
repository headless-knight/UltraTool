using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// SpanExtensions 单元测试
/// </summary>
public class SpanExtensionsTests
{
    #region ReverseTo 测试

    [Fact]
    public void ReverseTo_NormalSpan_ReversesToDestination()
    {
        int[] source = [1, 2, 3, 4, 5];
        var destination = new int[5];
        ((ReadOnlySpan<int>)source).ReverseTo(destination);
        Assert.Equal([5, 4, 3, 2, 1], destination);
    }

    [Fact]
    public void ReverseTo_SingleElement_RemainsUnchanged()
    {
        int[] source = [42];
        var destination = new int[1];
        ((ReadOnlySpan<int>)source).ReverseTo(destination);
        Assert.Equal([42], destination);
    }

    [Fact]
    public void ReverseTo_EmptySpan_DoesNotThrow()
    {
        var source = ReadOnlySpan<int>.Empty;
        var destination = new int[0];
        source.ReverseTo(destination);
        Assert.Empty(destination);
    }

    [Fact]
    public void ReverseTo_LargerDestination_ReversesCorrectly()
    {
        int[] source = [1, 2, 3];
        var destination = new int[5];
        ((ReadOnlySpan<int>)source).ReverseTo(destination);
        Assert.Equal(3, destination[0]);
        Assert.Equal(2, destination[1]);
        Assert.Equal(1, destination[2]);
    }

    #endregion

    #region Shuffle 测试

    [Fact]
    public void Shuffle_SpanElementsUnchanged_OnlyOrderMayChange()
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var original = array.ToArray();
        array.AsSpan().Shuffle();
        Assert.Equal(original.OrderBy(x => x), array.OrderBy(x => x));
    }

    #endregion
}
