using System.Diagnostics.CodeAnalysis;

namespace NttBankApi.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record CacheConfiguration
{
    public bool UseDistributed { get; init; }

    public bool UseSsl { get; init; }

    public string? Endpoint { get; init; }
}
