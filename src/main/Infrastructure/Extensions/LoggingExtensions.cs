using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using NttBankApi.Infrastructure.Configurations;
using Serilog;

namespace NttBankApi.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLogger(
        this WebApplicationBuilder builder,
        AppConfiguration appConfiguration)
    {
        builder.Host.UseSerilog((hostingContext,
            services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName",
                    appConfiguration.Application.Name)
                .Enrich.WithProperty("ApplicationVersion",
                    appConfiguration.Application.Version.ToString());
        });
        
        return builder;
    }
}
