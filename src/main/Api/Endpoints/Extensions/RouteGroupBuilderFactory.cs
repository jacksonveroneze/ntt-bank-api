namespace NttBankApi.Api.Endpoints.Extensions;

internal static class RouteGroupBuilderFactory
{
    public static RouteGroupBuilder Factory(
        WebApplication app,
        string resource,
        int version)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);

        return app.MapGroup($"/{resource}")
            .WithGroupName($"v{version}");
    }
}
