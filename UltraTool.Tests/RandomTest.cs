using System.Text.Json;
using UltraTool.Randoms;
using Xunit.Abstractions;

namespace UltraTool.Tests;

public class RandomTest(ITestOutputHelper output)
{
    [Fact]
    public void NextItemTest()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        var item = Random.Shared.Choice(array);
        Assert.Contains(item, array);
        IList<int> items = Random.Shared.Choice(array, 3);
        Assert.Equal(3, items.Count);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        items = Random.Shared.Sample(array, 3);
        Assert.Equal(3, items.Count);
        Assert.True(items.Distinct().Count() == 3);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        var copy = array.ToArray();
        items = Random.Shared.SampleShuffle(copy, 3);
        Assert.Equal(3, items.Count);
        Assert.True(items.Distinct().Count() == 3);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        Assert.True(copy.All(array.Contains));
        output.WriteLine(JsonSerializer.Serialize(copy));
    }

    [Fact]
    public void ShuffleTest()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        Random.Shared.Shuffle(array);
        Assert.Equal(6, array.Distinct().Count());
        output.WriteLine(JsonSerializer.Serialize(array));
        var dict = new Dictionary<int, int>() { { 1, 2 }, { 2, 3 }, { 3, 3 }, { 4, 4 } };
        Random.Shared.Shuffle(dict);
        Assert.Equal(4, dict.Count);
        output.WriteLine(JsonSerializer.Serialize(dict));
    }
}