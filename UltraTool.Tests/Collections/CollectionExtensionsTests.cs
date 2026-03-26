using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// CollectionExtensions 单元测试
/// </summary>
public class CollectionExtensionsTests
{
    #region ContainsAny 测试

    [Fact]
    public void ContainsAny_ContainsAnyElement_ReturnsTrue()
    {
        IReadOnlyCollection<int> coll = new List<int> { 1, 2, 3 };
        Assert.True(coll.ContainsAny([2, 5, 6]));
    }

    [Fact]
    public void ContainsAny_ContainsNoElement_ReturnsFalse()
    {
        IReadOnlyCollection<int> coll = new List<int> { 1, 2, 3 };
        Assert.False(coll.ContainsAny([4, 5, 6]));
    }

    #endregion

    #region ContainsAll 测试

    [Fact]
    public void ContainsAll_ContainsAllElements_ReturnsTrue()
    {
        IReadOnlyCollection<int> coll = new List<int> { 1, 2, 3, 4, 5 };
        Assert.True(coll.ContainsAll([1, 2, 3]));
    }

    [Fact]
    public void ContainsAll_MissingSomeElements_ReturnsFalse()
    {
        IReadOnlyCollection<int> coll = new List<int> { 1, 2, 3 };
        Assert.False(coll.ContainsAll([1, 2, 4]));
    }

    #endregion

    #region AddIf 测试

    [Fact]
    public void AddIf_ConditionMet_AddsSuccessfully()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AddIf(4, x => x > 0);
        Assert.True(result);
        Assert.Contains(4, list);
    }

    [Fact]
    public void AddIf_ConditionNotMet_DoesNotAdd()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AddIf(-1, x => x > 0);
        Assert.False(result);
        Assert.DoesNotContain(-1, list);
    }

    #endregion

    #region AddRangeIf 测试

    [Fact]
    public void AddRangeIf_PartiallyMeetsCondition_AddsMatchingElements()
    {
        var list = new List<int> { 1, 2, 3 };
        var count = list.AddRangeIf([4, -1, 5, -2], x => x > 0);
        Assert.Equal(2, count);
        Assert.Contains(4, list);
        Assert.Contains(5, list);
        Assert.DoesNotContain(-1, list);
    }

    #endregion

    #region AddNonNull 测试

    [Fact]
    public void AddNonNull_NonNullValue_AddsSuccessfully()
    {
        var list = new List<string>();
        var result = list.AddNonNull("hello");
        Assert.True(result);
        Assert.Single(list);
    }

    [Fact]
    public void AddNonNull_NullValue_DoesNotAdd()
    {
        var list = new List<string>();
        var result = list.AddNonNull<string, List<string>>(null);
        Assert.False(result);
        Assert.Empty(list);
    }

    #endregion

    #region AddNonNullRange 测试

    [Fact]
    public void AddNonNullRange_MixedNullAndNonNull_AddsOnlyNonNull()
    {
        var list = new List<string>();
        var count = list.AddNonNullRange(new string?[] { "a", null, "b", null, "c" });
        Assert.Equal(3, count);
        Assert.Equal(["a", "b", "c"], list);
    }

    #endregion

    #region RemoveRange 测试

    [Fact]
    public void RemoveRange_RemoveExistingElements_ReturnsRemovedCount()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var count = list.RemoveRange([2, 4, 6]);
        Assert.Equal(2, count);
        Assert.DoesNotContain(2, list);
        Assert.DoesNotContain(4, list);
    }

    #endregion

    #region RemoveIf 测试

    [Fact]
    public void RemoveIf_ConditionMetAndExists_RemovesSuccessfully()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.RemoveIf(2, x => x > 0);
        Assert.True(result);
        Assert.DoesNotContain(2, list);
    }

    [Fact]
    public void RemoveIf_ConditionNotMet_DoesNotRemove()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.RemoveIf(2, x => x > 5);
        Assert.False(result);
        Assert.Contains(2, list);
    }

    #endregion

    #region RemoveRangeIf 测试

    [Fact]
    public void RemoveRangeIf_PartiallyMeetsCondition_RemovesMatchingElements()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var count = list.RemoveRangeIf([1, 2, 3, 4, 5], x => x > 3);
        Assert.Equal(2, count);
        Assert.Contains(1, list);
        Assert.Contains(2, list);
        Assert.Contains(3, list);
    }

    [Fact]
    public void RemoveRangeIf_EmptyCollection_ReturnsZero()
    {
        var list = new List<int>();
        var count = list.RemoveRangeIf([1, 2], x => x > 0);
        Assert.Equal(0, count);
    }

    #endregion
}
