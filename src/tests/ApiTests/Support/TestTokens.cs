using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace NttBankApi.Api.Tests.Support;

internal static class TestTokens
{
    public const string Issuer = "https://issuer.test/";

    public const string Audience = "ntt-bank-api-tests";

    public static readonly SymmetricSecurityKey SigningKey =
        new(Encoding.UTF8.GetBytes("test-signing-key-with-at-least-32-bytes!"));

    public static string Create(string scope)
    {
        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                ["sub"] = "test-user",
                ["scope"] = scope,
            },
            SigningCredentials = new SigningCredentials(
                SigningKey, SecurityAlgorithms.HmacSha256),
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}
