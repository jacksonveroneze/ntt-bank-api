using JacksonVeroneze.NET.Result;
using NttBankApi.Domain.Enums;
using NttBankApi.Domain.ValueObjects;

namespace NttBankApi.Domain.Entities;

public sealed class Customer
{
    private const int NameMaxLength = 256;

    private static readonly Error NameInvalid =
        Error.Create("Customer.NameInvalid",
            "Customer name is required and must have at most 256 characters.");

    public int Id { get; }

    public string Name { get; }

    public Email Email { get; }

    public DateTimeOffset CreatedAt { get; }

    public AccountType AccountType { get; }

    public Status Status { get; }

    private Customer(
        int id,
        string name,
        Email email,
        DateTimeOffset createdAt,
        AccountType accountType,
        Status status)
    {
        Id = id;
        Name = name;
        Email = email;
        CreatedAt = createdAt;
        AccountType = accountType;
        Status = status;
    }

    public static Result<Customer> Create(
        string name,
        Email email,
        DateTimeOffset createdAt,
        AccountType accountType,
        Status status)
    {
        string trimmedName = name?.Trim() ?? string.Empty;

        if (trimmedName.Length == 0 || trimmedName.Length > NameMaxLength)
        {
            return Result<Customer>.FromInvalid(NameInvalid);
        }

        return Result<Customer>.WithSuccess(
            new Customer(0, trimmedName, email, createdAt, accountType, status));
    }
}
