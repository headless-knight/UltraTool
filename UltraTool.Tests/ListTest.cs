using System.Text.Json;
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

    [Fact]
    public void RemoveTest()
    {
        var list = new List<int>([1, 2, 3, 4, 5, 6, 7, 8, 9]);
        Assert.Equal(1, list.RemoveFirst());
        var success = list.TryRemoveFirst(it => it > 2, out var removed);
        Assert.True(success);
        Assert.Equal(3, removed);
        Assert.Equal(9, list.RemoveLast());
        success = list.TryRemoveLast(it => it < 5, out removed);
        Assert.True(success);
        Assert.Equal(4, removed);
    }

    [Fact]
    public void MiscTest()
    {
        var list = new List<int>([1, 2, 3]);
        Assert.True(list.SequenceEqual(list.AsReadOnly()));
        list.Swap(0, 1);
        Assert.Equal(2, list[0]);
        list.Shuffle();
        output.WriteLine(JsonSerializer.Serialize(list));
        Assert.True(list.Count >= 3);
        Assert.True(new[] { 1, 2, 3 }.All(list.Contains));
    }
}