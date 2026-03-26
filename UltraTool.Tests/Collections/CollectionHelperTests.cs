using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// CollectionHelper 单元测试
/// </summary>
public class CollectionHelperTests
{
    #region Union 测试

    [Fact]
    public void Union_TwoNormalCollections_ReturnsUnion()
    {
        var coll1 = new List<string> { "a", "b", "c", "c", "c" };
        var coll2 = new List<string> { "a", "b", "c", "c" };
        var result = CollectionHelper.Union(coll1, coll2);
        // Union keeps max count: a=1, b=1, c=3
        Assert.Equal(1, result.Count(x => x == "a"));
        Assert.Equal(1, result.Count(x => x == "b"));
        Assert.Equal(3, result.Count(x => x == "c"));
    }

    [Fact]
    public void Union_FirstCollectionEmpty_ReturnsSecondCollection()
    {
        var coll1 = new List<int>();
        var coll2 = new List<int> { 1, 2, 3 };
        var result = CollectionHelper.Union(coll1, coll2);
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Union_SecondCollectionEmpty_ReturnsFirstCollection()
    {
        var coll1 = new List<int> { 1, 2, 3 };
        var coll2 = new List<int>();
        var result = CollectionHelper.Union(coll1, coll2);
        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Union_NoIntersection_ReturnsAllElements()
    {
        var coll1 = new List<int> { 1, 2 };
        var coll2 = new List<int> { 3, 4 };
        var result = CollectionHelper.Union(coll1, coll2);
        Assert.Equal(4, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
    }

    #endregion

    #region Intersection 测试

    [Fact]
    public void Intersection_TwoNormalCollections_ReturnsIntersection()
    {
        var coll1 = new List<string> { "a", "b", "c", "c", "c" };
        var coll2 = new List<string> { "a", "b", "c", "c" };
        var result = CollectionHelper.Intersection(coll1, coll2);
        // Intersection keeps min count: a=1, b=1, c=2
        Assert.Equal(1, result.Count(x => x == "a"));
        Assert.Equal(1, result.Count(x => x == "b"));
        Assert.Equal(2, result.Count(x => x == "c"));
    }

    [Fact]
    public void Intersection_FirstCollectionEmpty_ReturnsEmptyCollection()
    {
        var coll1 = new List<int>();
        var coll2 = new List<int> { 1, 2, 3 };
        var result = CollectionHelper.Intersection(coll1, coll2);
        Assert.Empty(result);
    }

    [Fact]
    public void Intersection_NoIntersection_ReturnsEmptyCollection()
    {
        var coll1 = new List<int> { 1, 2 };
        var coll2 = new List<int> { 3, 4 };
        var result = CollectionHelper.Intersection(coll1, coll2);
        Assert.Empty(result);
    }

    #endregion

    #region Disjunction 测试

    [Fact]
    public void Disjunction_TwoNormalCollections_ReturnsDifference()
    {
        var coll1 = new List<string> { "a", "b", "c", "c", "c" };
        var coll2 = new List<string> { "a", "b", "c", "c" };
        var result = CollectionHelper.Disjunction(coll1, coll2);
        // Difference: a=0, b=0, c=1
        Assert.Single(result);
        Assert.Equal("c", result[0]);
    }

    [Fact]
    public void Disjunction_FirstCollectionEmpty_ReturnsSecondCollection()
    {
        var coll1 = new List<int>();
        var coll2 = new List<int> { 1, 2, 3 };
        var result = CollectionHelper.Disjunction(coll1, coll2);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Disjunction_NoIntersection_ReturnsAllElements()
    {
        var coll1 = new List<int> { 1, 2 };
        var coll2 = new List<int> { 3, 4 };
        var result = CollectionHelper.Disjunction(coll1, coll2);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void Disjunction_Identical_ReturnsEmptyCollection()
    {
        var coll1 = new List<int> { 1, 2, 3 };
        var coll2 = new List<int> { 1, 2, 3 };
        var result = CollectionHelper.Disjunction(coll1, coll2);
        Assert.Empty(result);
    }

    #endregion
}
