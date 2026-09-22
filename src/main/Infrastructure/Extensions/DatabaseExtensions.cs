using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NttBankApi.Infrastructure.Configurations;
using NttBankApi.Infrastructure.Context;

namespace NttBankApi.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class DatabaseExtensions
{
    private const int DefaultCommandTimeout = 5;

    extension(IServiceCollection services)
    {
        public IServiceCollection AddDatabase(
            AppConfiguration appConfiguration,
            IHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(appConfiguration);

            services.InternalAddDatabase<DefaultDbContext>(
                appConfiguration.Database.ConnectionString,
                environment);

            return services;
        }

        private IServiceCollection InternalAddDatabase<TContext>(
            string connectionString,
            IHostEnvironment environment,
            QueryTrackingBehavior behavior = QueryTrackingBehavior.NoTracking)
            where TContext : DbContext
        {
            ArgumentException.ThrowIfNullOrEmpty(connectionString);

            services.AddDbContext<TContext>((_, options) =>
            {
                options.UseNpgsql(connectionString, conf =>
                    {
                        conf.EnableRetryOnFailure()
                            .CommandTimeout(DefaultCommandTimeout);
                    })
                    .UseQueryTrackingBehavior(behavior)
                    .ConfigureOptionsDatabase(environment);
            });

            return services;
        }
    }

    private static void ConfigureOptionsDatabase(
        this DbContextOptionsBuilder optionsBuilder,
        IHostEnvironment environment)
    {
        optionsBuilder
            .EnableDetailedErrors(environment.IsDevelopment())
            .EnableSensitiveDataLogging(environment.IsDevelopment())
            .EnableThreadSafetyChecks()
            .UseSnakeCaseNamingConvention();
    }
}
