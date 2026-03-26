using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// DictionaryHelper 单元测试
/// </summary>
public class DictionaryHelperTests
{
    #region PlusOf 测试

    [Fact]
    public void PlusOf_NormalDictionary_AddsSpecifiedValueToEach()
    {
        var dict = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2,
            ["c"] = 3
        };
        var result = DictionaryHelper.PlusOf(dict, 10);
        Assert.Equal(11, result["a"]);
        Assert.Equal(12, result["b"]);
        Assert.Equal(13, result["c"]);
    }

    [Fact]
    public void PlusOf_EmptyDictionary_ReturnsEmptyDictionary()
    {
        var dict = new Dictionary<string, int>();
        var result = DictionaryHelper.PlusOf(dict, 10);
        Assert.Empty(result);
    }

    [Fact]
    public void PlusOf_AddNegativeNumber_CalculatesCorrectly()
    {
        var dict = new Dictionary<string, int>
        {
            ["x"] = 5,
            ["y"] = 10
        };
        var result = DictionaryHelper.PlusOf(dict, -3);
        Assert.Equal(2, result["x"]);
        Assert.Equal(7, result["y"]);
    }

    [Fact]
    public void PlusOf_AddZero_ValuesUnchanged()
    {
        var dict = new Dictionary<string, int>
        {
            ["a"] = 100
        };
        var result = DictionaryHelper.PlusOf(dict, 0);
        Assert.Equal(100, result["a"]);
    }

    #endregion
}
