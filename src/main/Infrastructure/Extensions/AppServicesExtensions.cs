using System.Diagnostics.CodeAnalysis;
using JacksonVeroneze.NET.EntityFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NttBankApi.Application.Abstractions.Repositories.Entities;
using NttBankApi.Infrastructure.Repositories.Entities;

namespace NttBankApi.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AppServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        #region Feature

        services.AddRepository();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        #endregion

        return services;
    }
}
