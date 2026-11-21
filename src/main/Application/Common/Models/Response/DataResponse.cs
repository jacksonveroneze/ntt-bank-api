namespace NttBankApi.Application.Common.Models.Response;

public abstract record DataResponse<TType>
{
    public TType? Data { get; init; }
}
