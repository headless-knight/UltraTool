using UltraTool.Collections;
using Xunit.Abstractions;

namespace UltraTool.Tests;

public class ListTest(ITestOutputHelper output)
{
    [Fact]
    public void EmptyIfNullTest()
    {
        var list = ((List<int>?)null).EmptyIfNull();
        Assert.NotNull(list);
    }

    [Fact]
    public void IsValidIndexTest()
    {
        var list = new List<int>([1, 2, 3]);
        Assert.True(list.IsValidIndex(0));
        Assert.True(list.IsValidIndex(2));
        Assert.False(list.IsValidIndex(3));
        Assert.False(list.IsValidIndex(-1));
    }

    [Fact]
    public void GetValueTest()
    {
        var list = new List<int>([1, 2, 3]);
        var isGot1 = list.TryGetValue(1, out var got1);
        Assert.True(isGot1);
        Assert.Equal(2, got1);
        var isGot2 = list.TryGetValue(4, out var got2);
        Assert.False(isGot2);
        Assert.Equal(default, got2);
        Assert.Equal(1, list.GetValueOrDefault(0));
        Assert.Equal(2, list.GetValueOrDefault(1));
        Assert.Equal(default, list.GetValueOrDefault(4));
        Assert.Equal(-1, list.GetValueOrDefault(4, -1));
    }

    [Fact]
    public void AddRangeTest()
    {
        var list = new List<int>();
        list.AddRange([1, 2, 3]);
        Assert.Equal([1, 2, 3], list.ToArray());
    }
}