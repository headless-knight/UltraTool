namespace UltraTool.Tests;

/// <summary>
/// ExceptionBuilder 单元测试
/// </summary>
public class ExceptionBuilderTests
{
    #region ExceptionBuilder<T> 测试

    [Fact]
    public void HasError_NoError_ReturnsFalse()
    {
        var builder = ExceptionBuilder.CreateDefault();
        Assert.False(builder.HasError);
    }

    [Fact]
    public void HasError_HasError_ReturnsTrue()
    {
        var builder = ExceptionBuilder.CreateDefault();
        builder.AddError("test error");
        Assert.True(builder.HasError);
    }

    [Fact]
    public void GetErrorString_NoError_ReturnsEmptyString()
    {
        var builder = ExceptionBuilder.CreateDefault();
        Assert.Equal(string.Empty, builder.GetErrorString());
    }

    [Fact]
    public void GetErrorString_HasErrorNoTitle_ReturnsErrorMessage()
    {
        var builder = ExceptionBuilder.CreateDefault();
        builder.AddError("error1");
        var result = builder.GetErrorString();
        Assert.Contains("error1", result);
    }

    [Fact]
    public void GetErrorString_HasErrorWithTitle_ContainsTitleAndErrorMessage()
    {
        var builder = ExceptionBuilder.CreateDefault("MyTitle");
        builder.AddError("error1");
        var result = builder.GetErrorString();
        Assert.Contains("MyTitle", result);
        Assert.Contains("error1", result);
    }

    [Fact]
    public void ThrowIfHasError_NoError_DoesNotThrow()
    {
        var builder = ExceptionBuilder.CreateDefault();
        builder.ThrowIfHasError(); // 不应抛出
    }

    [Fact]
    public void ThrowIfHasError_HasError_ThrowsException()
    {
        var builder = ExceptionBuilder.CreateDefault();
        builder.AddError("test error");
        Assert.Throws<Exception>(() => builder.ThrowIfHasError());
    }

    [Fact]
    public void Build_HasError_ReturnsExceptionObject()
    {
        var builder = ExceptionBuilder.CreateDefault();
        builder.AddError("build error");
        var exception = builder.Build();
        Assert.NotNull(exception);
        Assert.Contains("build error", exception.Message);
    }

    [Fact]
    public void Dispose_HasError_ThrowsException()
    {
        Assert.Throws<Exception>(() =>
        {
            using var builder = ExceptionBuilder.CreateDefault();
            builder.AddError("dispose error");
        });
    }

    [Fact]
    public void Dispose_NoError_DoesNotThrow()
    {
        using var builder = ExceptionBuilder.CreateDefault();
        // 不应抛出
    }

    [Fact]
    public void Create_CustomCreator_UsesCustomConstruction()
    {
        var builder = ExceptionBuilder.Create<InvalidOperationException>(
            creator: msg => new InvalidOperationException(msg));
        builder.AddError("custom error");
        var exception = builder.Build();
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Contains("custom error", exception.Message);
    }

    #endregion

    #region ValueExceptionBuilder<T> 测试

    [Fact]
    public void ValueBuilder_HasError_NoError_ReturnsFalse()
    {
        var builder = ValueExceptionBuilder.CreateDefault();
        Assert.False(builder.HasError);
    }

    [Fact]
    public void ValueBuilder_AddError_HasError_ReturnsTrue()
    {
        var builder = ValueExceptionBuilder.CreateDefault();
        builder.AddError("value error");
        Assert.True(builder.HasError);
    }

    [Fact]
    public void ValueBuilder_ThrowIfHasError_HasError_ThrowsException()
    {
        var builder = ValueExceptionBuilder.CreateDefault();
        builder.AddError("value error");
        Assert.Throws<Exception>(() => builder.ThrowIfHasError());
    }

    [Fact]
    public void ValueBuilder_Build_HasError_ReturnsExceptionObject()
    {
        var builder = ValueExceptionBuilder.CreateDefault("Title");
        builder.AddError("value build error");
        var exception = builder.Build();
        Assert.Contains("Title", exception.Message);
        Assert.Contains("value build error", exception.Message);
    }

    #endregion
}
