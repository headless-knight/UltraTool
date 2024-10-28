namespace UltraTool.Tests;

public class SingletonTest
{
    [Fact]
    public void Test()
    {
        Assert.NotNull(Singleton<MyClass>.Instance);
        Assert.Same(Singleton<MyClass>.Instance, Singleton<MyClass>.Instance);
        Assert.NotNull(MyClass.Instance);
        Assert.Same(MyClass.Instance, MyClass.Instance);
    }

    private class MyClass : Singleton<MyClass>;
}