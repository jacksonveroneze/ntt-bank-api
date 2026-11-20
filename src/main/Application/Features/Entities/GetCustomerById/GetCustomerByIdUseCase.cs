using JacksonVeroneze.NET.Result;
using MapsterMapper;
using NttBankApi.Application.Abstractions.Repositories.Entities;
using NttBankApi.Domain.Entities;
using NttBankApi.Domain.Errors;

namespace NttBankApi.Application.Features.Entities.GetCustomerById;

public sealed class GetCustomerByIdUseCase(
    IMapper mapper,
    ICustomerRepository repository) : IGetCustomerByIdUseCase
{
    public async Task<Result<GetCustomerByIdResponse>> ExecuteAsync(
        GetCustomerByIdRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = await repository
            .GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            return Result<GetCustomerByIdResponse>
                .FromNotFound(DomainErrors.CustomerError.NotFound);
        }

        var response = mapper
            .Map<Customer, GetCustomerByIdResponse>(entity);

        return Result<GetCustomerByIdResponse>
            .WithSuccess(response);
    }
}
