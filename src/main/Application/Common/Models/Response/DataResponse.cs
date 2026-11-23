using NttBankApi.Application.Abstractions.UseCases;

namespace NttBankApi.Application.Common.Models.Response;

public abstract record DataResponse<TType>
    : IResponse
{
    public TType? Data { get; init; }
}
