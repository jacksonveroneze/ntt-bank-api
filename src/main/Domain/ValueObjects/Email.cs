using System.Text.RegularExpressions;
using JacksonVeroneze.NET.Result;

namespace NttBankApi.Domain.ValueObjects;

public sealed partial record Email
{
    private const int MaxLength = 254;

    private static readonly Error Invalid =
        Error.Create("Email.Invalid",
            "Email is invalid.");

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string value)
    {
        string trimmed = value?.Trim() ?? string.Empty;

        if (trimmed.Length == 0 || trimmed.Length > MaxLength)
        {
            return Result<Email>.FromInvalid(Invalid);
        }

        if (!EmailRegex().IsMatch(trimmed))
        {
            return Result<Email>.FromInvalid(Invalid);
        }

        return Result<Email>.WithSuccess(
            new Email(trimmed.ToLowerInvariant()));
    }

    public override string ToString()
    {
        int atIndex = Value.IndexOf('@', StringComparison.Ordinal);

        return $"{Value[0]}***{Value[atIndex..]}";
    }

    [GeneratedRegex(
        @"\A[A-Za-z0-9._%+-]{1,64}@"
        + @"(?:[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?\.){1,8}[A-Za-z]{2,63}\z",
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 100)]
    private static partial Regex EmailRegex();
}
