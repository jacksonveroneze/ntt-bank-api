using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace NttBankApi.ApiTests.Support;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string Issuer = "test-issuer";
    private const string Audience = "test-audience";

    private const int KeySizeInBytes = 32;

    private static readonly DateTime TokenExpiration =
        DateTime.UtcNow.AddHours(1);

    private readonly SymmetricSecurityKey _signingKey =
        new(RandomNumberGenerator.GetBytes(KeySizeInBytes));

    public string CreateToken(string scope)
    {
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = Issuer,
            Audience = Audience,
            Subject = new ClaimsIdentity([new Claim("scope", scope)]),
            Expires = TokenExpiration,
            SigningCredentials = new SigningCredentials(
                _signingKey, SecurityAlgorithms.HmacSha256),
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseSetting("Cache:UseDistributed", "false");

        builder.ConfigureTestServices(services =>
        {

            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.ConfigurationManager =
                        new StaticConfigurationManager<OpenIdConnectConfiguration>(
                            new OpenIdConnectConfiguration());

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = _signingKey,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
        });
    }
}
