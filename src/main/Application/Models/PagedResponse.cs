namespace NttBankApi.Application.Models;

public record PagedResponse<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];

    public PagedInfoResponse? PageInfo { get; init; }
}
