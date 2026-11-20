using NttBankApi.Domain.Enums;

namespace NttBankApi.Application.Features.Entities.Common.Models;

public sealed record CustomerResponse(
    int Id,
    string Name,
    string Email,
    DateTimeOffset CreatedAt,
    AccountType AccountType,
    Status Status);
