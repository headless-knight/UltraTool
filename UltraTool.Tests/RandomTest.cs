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
        var item = Random.Shared.NextItem(array);
        Assert.Contains(item, array);
        var items = Random.Shared.NextItemsSelection(array, 3);
        Assert.Equal(3, items.Length);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        items = Random.Shared.NextItemsSample(array, 3);
        Assert.Equal(3, items.Length);
        Assert.True(items.Distinct().Count() == 3);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        var copy = array.ToArray();
        items = Random.Shared.NextItemsSampleShuffle(copy, 3);
        Assert.Equal(3, items.Length);
        Assert.True(items.Distinct().Count() == 3);
        foreach (var it in items)
        {
            Assert.Contains(it, array);
        }

        Assert.Equal(copy.Distinct().Count(), array.Distinct().Count());
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