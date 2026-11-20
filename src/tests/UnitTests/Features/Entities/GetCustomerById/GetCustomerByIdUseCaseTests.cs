using JacksonVeroneze.NET.Result;
using Mapster;
using MapsterMapper;
using Moq;
using NttBankApi.Application.Abstractions.Repositories.Entities;
using NttBankApi.Application.Features.Entities.Common.Mappers;
using NttBankApi.Application.Features.Entities.GetCustomerById;
using NttBankApi.Domain.Entities;
using NttBankApi.Domain.Enums;
using NttBankApi.Domain.ValueObjects;
using Shouldly;

namespace NttBankApi.Tests.Features.Entities.GetCustomerById;

public sealed class GetCustomerByIdUseCaseTests
{
    private static readonly DateTimeOffset CreatedAt =
        new(2026, 1, 15, 10, 0, 0, TimeSpan.Zero);

    private readonly Mock<ICustomerRepository> _repository = new();

    private readonly TypeAdapterConfig _config = BuildConfig();

    private readonly GetCustomerByIdUseCase _useCase;

    public GetCustomerByIdUseCaseTests()
    {
        _useCase = new GetCustomerByIdUseCase(
            new Mapper(_config),
            _repository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ExistingCustomer_ReturnsAllFields_CA01Async()
    {
        // Arrange
        var customer = CreateCustomer(AccountType.Current, Status.Active);

        _repository
            .Setup(repository => repository.GetByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(customer);

        // Act
        var result = await _useCase.ExecuteAsync(
            new GetCustomerByIdRequest(1), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value!.Data!.Name.ShouldBe("Maria Teste");
        result.Value.Data.Email.ShouldBe("maria.teste@example.com");
        result.Value.Data.CreatedAt.ShouldBe(CreatedAt);
        result.Value.Data.AccountType.ShouldBe(AccountType.Current);
        result.Value.Data.Status.ShouldBe(Status.Active);
    }

    [Theory]
    [InlineData(Status.Blocked)]
    [InlineData(Status.Closed)]
    public async Task ExecuteAsync_BlockedOrClosedCustomer_ReturnsCustomerWithStatus_CA02Async(
        Status status)
    {
        // Arrange
        var customer = CreateCustomer(AccountType.Savings, status);

        _repository
            .Setup(repository => repository.GetByIdAsync(2, CancellationToken.None))
            .ReturnsAsync(customer);

        // Act
        var result = await _useCase.ExecuteAsync(
            new GetCustomerByIdRequest(2), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value!.Data!.Status.ShouldBe(status);
    }

    [Fact]
    public async Task ExecuteAsync_CustomerNotFound_ReturnsNotFoundError_CA03Async()
    {
        // Arrange
        _repository
            .Setup(repository => repository.GetByIdAsync(999, CancellationToken.None))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _useCase.ExecuteAsync(
            new GetCustomerByIdRequest(999), CancellationToken.None);

        // Assert
        result.IsFailure.ShouldBeTrue();
        result.Type.ShouldBe(ResultType.NotFound);
        result.FirstError!.Code.ShouldBe("Customer.NotFound");
    }

    [Fact]
    public async Task ExecuteAsync_NullRequest_ThrowsArgumentNullExceptionAsync()
    {
        // Arrange & Act
        var act = () => _useCase.ExecuteAsync(null!, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<ArgumentNullException>(act);
    }

    [Fact]
    public void Config_AllMappersRegistered_CompilesWithoutUnmappedMembers()
    {
        // Arrange & Act
        var act = () => _config.Compile();

        // Assert
        Should.NotThrow(act);
    }

    private static Customer CreateCustomer(
        AccountType accountType,
        Status status)
    {
        var email = Email.Create("maria.teste@example.com").Value!;

        return Customer.Create(
            "Maria Teste", email, CreatedAt, accountType, status).Value!;
    }

    private static TypeAdapterConfig BuildConfig()
    {
        var config = new TypeAdapterConfig
        {
            RequireExplicitMapping = true,
            RequireDestinationMemberSource = true
        };

        new CustomerResponseMapper().Register(config);
        new GetCustomerByIdMapper().Register(config);

        return config;
    }
}
