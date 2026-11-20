using Microsoft.AspNetCore.Mvc;
using NttBankApi.Api.Endpoints.Extensions;
using NttBankApi.Application.Features.Entities.GetCustomerById;
using NttBankApi.Infrastructure.Security;

namespace NttBankApi.Api.Endpoints.Entities.v1;

internal static class GetCustomerByIdEndpoint
{
    extension(RouteGroupBuilder builder)
    {
        public RouteGroupBuilder AddGetById()
        {
            builder.MapGet("{id:int}", async (
                    [FromServices] IGetCustomerByIdUseCase useCase,
                    int id,
                    CancellationToken cancellationToken) =>
                {
                    GetCustomerByIdRequest input = new(id);

                    var output = await useCase.ExecuteAsync(
                        input, cancellationToken);

                    return output.ToIResult();
                })
                .WithName(RouteNames.GetCustomerById)
                .Produces<GetCustomerByIdResponse>()
                .AddDefaultResponseEndpoints()
                .RequireAuthorization(AuthorizationPolicies.EntitiesRead);

            return builder;
        }
    }
}
