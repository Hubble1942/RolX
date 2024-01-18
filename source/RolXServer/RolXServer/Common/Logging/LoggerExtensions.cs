// -----------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

using Serilog.Events;

namespace RolXServer.Common.Logging;

/// <summary>
/// Extension methods for <see cref="ILogger"/> implementations.
/// </summary>
public static class LoggerExtensions
{
    private const string Stars = "***************************************************************************************************";

    /// <summary>
    /// Logs the start of the applications to the specified logger.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="applicationAssembly">The main application assembly.</param>
    /// <returns>The <see cref="ILogger"/> so that additional calls can be chained.</returns>
    public static ILogger ApplicationStart(this ILogger logger, Assembly applicationAssembly)
    {
        logger.Information(Stars);
        logger.Information("*** Starting {application}", applicationAssembly.GetName().FullName);
        logger.Information(Stars);

        return logger;
    }

    /// <summary>
    /// Logs the end of the applications to the specified logger.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The <see cref="ILogger"/> so that additional calls can be chained.</returns>
    public static ILogger ApplicationEnd(this ILogger logger)
    {
        logger.Information(Stars);
        logger.Information("*** Application shut down gracefully");
        logger.Information(Stars);

        return logger;
    }

    /// <summary>
    /// Logs the ungraceful end of the applications to the specified logger.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="exception">The exception.</param>
    /// <returns>
    /// The <see cref="ILogger" /> so that additional calls can be chained.
    /// </returns>
    public static ILogger UngracefulShutdown(this ILogger logger, Exception? exception = null)
    {
        logger.Fatal(Stars);
        logger.Fatal("*** Application shut down ungracefully");
        logger.Fatal(exception, Stars);

        return logger;
    }

    /// <summary>
    /// Initiates the logging of the caller method.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="logEventLevel">The log event level.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <returns>The method logger.</returns>
    public static MethodLogger Method(this ILogger logger, LogEventLevel logEventLevel = LogEventLevel.Information, [CallerMemberName] string methodName = "")
        => new(logger, methodName, logEventLevel);
}
