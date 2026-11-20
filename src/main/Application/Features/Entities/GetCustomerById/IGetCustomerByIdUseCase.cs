using JacksonVeroneze.NET.Result;
using NttBankApi.Application.Abstractions.UseCases;

namespace NttBankApi.Application.Features.Entities.GetCustomerById;

public interface IGetCustomerByIdUseCase :
    IUseCase<GetCustomerByIdRequest, Result<GetCustomerByIdResponse>>;
