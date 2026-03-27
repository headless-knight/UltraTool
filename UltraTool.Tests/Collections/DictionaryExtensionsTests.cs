using UltraTool.Collections;

namespace UltraTool.Tests.Collections;

/// <summary>
/// DictionaryExtensions 单元测试
/// </summary>
public class DictionaryExtensionsTests
{
    #region EmptyIfNull 测试

    [Fact]
    public void EmptyIfNull_NullDictionary_ReturnsEmptyDictionary()
    {
        IReadOnlyDictionary<string, int>? dict = null;
        var result = dict.EmptyIfNull();
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void EmptyIfNull_NonNullDictionary_ReturnsOriginalDictionary()
    {
        IReadOnlyDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var result = dict.EmptyIfNull();
        Assert.Same(dict, result);
    }

    #endregion

    #region GetOrAdd 测试

    [Fact]
    public void GetOrAdd_KeyExists_ReturnsExistingValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 42 };
        var result = dict.GetOrAdd("key", 99);
        Assert.Equal(42, result);
        Assert.Single(dict);
    }

    [Fact]
    public void GetOrAdd_KeyNotExists_AddsAndReturnsValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.GetOrAdd("key", 42);
        Assert.Equal(42, result);
        Assert.Equal(42, dict["key"]);
    }

    [Fact]
    public void GetOrAdd_WithCreator_KeyExists_ReturnsExistingValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 42 };
        var result = dict.GetOrAdd("key", k => k.Length * 10);
        Assert.Equal(42, result);
    }

    [Fact]
    public void GetOrAdd_WithCreator_KeyNotExists_CreatesAndReturnsValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.GetOrAdd("hello", k => k.Length);
        Assert.Equal(5, result);
        Assert.Equal(5, dict["hello"]);
    }

    [Fact]
    public void GetOrAdd_WithCreatorAndArgs_KeyNotExists_CreatesAndReturnsValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.GetOrAdd("key", static (k, multiplier) => k.Length * multiplier, 10);
        Assert.Equal(30, result);
        Assert.Equal(30, dict["key"]);
    }

    [Fact]
    public void GetOrAdd_WithCreatorAndArgs_KeyExists_ReturnsExistingValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 42 };
        var result = dict.GetOrAdd("key", static (k, multiplier) => k.Length * multiplier, 10);
        Assert.Equal(42, result);
    }

    #endregion

    #region GetOrCreate 测试

    [Fact]
    public void GetOrCreate_KeyNotExists_CreatesNewInstance()
    {
        IDictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
        var result = dict.GetOrCreate("key");
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Same(result, dict["key"]);
    }

    [Fact]
    public void GetOrCreate_KeyExists_ReturnsExistingValue()
    {
        var existing = new List<int> { 1, 2, 3 };
        IDictionary<string, List<int>> dict = new Dictionary<string, List<int>> { ["key"] = existing };
        var result = dict.GetOrCreate("key");
        Assert.Same(existing, result);
    }

    [Fact]
    public void GetOrCreate_KeyNotExists_WithOnCreated_InvokesCallback()
    {
        IDictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
        var callbackInvoked = false;
        var result = dict.GetOrCreate("key", (k, v) =>
        {
            callbackInvoked = true;
            v.Add(100);
        });
        Assert.True(callbackInvoked);
        Assert.Single(result);
        Assert.Equal(100, result[0]);
    }

    [Fact]
    public void GetOrCreate_WithArgs_KeyNotExists_CreatesAndInvokesCallback()
    {
        IDictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
        var result = dict.GetOrCreate("key", static (k, args, v) => v.Add(args), 42);
        Assert.Single(result);
        Assert.Equal(42, result[0]);
    }

    #endregion

    #region AddRange 测试

    [Fact]
    public void AddRange_AddsAllKeyValuePairs()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var range = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2, ["c"] = 3 };
        dict.AddRange(range);
        Assert.Equal(3, dict.Count);
        Assert.Equal(1, dict["a"]);
        Assert.Equal(2, dict["b"]);
        Assert.Equal(3, dict["c"]);
    }

    [Fact]
    public void AddRange_EmptyRange_NoChange()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        dict.AddRange(Array.Empty<KeyValuePair<string, int>>());
        Assert.Single(dict);
    }

    [Fact]
    public void AddRange_DuplicateKey_ThrowsException()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var range = new Dictionary<string, int> { ["a"] = 2 };
        Assert.Throws<ArgumentException>(() => dict.AddRange(range));
    }

    #endregion

    #region TryAddRange 测试

    [Fact]
    public void TryAddRange_AllNew_ReturnsFullCount()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var range = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var count = dict.TryAddRange(range);
        Assert.Equal(2, count);
    }

    [Fact]
    public void TryAddRange_SomeDuplicate_ReturnsAddedCount()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var range = new Dictionary<string, int> { ["a"] = 2, ["b"] = 3 };
        var count = dict.TryAddRange(range);
        Assert.Equal(1, count);
        Assert.Equal(1, dict["a"]); // 原值不变
        Assert.Equal(3, dict["b"]);
    }

    [Fact]
    public void TryAddRange_EmptyRange_ReturnsZero()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var count = dict.TryAddRange(Array.Empty<KeyValuePair<string, int>>());
        Assert.Equal(0, count);
    }

    #endregion

    #region Put 测试

    [Fact]
    public void Put_NewKey_ReturnsDefault()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var old = dict.Put("key", 42);
        Assert.Equal(default, old);
        Assert.Equal(42, dict["key"]);
    }

    [Fact]
    public void Put_ExistingKey_ReturnsOldValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 10 };
        var old = dict.Put("key", 42);
        Assert.Equal(10, old);
        Assert.Equal(42, dict["key"]);
    }

    #endregion

    #region PutAll 测试

    [Fact]
    public void PutAll_OverwritesExistingAndAddsNew()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        var range = new Dictionary<string, int> { ["b"] = 20, ["c"] = 30 };
        dict.PutAll(range);
        Assert.Equal(3, dict.Count);
        Assert.Equal(1, dict["a"]);
        Assert.Equal(20, dict["b"]);
        Assert.Equal(30, dict["c"]);
    }

    [Fact]
    public void PutAll_EmptyRange_NoChange()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        dict.PutAll(Array.Empty<KeyValuePair<string, int>>());
        Assert.Single(dict);
    }

    #endregion

    #region AddOrUpdate 测试

    [Fact]
    public void AddOrUpdate_KeyNotExists_AddsValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.AddOrUpdate("key", 10, (k, existing) => existing + 1);
        Assert.Equal(10, result);
        Assert.Equal(10, dict["key"]);
    }

    [Fact]
    public void AddOrUpdate_KeyExists_UpdatesValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 10 };
        var result = dict.AddOrUpdate("key", 99, (k, existing) => existing + 1);
        Assert.Equal(11, result);
        Assert.Equal(11, dict["key"]);
    }

    [Fact]
    public void AddOrUpdate_WithThreeArgUpdater_KeyNotExists_AddsValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.AddOrUpdate("key", 10, (k, existing, newVal) => existing + newVal);
        Assert.Equal(10, result);
    }

    [Fact]
    public void AddOrUpdate_WithThreeArgUpdater_KeyExists_UpdatesWithBothValues()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 10 };
        var result = dict.AddOrUpdate("key", 5, (k, existing, newVal) => existing + newVal);
        Assert.Equal(15, result);
        Assert.Equal(15, dict["key"]);
    }

    [Fact]
    public void AddOrUpdate_WithCreatorAndUpdater_KeyNotExists_CreatesValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        var result = dict.AddOrUpdate("key",
            static (k, args) => args * 2,
            (k, existing, args) => existing + args,
            5);
        Assert.Equal(10, result);
        Assert.Equal(10, dict["key"]);
    }

    [Fact]
    public void AddOrUpdate_WithCreatorAndUpdater_KeyExists_UpdatesValue()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["key"] = 10 };
        var result = dict.AddOrUpdate("key",
            static (k, args) => args * 2,
            (k, existing, args) => existing + args,
            5);
        Assert.Equal(15, result);
        Assert.Equal(15, dict["key"]);
    }

    #endregion

    #region AddOrUpdateRange 测试

    [Fact]
    public void AddOrUpdateRange_MixedNewAndExisting_AddsAndUpdates()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var range = new Dictionary<string, int> { ["a"] = 10, ["b"] = 20 };
        dict.AddOrUpdateRange(range, (k, existing) => existing + 100);
        Assert.Equal(101, dict["a"]); // 已存在，更新
        Assert.Equal(20, dict["b"]); // 新增
    }

    [Fact]
    public void AddOrUpdateRange_WithThreeArgUpdater_MixedNewAndExisting()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        var range = new Dictionary<string, int> { ["a"] = 10, ["b"] = 20 };
        dict.AddOrUpdateRange(range, (k, existing, newVal) => existing + newVal);
        Assert.Equal(11, dict["a"]); // 1 + 10
        Assert.Equal(20, dict["b"]); // 新增
    }

    [Fact]
    public void AddOrUpdateRange_EmptyRange_NoChange()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        dict.AddOrUpdateRange(Array.Empty<KeyValuePair<string, int>>(), (k, existing) => existing);
        Assert.Single(dict);
    }

    #endregion

    #region RemoveKeys 测试

    [Fact]
    public void RemoveKeys_RemovesExistingKeys_ReturnsCount()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>
        {
            ["a"] = 1, ["b"] = 2, ["c"] = 3
        };
        var count = dict.RemoveKeys(new[] { "a", "c", "d" });
        Assert.Equal(2, count);
        Assert.Single(dict);
        Assert.Equal(2, dict["b"]);
    }

    [Fact]
    public void RemoveKeys_WithRemovedOutput_ReturnsRemovedDictionary()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>
        {
            ["a"] = 1, ["b"] = 2, ["c"] = 3
        };
        dict.RemoveKeys<string, int, Dictionary<string, int>>(new[] { "a", "c", "d" }, out var removed);
        Assert.NotNull(removed);
        Assert.Equal(2, removed!.Count);
        Assert.Equal(1, removed["a"]);
        Assert.Equal(3, removed["c"]);
    }

    [Fact]
    public void RemoveKeys_WithRemovedOutput_NoKeysRemoved_RemovedIsEmpty()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        dict.RemoveKeys<string, int, Dictionary<string, int>>(new[] { "x", "y" }, out var removed);
        Assert.NotNull(removed);
        Assert.Empty(removed!);
    }

    [Fact]
    public void RemoveKeys_WithRemovedOutput_EmptyDict_RemovedIsNull()
    {
        IDictionary<string, int> dict = new Dictionary<string, int>();
        dict.RemoveKeys<string, int, Dictionary<string, int>>(new[] { "a" }, out var removed);
        Assert.Null(removed);
    }

    #endregion

    #region TryGetNestedValue 测试

    [Fact]
    public void TryGetNestedValue_BothKeysExist_ReturnsTrueAndValue()
    {
        var inner = new Dictionary<string, int> { ["inner"] = 42 };
        IReadOnlyDictionary<string, Dictionary<string, int>> dict =
            new Dictionary<string, Dictionary<string, int>> { ["outer"] = inner };
        var result = dict.TryGetNestedValue("outer", "inner", out var value);
        Assert.True(result);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetNestedValue_OuterKeyNotExists_ReturnsFalse()
    {
        IReadOnlyDictionary<string, Dictionary<string, int>> dict =
            new Dictionary<string, Dictionary<string, int>>();
        var result = dict.TryGetNestedValue("outer", "inner", out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetNestedValue_InnerKeyNotExists_ReturnsFalse()
    {
        var inner = new Dictionary<string, int>();
        IReadOnlyDictionary<string, Dictionary<string, int>> dict =
            new Dictionary<string, Dictionary<string, int>> { ["outer"] = inner };
        var result = dict.TryGetNestedValue("outer", "missing", out var value);
        Assert.False(result);
        Assert.Equal(default, value);
    }

    [Fact]
    public void TryGetNestedValue_ReadOnlyNested_BothKeysExist_ReturnsTrueAndValue()
    {
        IReadOnlyDictionary<string, int> inner = new Dictionary<string, int> { ["inner"] = 42 };
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> dict =
            new Dictionary<string, IReadOnlyDictionary<string, int>> { ["outer"] = inner };
        var result = dict.TryGetNestedValue("outer", "inner", out var value);
        Assert.True(result);
        Assert.Equal(42, value);
    }

    #endregion

    #region NestedKeys 测试

    [Fact]
    public void NestedKeys_ReturnsAllKeyPairs()
    {
        var dict = new Dictionary<string, Dictionary<string, int>>
        {
            ["a"] = new() { ["x"] = 1, ["y"] = 2 },
            ["b"] = new() { ["z"] = 3 }
        };
        IReadOnlyDictionary<string, Dictionary<string, int>> readOnlyDict = dict;
        var keys = readOnlyDict.NestedKeys().ToList();
        Assert.Equal(3, keys.Count);
        Assert.Contains(("a", "x"), keys);
        Assert.Contains(("a", "y"), keys);
        Assert.Contains(("b", "z"), keys);
    }

    [Fact]
    public void NestedKeys_EmptyDict_ReturnsEmpty()
    {
        IReadOnlyDictionary<string, Dictionary<string, int>> dict =
            new Dictionary<string, Dictionary<string, int>>();
        var keys = dict.NestedKeys().ToList();
        Assert.Empty(keys);
    }

    [Fact]
    public void NestedKeys_ReadOnlyNested_ReturnsAllKeyPairs()
    {
        var dict = new Dictionary<string, IReadOnlyDictionary<string, int>>
        {
            ["a"] = new Dictionary<string, int> { ["x"] = 1 },
            ["b"] = new Dictionary<string, int> { ["y"] = 2 }
        };
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> readOnlyDict = dict;
        var keys = readOnlyDict.NestedKeys().ToList();
        Assert.Equal(2, keys.Count);
        Assert.Contains(("a", "x"), keys);
        Assert.Contains(("b", "y"), keys);
    }

    #endregion

    #region NestedValues 测试

    [Fact]
    public void NestedValues_ReturnsAllValues()
    {
        var dict = new Dictionary<string, Dictionary<string, int>>
        {
            ["a"] = new() { ["x"] = 1, ["y"] = 2 },
            ["b"] = new() { ["z"] = 3 }
        };
        IReadOnlyDictionary<string, Dictionary<string, int>> readOnlyDict = dict;
        var values = readOnlyDict.NestedValues().OrderBy(v => v).ToList();
        Assert.Equal([1, 2, 3], values);
    }

    [Fact]
    public void NestedValues_ReadOnlyNested_ReturnsAllValues()
    {
        var dict = new Dictionary<string, IReadOnlyDictionary<string, int>>
        {
            ["a"] = new Dictionary<string, int> { ["x"] = 10 },
            ["b"] = new Dictionary<string, int> { ["y"] = 20 }
        };
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, int>> readOnlyDict = dict;
        var values = readOnlyDict.NestedValues().OrderBy(v => v).ToList();
        Assert.Equal([10, 20], values);
    }

    #endregion

    #region Swap 测试

    [Fact]
    public void Swap_TwoKeys_SwapsValues()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
        dict.Swap("a", "b");
        Assert.Equal(2, dict["a"]);
        Assert.Equal(1, dict["b"]);
    }

    [Fact]
    public void Swap_SameKey_NoChange()
    {
        IDictionary<string, int> dict = new Dictionary<string, int> { ["a"] = 1 };
        dict.Swap("a", "a");
        Assert.Equal(1, dict["a"]);
    }

    #endregion

    #region Shuffle 测试

    [Fact]
    public void Shuffle_DictionaryValuesUnchanged_OnlyOrderMayChange()
    {
        IDictionary<int, string> dict = new Dictionary<int, string>
        {
            [1] = "a", [2] = "b", [3] = "c", [4] = "d", [5] = "e"
        };
        var originalKeys = dict.Keys.OrderBy(k => k).ToList();
        var originalValues = dict.Values.OrderBy(v => v).ToList();
        dict.Shuffle();
        // 键集合不变
        Assert.Equal(originalKeys, dict.Keys.OrderBy(k => k).ToList());
        // 值集合不变（只是可能重新分配了）
        Assert.Equal(originalValues, dict.Values.OrderBy(v => v).ToList());
    }

    #endregion
}
