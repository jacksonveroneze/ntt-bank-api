using JacksonVeroneze.NET.Pagination.Offset;
using NttBankApi.Domain.Entities;

namespace NttBankApi.Application.Abstractions.Repositories.Entities;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<Page<Customer>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task CreateAsync(
        Customer customer,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Customer customer,
        CancellationToken cancellationToken);
}
