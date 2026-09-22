using System.Diagnostics.CodeAnalysis;

namespace NttBankApi.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AppConfiguration
{
    public required AppInfoConfiguration Application { get; init; }
    
    public required DatabaseConfiguration Database { get; init; }

    public required CacheConfiguration Cache { get; init; }

    public required OpenTelemetryConfiguration OpenTelemetry { get; init; }
}
