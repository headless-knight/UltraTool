using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// ListExtensions 单元测试
/// </summary>
public class ListExtensionsTests
{
    #region EmptyIfNull 测试

    [Fact]
    public void EmptyIfNull_NullList_ReturnsEmptyList()
    {
        IReadOnlyList<int>? list = null;
        var result = list.EmptyIfNull();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyIfNull_NonNullList_ReturnsOriginalList()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        var result = list.EmptyIfNull();
        Assert.Same(list, result);
    }

    #endregion

    #region IsValidIndex 测试

    [Fact]
    public void IsValidIndex_ValidIndex_ReturnsTrue()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.True(list.IsValidIndex(0));
        Assert.True(list.IsValidIndex(1));
        Assert.True(list.IsValidIndex(2));
    }

    [Fact]
    public void IsValidIndex_NegativeIndex_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.False(list.IsValidIndex(-1));
    }

    [Fact]
    public void IsValidIndex_IndexEqualToCount_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.False(list.IsValidIndex(3));
    }

    [Fact]
    public void IsValidIndex_IndexGreaterThanCount_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.False(list.IsValidIndex(10));
    }

    #endregion

    #region IsInvalidIndex 测试

    [Fact]
    public void IsInvalidIndex_ValidIndex_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.False(list.IsInvalidIndex(0));
        Assert.False(list.IsInvalidIndex(2));
    }

    [Fact]
    public void IsInvalidIndex_NegativeIndex_ReturnsTrue()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.True(list.IsInvalidIndex(-1));
    }

    [Fact]
    public void IsInvalidIndex_IndexOutOfRange_ReturnsTrue()
    {
        IReadOnlyList<int> list = new List<int> { 1, 2, 3 };
        Assert.True(list.IsInvalidIndex(3));
    }

    #endregion

    #region TryGetValue 测试

    [Fact]
    public void TryGetValue_ValidIndex_ReturnsTrueAndValue()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryGetValue(1, out var value);
        Assert.True(result);
        Assert.Equal(20, value);
    }

    [Fact]
    public void TryGetValue_InvalidIndex_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryGetValue(5, out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetValue_NegativeIndex_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryGetValue(-1, out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetValue_NullList_ReturnsFalse()
    {
        IReadOnlyList<int>? list = null;
        var result = list.TryGetValue(0, out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetValue_EmptyList_ReturnsFalse()
    {
        IReadOnlyList<int> list = new List<int>();
        var result = list.TryGetValue(0, out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    #endregion

    #region GetValueOrDefault 测试

    [Fact]
    public void GetValueOrDefault_ValidIndex_ReturnsValue()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.GetValueOrDefault(1);
        Assert.Equal(20, result);
    }

    [Fact]
    public void GetValueOrDefault_InvalidIndex_ReturnsDefault()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.GetValueOrDefault(5);
        Assert.Equal(default, result);
    }

    [Fact]
    public void GetValueOrDefault_WithDefaultValue_ValidIndex_ReturnsValue()
    {
        var list = new List<int> { 10, 20, 30 };
        var result = list.GetValueOrDefault(1, -1);
        Assert.Equal(20, result);
    }

    [Fact]
    public void GetValueOrDefault_WithDefaultValue_InvalidIndex_ReturnsDefaultValue()
    {
        var list = new List<int> { 10, 20, 30 };
        var result = list.GetValueOrDefault(5, -1);
        Assert.Equal(-1, result);
    }

    #endregion

    #region GetNearestOrDefault 测试

    [Fact]
    public void GetNearestOrDefault_ValidIndex_ReturnsValue()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.GetNearestOrDefault(1);
        Assert.Equal(20, result);
    }

    [Fact]
    public void GetNearestOrDefault_NegativeIndex_ReturnsFirstElement()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.GetNearestOrDefault(-5);
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetNearestOrDefault_IndexExceedsCount_ReturnsLastElement()
    {
        IReadOnlyList<int> list = new List<int> { 10, 20, 30 };
        var result = list.GetNearestOrDefault(100);
        Assert.Equal(30, result);
    }

    [Fact]
    public void GetNearestOrDefault_EmptyList_ReturnsDefault()
    {
        IReadOnlyList<int> list = new List<int>();
        var result = list.GetNearestOrDefault(0);
        Assert.Equal(default, result);
    }

    [Fact]
    public void GetNearestOrDefault_WithDefaultValue_EmptyList_ReturnsDefaultValue()
    {
        var list = new List<int>();
        var result = list.GetNearestOrDefault(0, -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void GetNearestOrDefault_WithDefaultValue_NegativeIndex_ReturnsFirstElement()
    {
        var list = new List<int> { 10, 20, 30 };
        var result = list.GetNearestOrDefault(-1, -1);
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetNearestOrDefault_WithDefaultValue_IndexExceedsCount_ReturnsLastElement()
    {
        var list = new List<int> { 10, 20, 30 };
        var result = list.GetNearestOrDefault(100, -1);
        Assert.Equal(30, result);
    }

    #endregion

    #region AddRange 测试

    [Fact]
    public void AddRange_AddsAllElements()
    {
        IList<int> list = new List<int> { 1, 2 };
        list.AddRange(new[] { 3, 4, 5 });
        Assert.Equal([1, 2, 3, 4, 5], list);
    }

    [Fact]
    public void AddRange_EmptyRange_NoChange()
    {
        IList<int> list = new List<int> { 1, 2 };
        list.AddRange(Array.Empty<int>());
        Assert.Equal([1, 2], list);
    }

    #endregion

    #region RemoveFirst 测试

    [Fact]
    public void RemoveFirst_NonEmptyList_RemovesAndReturnsFirstElement()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var removed = list.RemoveFirst();
        Assert.Equal(10, removed);
        Assert.Equal([20, 30], list);
    }

    #endregion

    #region TryRemoveFirst 测试

    [Fact]
    public void TryRemoveFirst_NonEmptyList_ReturnsTrue()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryRemoveFirst();
        Assert.True(result);
        Assert.Equal([20, 30], list);
    }

    [Fact]
    public void TryRemoveFirst_EmptyList_ReturnsFalse()
    {
        IList<int> list = new List<int>();
        var result = list.TryRemoveFirst();
        Assert.False(result);
    }

    [Fact]
    public void TryRemoveFirst_NullList_ReturnsFalse()
    {
        IList<int>? list = null;
        var result = list.TryRemoveFirst();
        Assert.False(result);
    }

    [Fact]
    public void TryRemoveFirst_WithPredicate_MatchFound_RemovesAndReturnsTrue()
    {
        IList<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.TryRemoveFirst(x => x > 3);
        Assert.True(result);
        Assert.Equal([1, 2, 3, 5], list);
    }

    [Fact]
    public void TryRemoveFirst_WithPredicate_NoMatch_ReturnsFalse()
    {
        IList<int> list = new List<int> { 1, 2, 3 };
        var result = list.TryRemoveFirst(x => x > 10);
        Assert.False(result);
        Assert.Equal([1, 2, 3], list);
    }

    [Fact]
    public void TryRemoveFirst_WithPredicate_NullList_ReturnsFalse()
    {
        IList<int>? list = null;
        var result = list.TryRemoveFirst(x => x > 0);
        Assert.False(result);
    }

    [Fact]
    public void TryRemoveFirst_WithOutParam_NonEmptyList_ReturnsTrueAndRemovedValue()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryRemoveFirst(out var removed);
        Assert.True(result);
        Assert.Equal(10, removed);
        Assert.Equal([20, 30], list);
    }

    [Fact]
    public void TryRemoveFirst_WithOutParam_EmptyList_ReturnsFalse()
    {
        IList<int> list = new List<int>();
        var result = list.TryRemoveFirst(out var removed);
        Assert.False(result);
        Assert.Equal(default, removed);
    }

    [Fact]
    public void TryRemoveFirst_WithPredicateAndOutParam_MatchFound_ReturnsTrueAndRemovedValue()
    {
        IList<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.TryRemoveFirst(x => x > 3, out var removed);
        Assert.True(result);
        Assert.Equal(4, removed);
        Assert.Equal([1, 2, 3, 5], list);
    }

    [Fact]
    public void TryRemoveFirst_WithPredicateAndOutParam_NoMatch_ReturnsFalse()
    {
        IList<int> list = new List<int> { 1, 2, 3 };
        var result = list.TryRemoveFirst(x => x > 10, out var removed);
        Assert.False(result);
        Assert.Equal(default, removed);
    }

    #endregion

    #region RemoveLast 测试

    [Fact]
    public void RemoveLast_NonEmptyList_RemovesAndReturnsLastElement()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var removed = list.RemoveLast();
        Assert.Equal(30, removed);
        Assert.Equal([10, 20], list);
    }

    #endregion

    #region TryRemoveLast 测试

    [Fact]
    public void TryRemoveLast_NonEmptyList_ReturnsTrue()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryRemoveLast();
        Assert.True(result);
        Assert.Equal([10, 20], list);
    }

    [Fact]
    public void TryRemoveLast_EmptyList_ReturnsFalse()
    {
        IList<int> list = new List<int>();
        var result = list.TryRemoveLast();
        Assert.False(result);
    }

    [Fact]
    public void TryRemoveLast_NullList_ReturnsFalse()
    {
        IList<int>? list = null;
        var result = list.TryRemoveLast();
        Assert.False(result);
    }

    [Fact]
    public void TryRemoveLast_WithPredicate_MatchFound_RemovesLastMatchAndReturnsTrue()
    {
        IList<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.TryRemoveLast(x => x > 3);
        Assert.True(result);
        Assert.Equal([1, 2, 3, 4], list);
    }

    [Fact]
    public void TryRemoveLast_WithPredicate_NoMatch_ReturnsFalse()
    {
        IList<int> list = new List<int> { 1, 2, 3 };
        var result = list.TryRemoveLast(x => x > 10);
        Assert.False(result);
        Assert.Equal([1, 2, 3], list);
    }

    [Fact]
    public void TryRemoveLast_WithOutParam_NonEmptyList_ReturnsTrueAndRemovedValue()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        var result = list.TryRemoveLast(out var removed);
        Assert.True(result);
        Assert.Equal(30, removed);
        Assert.Equal([10, 20], list);
    }

    [Fact]
    public void TryRemoveLast_WithOutParam_EmptyList_ReturnsFalse()
    {
        IList<int> list = new List<int>();
        var result = list.TryRemoveLast(out var removed);
        Assert.False(result);
        Assert.Equal(default, removed);
    }

    [Fact]
    public void TryRemoveLast_WithPredicateAndOutParam_MatchFound_ReturnsTrueAndRemovedValue()
    {
        IList<int> list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.TryRemoveLast(x => x > 3, out var removed);
        Assert.True(result);
        Assert.Equal(5, removed);
        Assert.Equal([1, 2, 3, 4], list);
    }

    [Fact]
    public void TryRemoveLast_WithPredicateAndOutParam_NoMatch_ReturnsFalse()
    {
        IList<int> list = new List<int> { 1, 2, 3 };
        var result = list.TryRemoveLast(x => x > 10, out var removed);
        Assert.False(result);
        Assert.Equal(default, removed);
    }

    #endregion

    #region Swap 测试

    [Fact]
    public void Swap_TwoIndices_SwapsValues()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        list.Swap(0, 2);
        Assert.Equal([30, 20, 10], list);
    }

    [Fact]
    public void Swap_SameIndex_NoChange()
    {
        IList<int> list = new List<int> { 10, 20, 30 };
        list.Swap(1, 1);
        Assert.Equal([10, 20, 30], list);
    }

    #endregion

    #region Shuffle 测试

    [Fact]
    public void Shuffle_ListElementsUnchanged_OnlyOrderMayChange()
    {
        IList<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var original = list.ToArray();
        list.Shuffle();
        Assert.Equal(original.OrderBy(x => x), list.OrderBy(x => x));
    }

    #endregion

    #region AsOrToArray 测试

    [Fact]
    public void AsOrToArray_ArrayInput_ReturnsSameArray()
    {
        int[] array = [1, 2, 3];
        IList<int> list = array;
        var result = list.AsOrToArray();
        Assert.Same(array, result);
    }

    [Fact]
    public void AsOrToArray_ListInput_ReturnsNewArray()
    {
        IList<int> list = new List<int> { 1, 2, 3 };
        var result = list.AsOrToArray();
        Assert.Equal([1, 2, 3], result);
    }

    #endregion
}
