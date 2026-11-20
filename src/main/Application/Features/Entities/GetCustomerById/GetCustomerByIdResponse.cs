using NttBankApi.Application.Common.Models.Response;
using NttBankApi.Application.Features.Entities.Common.Models;

namespace NttBankApi.Application.Features.Entities.GetCustomerById;

public sealed record GetCustomerByIdResponse
    : DataResponse<CustomerResponse>;
