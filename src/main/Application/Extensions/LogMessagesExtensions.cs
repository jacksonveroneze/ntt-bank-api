using Microsoft.Extensions.Logging;

namespace NttBankApi.Application.Extensions;

public static partial class LogMessagesExtensions
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Warning,
        Message = "{className} - {methodName} - " +
                  "Identifier: '{identifier}' - NotFound")]
    public static partial void LogNotFound(this ILogger logger,
        string className, string methodName, int identifier);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Warning,
        Message = "{className} - {methodName} - " +
                  "Identifier: '{identifier}' - " +
                  "SecondaryIdentifier: '{secondaryIdentifier}' - NotFound")]
    public static partial void LogNotFound(this ILogger logger,
        string className, string methodName,
        int identifier, int secondaryIdentifier);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - " +
                  "Identifier: '{identifier}' - Count: {count}")]
    public static partial void LogCollectionResult(this ILogger logger,
        string className, string methodName, int identifier, int count);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Count: {count}")]
    public static partial void LogCollectionResult(this ILogger logger,
        string className, string methodName, int count);
}
