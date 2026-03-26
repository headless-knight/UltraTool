using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// SetExtensions 单元测试
/// </summary>
public class SetExtensionsTests
{
    #region EmptyIfNull 测试

    [Fact]
    public void EmptyIfNull_NullSet_ReturnsEmptySet()
    {
        HashSet<int>? set = null;
        var result = set.EmptyIfNull();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyIfNull_NonNullSet_ReturnsOriginalSet()
    {
        var set = new HashSet<int> { 1, 2, 3 };
        var result = set.EmptyIfNull();
        Assert.Equal(3, result.Count);
    }

    #endregion

    #region AddRange 测试

    [Fact]
    public void AddRange_AddNewElements_ReturnsAddedCount()
    {
        var set = new HashSet<int> { 1, 2 };
        var count = set.AddRange([3, 4, 5]);
        Assert.Equal(3, count);
        Assert.Equal(5, set.Count);
    }

    [Fact]
    public void AddRange_AddDuplicateElements_ReturnsActualAddedCount()
    {
        var set = new HashSet<int> { 1, 2, 3 };
        var count = set.AddRange([2, 3, 4, 5]);
        Assert.Equal(2, count);
        Assert.Equal(5, set.Count);
    }

    [Fact]
    public void AddRange_AddEmptySequence_ReturnsZero()
    {
        var set = new HashSet<int> { 1, 2 };
        var count = set.AddRange(Array.Empty<int>());
        Assert.Equal(0, count);
    }

    #endregion
}
