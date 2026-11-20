using Mapster;
using NttBankApi.Domain.Entities;

namespace NttBankApi.Application.Features.Entities.GetCustomerById;

public sealed class GetCustomerByIdMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.NewConfig<Customer, GetCustomerByIdResponse>()
            .Map(dest => dest.Data, src => src);
    }
}
