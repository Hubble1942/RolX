// -----------------------------------------------------------------------
// <copyright file="LoggerConfigurationExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Serilog.Events;

namespace RolXServer.Common.Logging;

/// <summary>
/// Extension methods for <see cref="LoggerConfiguration"/> instances.
/// </summary>
public static class LoggerConfigurationExtensions
{
    private const string SingleLine = "[{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}";
    private const string MultiLine = "[{Level:u3}] ({SourceContext}){NewLine}{Message}{NewLine}{Exception}";
    private const string ShortTimestamp = "{Timestamp:HH:mm:ss.fff}";
    private const string LongTimestamp = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff}";

    /// <summary>
    /// Configures the specified logger configuration for RolX-style logging.
    /// </summary>
    /// <param name="loggerConfiguration">The logger configuration.</param>
    /// <returns>The configured logger configuration.</returns>
    public static LoggerConfiguration ConfigureForRolX(this LoggerConfiguration loggerConfiguration)
       => loggerConfiguration
           .MinimumLevel.Verbose()
           .WriteTo.Console(outputTemplate: $"    {LongTimestamp} {MultiLine}")
           .WriteTo.Debug(
               outputTemplate: $"{ShortTimestamp} {SingleLine}",
               restrictedToMinimumLevel: LogEventLevel.Information)
           .Enrich.FromLogContext();

    /// <summary>
    /// Configures the specified logger configuration for RolX-style logging.
    /// </summary>
    /// <param name="loggerConfiguration">The logger configuration.</param>
    /// <param name="applicationConfiguration">The application configuration.</param>
    /// <returns>
    /// The configured logger configuration.
    /// </returns>
    public static LoggerConfiguration ConfigureForRolX(this LoggerConfiguration loggerConfiguration, IConfiguration applicationConfiguration)
        => loggerConfiguration
            .ConfigureForRolX()
            .ReadFrom.Configuration(applicationConfiguration);
}
