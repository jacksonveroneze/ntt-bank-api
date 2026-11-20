namespace NttBankApi.Api.Endpoints.Extensions;

internal static class DefaultResponsesExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        public RouteHandlerBuilder AddDefaultResponseEndpoints()
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .ProducesProblem(StatusCodes.Status403Forbidden)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
