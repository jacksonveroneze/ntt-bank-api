using JacksonVeroneze.NET.Result;
using NttBankApi.Domain.ValueObjects;
using Shouldly;

namespace NttBankApi.Tests.Domain.ValueObjects;

public sealed class EmailTests
{
    [Fact]
    public void Create_ValueWithSpacesAndUppercase_ReturnsTrimmedLowercaseValue_CA12()
    {
        // Arrange
        const string value = "  Maria.Teste@Example.com ";

        // Act
        var result = Email.Create(value);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value!.Value.ShouldBe("maria.teste@example.com");
    }

    [Theory]
    [InlineData("maria.teste")]
    [InlineData("maria.teste@")]
    [InlineData("@example.com")]
    [InlineData("maria teste@example.com")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidValue_ReturnsInvalidError_CA13(string value)
    {
        // Arrange & Act
        var result = Email.Create(value);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Type.ShouldBe(ResultType.Invalid);
        result.FirstError!.Code.ShouldBe("Email.Invalid");
    }

    [Fact]
    public void Create_ValueLongerThanMaxLength_ReturnsInvalidError_CA13()
    {
        // Arrange
        string value = new string('a', 250) + "@example.com";

        // Act
        var result = Email.Create(value);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Type.ShouldBe(ResultType.Invalid);
        result.FirstError!.Code.ShouldBe("Email.Invalid");
    }

    [Fact]
    public void ToString_ValidEmail_MasksValue()
    {
        // Arrange
        var email = Email.Create("maria.teste@example.com").Value!;

        // Act
        string text = email.ToString();

        // Assert
        text.ShouldBe("m***@example.com");
    }
}
