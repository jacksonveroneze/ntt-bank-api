using NttBankApi.Domain.Errors;
using Shouldly;

namespace NttBankApi.Tests.Domain.Errors;

public sealed class DomainErrorsTests
{
    [Fact]
    public void CustomerNotFound_Always_HasStableCode()
    {
        // Arrange & Act
        var error = DomainErrors.CustomerError.NotFound;

        // Assert
        error.Code.ShouldBe("Customer.NotFound");
    }
}
