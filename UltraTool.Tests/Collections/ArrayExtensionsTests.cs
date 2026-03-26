using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// ArrayExtensions 单元测试
/// </summary>
public class ArrayExtensionsTests
{
    #region EmptyIfNull 测试

    [Fact]
    public void EmptyIfNull_NullArray_ReturnsEmptyArray()
    {
        int[]? array = null;
        var result = array.EmptyIfNull();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyIfNull_NonNullArray_ReturnsOriginalArray()
    {
        int[] array = [1, 2, 3];
        var result = array.EmptyIfNull();
        Assert.Same(array, result);
    }

    #endregion

    #region AsReadOnlySpan 测试

    [Fact]
    public void AsReadOnlySpan_NormalArray_ReturnsCorrectSpan()
    {
        int[] array = [1, 2, 3];
        var span = array.AsReadOnlySpan();
        Assert.Equal(3, span.Length);
        Assert.Equal(1, span[0]);
        Assert.Equal(2, span[1]);
        Assert.Equal(3, span[2]);
    }

    [Fact]
    public void AsReadOnlySpan_SpecifiedRange_ReturnsCorrectSpan()
    {
        int[] array = [1, 2, 3, 4, 5];
        var span = array.AsReadOnlySpan(1, 3);
        Assert.Equal(3, span.Length);
        Assert.Equal(2, span[0]);
        Assert.Equal(3, span[1]);
        Assert.Equal(4, span[2]);
    }

    #endregion

    #region AsReadOnlyMemory 测试

    [Fact]
    public void AsReadOnlyMemory_NormalArray_ReturnsCorrectMemory()
    {
        int[] array = [1, 2, 3];
        var memory = array.AsReadOnlyMemory();
        Assert.Equal(3, memory.Length);
    }

    [Fact]
    public void AsReadOnlyMemory_SpecifiedRange_ReturnsCorrectMemory()
    {
        int[] array = [1, 2, 3, 4, 5];
        var memory = array.AsReadOnlyMemory(1, 3);
        Assert.Equal(3, memory.Length);
    }

    #endregion

    #region Shuffle 测试

    [Fact]
    public void Shuffle_ArrayElementsUnchanged_OnlyOrderMayChange()
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        var original = array.ToArray();
        array.Shuffle();
        // Element set should be the same
        Assert.Equal(original.OrderBy(x => x), array.OrderBy(x => x));
    }

    #endregion
}
