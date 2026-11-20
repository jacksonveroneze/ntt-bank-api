using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using JacksonVeroneze.NET.Result;
using Moq;
using NttBankApi.Api.Tests.Support;
using NttBankApi.Application.Features.Entities.Common.Models;
using NttBankApi.Application.Features.Entities.GetCustomerById;
using NttBankApi.Domain.Enums;
using NttBankApi.Domain.Errors;
using Shouldly;

namespace NttBankApi.Api.Tests.Endpoints.Entities.v1;

public sealed class GetCustomerByIdEndpointTests : IDisposable
{
    private const string ReadScope = "customers.read";

    private const string CustomerName = "Maria Teste";

    private const string CustomerEmail = "maria.teste@example.com";

    private readonly CustomerApiFactory _factory = new();

    [Fact]
    public async Task Get_ExistingCustomer_ReturnsOkWithCustomerData_CA01Async()
    {
        // Arrange
        GetCustomerByIdResponse response = new()
        {
            Data = new CustomerResponse(
                1,
                CustomerName,
                CustomerEmail,
                new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero),
                AccountType.Current,
                Status.Active),
        };

        SetupUseCase(Result<GetCustomerByIdResponse>.WithSuccess(response));

        using HttpClient client = CreateClient(TestTokens.Create(ReadScope));

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri("/customers/1", UriKind.Relative));

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        using JsonDocument json = JsonDocument.Parse(
            await result.Content.ReadAsStringAsync());
        JsonElement data = json.RootElement.GetProperty("data");

        data.GetProperty("id").GetInt32().ShouldBe(1);
        data.GetProperty("name").GetString().ShouldBe(CustomerName);
        data.GetProperty("email").GetString().ShouldBe(CustomerEmail);
        data.GetProperty("createdAt").GetDateTimeOffset()
            .ShouldBe(new DateTimeOffset(2026, 1, 15, 10, 0, 0, TimeSpan.Zero));
        data.GetProperty("accountType").GetString().ShouldBe("Current");
        data.GetProperty("status").GetString().ShouldBe("Active");
    }

    [Fact]
    public async Task Get_CustomerNotFound_ReturnsNotFoundProblem_CA03Async()
    {
        // Arrange
        SetupUseCase(Result<GetCustomerByIdResponse>
            .FromNotFound(DomainErrors.CustomerError.NotFound));

        using HttpClient client = CreateClient(TestTokens.Create(ReadScope));

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri("/customers/999", UriKind.Relative));

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        using JsonDocument json = JsonDocument.Parse(
            await result.Content.ReadAsStringAsync());

        json.RootElement.GetProperty("title").GetString()
            .ShouldBe("Resource Not Found");
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("99999999999")]
    public async Task Get_InvalidId_ReturnsBadRequestWithoutCallingUseCase_CA04_CA05Async(
        string id)
    {
        // Arrange
        using HttpClient client = CreateClient(TestTokens.Create(ReadScope));

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri($"/customers/{id}", UriKind.Relative));

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        VerifyUseCaseNotCalled();
    }

    [Fact]
    public async Task Get_WithoutToken_ReturnsUnauthorizedWithoutCallingUseCase_CA06Async()
    {
        // Arrange
        using HttpClient client = CreateClient(null);

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri("/customers/1", UriKind.Relative));

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        VerifyUseCaseNotCalled();
    }

    [Fact]
    public async Task Get_TokenWithoutReadScope_ReturnsForbiddenWithoutCallingUseCase_CA07Async()
    {
        // Arrange
        using HttpClient client = CreateClient(TestTokens.Create("other.read"));

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri("/customers/1", UriKind.Relative));

        // Assert
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        VerifyUseCaseNotCalled();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("999")]
    public async Task Get_ErrorResponse_BodyHasNoPersonalData_CA15Async(string id)
    {
        // Arrange
        SetupUseCase(Result<GetCustomerByIdResponse>
            .FromNotFound(DomainErrors.CustomerError.NotFound));

        using HttpClient client = CreateClient(TestTokens.Create(ReadScope));

        // Act
        using HttpResponseMessage result = await client.GetAsync(
            new Uri($"/customers/{id}", UriKind.Relative));

        // Assert
        string body = await result.Content.ReadAsStringAsync();

        body.ShouldNotContain(CustomerName);
        body.ShouldNotContain(CustomerEmail);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }

    private HttpClient CreateClient(string? token)
    {
        HttpClient client = _factory.CreateClient();

        if (token is not null)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    private void SetupUseCase(Result<GetCustomerByIdResponse> result)
    {
        _factory.UseCase
            .Setup(useCase => useCase.ExecuteAsync(
                It.IsAny<GetCustomerByIdRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    }

    private void VerifyUseCaseNotCalled()
    {
        _factory.UseCase.Verify(
            useCase => useCase.ExecuteAsync(
                It.IsAny<GetCustomerByIdRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
