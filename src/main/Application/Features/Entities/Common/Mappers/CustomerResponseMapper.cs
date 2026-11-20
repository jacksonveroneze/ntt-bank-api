using Mapster;
using NttBankApi.Application.Features.Entities.Common.Models;
using NttBankApi.Domain.Entities;

namespace NttBankApi.Application.Features.Entities.Common.Mappers;

public sealed class CustomerResponseMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.NewConfig<Customer, CustomerResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email.Value)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.AccountType, src => src.AccountType)
            .Map(dest => dest.Status, src => src.Status);
    }
}
