using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NttBankApi.Application.Features.Entities.GetCustomerById;

namespace NttBankApi.Api.Tests.Support;

internal sealed class CustomerApiFactory : WebApplicationFactory<Program>
{
    public Mock<IGetCustomerByIdUseCase> UseCase { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment("Development");
        builder.UseSetting("Database:ConnectionString",
            "Host=localhost;Database=tests;Username=tests;Password=tests");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IGetCustomerByIdUseCase>();
            services.AddSingleton(UseCase.Object);

            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Authority = null;
                    options.RequireHttpsMetadata = false;
                    options.Configuration = new OpenIdConnectConfiguration();
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = TestTokens.Issuer,
                            ValidAudience = TestTokens.Audience,
                            IssuerSigningKey = TestTokens.SigningKey,
                            ClockSkew = TimeSpan.Zero,
                        };
                });
        });
    }
}
