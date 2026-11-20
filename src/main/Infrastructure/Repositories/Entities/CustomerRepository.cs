using JacksonVeroneze.NET.EntityFramework.Interfaces;
using JacksonVeroneze.NET.Pagination.Offset;
using NttBankApi.Application.Abstractions.Repositories.Entities;
using NttBankApi.Domain.Entities;
using NttBankApi.Infrastructure.Context;

namespace NttBankApi.Infrastructure.Repositories.Entities;

public sealed class CustomerRepository(
    IEfCoreRepository<Customer, DefaultDbContext> efRepository) : ICustomerRepository
{
    public Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return efRepository.GetByIdAsync(
            filter => filter.Id == id, cancellationToken);
    }

    public Task<Page<Customer>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        PaginationParameters pagination = new(page, pageSize);

        return efRepository.GetPagedAsync(
            pagination,
            entity => true,
            entity => entity.Id,
            cancellationToken);
    }

    public async Task CreateAsync(
        Customer customer,
        CancellationToken cancellationToken)
    {
        await efRepository.CreateAsync(
            customer, cancellationToken);

        await efRepository.DbContext
            .SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken)
    {
        efRepository.Update(customer);

        return efRepository.DbContext
            .SaveChangesAsync(cancellationToken);
    }

    public Task DeleteAsync(
        Customer customer,
        CancellationToken cancellationToken)
    {
        efRepository.Delete(customer);

        return efRepository.DbContext
            .SaveChangesAsync(cancellationToken);
    }
}
