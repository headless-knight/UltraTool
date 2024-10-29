using Xunit.Abstractions;

namespace UltraTool.Tests;

public class ExceptionTest(ITestOutputHelper output)
{
    [Fact]
    public void BuilderTest()
    {
        var builder = ExceptionBuilder.CreateDefault("测试异常构建");
        builder.ThrowIfHasError();
        builder.AddError("错误1");
        builder.AddError("错误2");
        var exception = Assert.Throws<Exception>(() => builder.ThrowIfHasError());
        output.WriteLine(exception.ToString());
    }
}