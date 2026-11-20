using NttBankApi.Application.Abstractions.UseCases;

namespace NttBankApi.Application.Features.Entities.GetCustomerById;

public sealed record GetCustomerByIdRequest(
    int Id) : IBaseRequest;
