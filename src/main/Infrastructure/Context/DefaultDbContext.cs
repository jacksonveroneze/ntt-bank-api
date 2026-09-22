using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace NttBankApi.Infrastructure.Context;

[ExcludeFromCodeCoverage]
public class DefaultDbContext(
    DbContextOptions<DefaultDbContext> options)
    : DbContext(options)
{
    private const string Schema = "schema";

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);
    }
}
