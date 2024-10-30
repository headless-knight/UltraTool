using UltraTool.Collections;

namespace UltraTool.Tests;

public class SingleLinkedTest
{
    [Fact]
    public void Test()
    {
        var list = new SingleLinkedList<int>();
        Assert.Empty(list);
        list.Add(1);
        Assert.Single(list);
        Assert.Equal(1, list.First!.Value);
        list.Add(3);
        Assert.Equal(3, list.Last!.Value);
        Assert.Equal(2, list.Count);
        list.AddAfter(list.First, 2);
        Assert.Equal(3, list.Count);
        Assert.Equal(2, list.First!.Next!.Value);
        Assert.True(list.Remove(2));
        Assert.Equal(2, list.Count);
        Assert.Equal(new[] { 1, 3 }, list);
    }
}