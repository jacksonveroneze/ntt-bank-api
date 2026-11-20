using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NttBankApi.Infrastructure.Context.Configurations;

namespace NttBankApi.Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class DefaultDbContext(
    DbContextOptions<DefaultDbContext> options)
    : DbContext(options)
{
    private const string Schema = "bank";

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
