using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NttBankApi.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record DatabaseConfiguration
{
    [Required]
    public required string ConnectionString { get; init; }
}
