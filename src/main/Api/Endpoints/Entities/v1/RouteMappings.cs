using NttBankApi.Api.Endpoints.Extensions;

namespace NttBankApi.Api.Endpoints.Entities.v1;

internal static class RouteMappings
{
    private const string Resource = "customers";

    private const int Version = 1;

    extension(WebApplication app)
    {
        public WebApplication AddEntitiesEndpoints()
        {
            RouteGroupBuilder builder = RouteGroupBuilderFactory
                .Factory(app, Resource, Version);

            builder.AddGetById();

            return app;
        }
    }
}
