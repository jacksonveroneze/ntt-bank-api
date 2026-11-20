using JacksonVeroneze.NET.Result;
using NttBankApi.Domain.Entities;
using NttBankApi.Domain.Enums;
using NttBankApi.Domain.ValueObjects;
using Shouldly;

namespace NttBankApi.Tests.Domain.Entities;

public sealed class CustomerTests
{
    private static readonly DateTimeOffset CreatedAt =
        new(2026, 1, 15, 10, 0, 0, TimeSpan.Zero);

    private static readonly Email ValidEmail =
        Email.Create("maria.teste@example.com").Value!;

    [Fact]
    public void Create_NameWithSpaces_ReturnsTrimmedName_CA08()
    {
        // Arrange
        const string name = "  Maria Teste  ";

        // Act
        var result = Customer.Create(
            name, ValidEmail, CreatedAt, AccountType.Current, Status.Active);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value!.Name.ShouldBe("Maria Teste");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyOrWhitespaceName_ReturnsNameInvalidError_CA09(string name)
    {
        // Arrange & Act
        var result = Customer.Create(
            name, ValidEmail, CreatedAt, AccountType.Current, Status.Active);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Type.ShouldBe(ResultType.Invalid);
        result.FirstError!.Code.ShouldBe("Customer.NameInvalid");
    }

    [Fact]
    public void Create_NameAboveMaxLength_ReturnsNameInvalidError_CA10()
    {
        // Arrange
        string name = new('a', 257);

        // Act
        var result = Customer.Create(
            name, ValidEmail, CreatedAt, AccountType.Current, Status.Active);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Type.ShouldBe(ResultType.Invalid);
        result.FirstError!.Code.ShouldBe("Customer.NameInvalid");
    }

    [Fact]
    public void Create_NameAtMaxLength_ReturnsSuccess_CA11()
    {
        // Arrange
        string name = new('a', 256);

        // Act
        var result = Customer.Create(
            name, ValidEmail, CreatedAt, AccountType.Current, Status.Active);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Create_GivenCreatedAt_KeepsCreatedAt_CA14()
    {
        // Arrange
        var createdAt = new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero);

        // Act
        var result = Customer.Create(
            "Maria Teste", ValidEmail, createdAt, AccountType.Savings, Status.Blocked);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value!.CreatedAt.ShouldBe(createdAt);
    }
}
