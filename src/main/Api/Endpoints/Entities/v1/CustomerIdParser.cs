using System.Globalization;
using JacksonVeroneze.NET.Result;

namespace NttBankApi.Api.Endpoints.Entities.v1;

internal static class CustomerIdParser
{
    private static readonly Error InvalidId =
        Error.Create("Customer.IdInvalid",
            "The customer identifier must be a positive integer.");

    public static bool TryParse(
        string? value,
        out int id)
    {
        bool parsed = int.TryParse(
            value,
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out id);

        return parsed && id > 0;
    }

    public static Result<T> ToInvalidResult<T>()
    {
        return Result<T>.FromInvalid(InvalidId);
    }
}
