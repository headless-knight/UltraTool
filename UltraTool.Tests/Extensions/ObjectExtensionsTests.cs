using UltraTool.Extensions;

namespace UltraTool.Tests.Extensions;

/// <summary>
/// ObjectExtensions 单元测试用辅助类
/// </summary>
public class TestPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string City = string.Empty;
}

/// <summary>
/// ObjectExtensions 单元测试
/// </summary>
public class ObjectExtensionsTests
{
    #region GetMemberValue 测试

    [Fact]
    public void GetMemberValue_GetPropertyValue_ReturnsCorrectValue()
    {
        var person = new TestPerson { Name = "Alice", Age = 30 };
        var name = person.GetMemberValue("Name");
        Assert.Equal("Alice", name);
    }

    [Fact]
    public void GetMemberValue_GetFieldValue_ReturnsCorrectValue()
    {
        var person = new TestPerson { City = "Beijing" };
        var city = person.GetMemberValue("City");
        Assert.Equal("Beijing", city);
    }

    [Fact]
    public void GetMemberValue_GenericVersion_ReturnsCorrectTypedValue()
    {
        var person = new TestPerson { Age = 25 };
        var age = person.GetMemberValue<TestPerson, int>("Age");
        Assert.Equal(25, age);
    }

    [Fact]
    public void GetMemberValue_NonExistentMember_ThrowsException()
    {
        var person = new TestPerson();
        Assert.Throws<ArgumentException>(() => person.GetMemberValue("NotExist"));
    }

    #endregion

    #region SetMemberValue 测试

    [Fact]
    public void SetMemberValue_SetPropertyValue_Succeeds()
    {
        var person = new TestPerson();
        person.SetMemberValue("Name", "Bob");
        Assert.Equal("Bob", person.Name);
    }

    [Fact]
    public void SetMemberValue_SetFieldValue_Succeeds()
    {
        var person = new TestPerson();
        person.SetMemberValue("City", "Shanghai");
        Assert.Equal("Shanghai", person.City);
    }

    [Fact]
    public void SetMemberValue_GenericVersion_Succeeds()
    {
        var person = new TestPerson();
        person.SetMemberValue<TestPerson, int>("Age", 35);
        Assert.Equal(35, person.Age);
    }

    [Fact]
    public void SetMemberValue_NonExistentMember_ThrowsException()
    {
        var person = new TestPerson();
        Assert.Throws<ArgumentException>(() => person.SetMemberValue("NotExist", "value"));
    }

    #endregion

    #region TryGetMemberValue 测试

    [Fact]
    public void TryGetMemberValue_ExistingMember_ReturnsTrue()
    {
        var person = new TestPerson { Name = "Charlie" };
        var result = person.TryGetMemberValue("Name", out string? value);
        Assert.True(result);
        Assert.Equal("Charlie", value);
    }

    [Fact]
    public void TryGetMemberValue_NonExistentMember_ReturnsFalse()
    {
        var person = new TestPerson();
        var result = person.TryGetMemberValue("NotExist", out string? _);
        Assert.False(result);
    }

    #endregion

    #region TrySetMemberValue 测试

    [Fact]
    public void TrySetMemberValue_ExistingMember_ReturnsTrue()
    {
        var person = new TestPerson();
        var result = person.TrySetMemberValue("Name", "Dave");
        Assert.True(result);
        Assert.Equal("Dave", person.Name);
    }

    [Fact]
    public void TrySetMemberValue_NonExistentMember_ReturnsFalse()
    {
        var person = new TestPerson();
        var result = person.TrySetMemberValue("NotExist", "value");
        Assert.False(result);
    }

    #endregion
}
