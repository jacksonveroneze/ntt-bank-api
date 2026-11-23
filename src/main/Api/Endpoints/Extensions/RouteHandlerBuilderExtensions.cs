namespace NttBankApi.Api.Endpoints.Extensions;

internal static class RouteHandlerBuilderExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder AddDefaultResponseEndpoints()
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
