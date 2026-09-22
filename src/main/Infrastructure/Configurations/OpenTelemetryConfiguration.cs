using System.Diagnostics.CodeAnalysis;

namespace NttBankApi.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record OpenTelemetryConfiguration
{
    public Uri? EndpointTracing { get; init; }
}
