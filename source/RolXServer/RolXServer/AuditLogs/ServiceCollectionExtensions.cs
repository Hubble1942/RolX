// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.AuditLogs;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> instances.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the services of the audit logs package.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>
    /// The service collection.
    /// </returns>
    public static IServiceCollection AddAuditLogs(this IServiceCollection services)
    {
        services.AddScoped<Domain.IAuditLogService, Domain.Detail.AuditLogService>();
        return services;
    }
}
