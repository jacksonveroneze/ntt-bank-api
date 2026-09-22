namespace NttBankApi.Application.Models;

public sealed record PagedInfoResponse
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public long TotalCount { get; init; }

    public bool HasNext { get; init; }
}
