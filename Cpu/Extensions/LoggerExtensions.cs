﻿using Microsoft.Extensions.Logging;

namespace Cpu.Extensions;

/// <summary>
/// Extensions for <see cref="ILogger"/> interface
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Logs a message using an Action with data for increased performance
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> to be extended</param>
    /// <param name="logMessage"><see cref="Action{T1, T2, T3}"/> performing the message</param>
    /// <param name="data">Data to pass to the message</param>
    /// <param name="ex"><see cref="Exception"/> content for the message. May be null</param>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging"/>
    public static void LogAction(this ILogger logger,
        Action<ILogger, object, Exception?> logMessage,
        object data,
        Exception? ex = default)
    {
        ArgumentNullException.ThrowIfNull(logMessage, nameof(logMessage));
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        logMessage(logger, data, ex);
    }

    /// <summary>
    /// Logs a message using an Action for increased performance
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> to be extended</param>
    /// <param name="logMessage"><see cref="Action{T1, T2, T3}"/> performing the message</param>
    /// <param name="ex"><see cref="Exception"/> content for the message. May be null</param>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging"/>
    public static void LogAction(this ILogger logger,
        Action<ILogger, Exception?> logMessage,
        Exception? ex = default)
    {
        ArgumentNullException.ThrowIfNull(logMessage, nameof(logMessage));
        logMessage(logger, ex);
    }

    /// <summary>
    /// Logs a message using an Action with multiple data for increased performance
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> to be extended</param>
    /// <param name="logMessage"><see cref="Action{T1, T2, T3}"/> performing the message</param>
    /// <param name="data">Multiple data to pass to the message</param>
    /// <param name="ex"><see cref="Exception"/> content for the message. May be null</param>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging"/>
    public static void LogAction(this ILogger logger,
        Action<ILogger, object, object, Exception?> logMessage,
        Exception? ex = default,
        params object[] data)
    {
        ArgumentNullException.ThrowIfNull(logMessage, nameof(logMessage));
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        logMessage(logger, data[0], data[1], ex);
    }
}
