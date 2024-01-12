// -----------------------------------------------------------------------
// <copyright file="IAuditLogService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.AuditLogs.Domain;

/// <summary>
/// Provides access to audit logs.
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Gets or sets the current user making modifications.
    /// </summary>
    public Guid? CurrentUser { get; set; }

    /// <summary>
    /// Generate an audit log entry.
    /// </summary>
    /// <typeparam name="T">The type of the object which changed.</typeparam>
    /// <param name="oldValue">The value before the update.</param>
    /// <param name="newValue">The value after the update.</param>
    void GenerateAuditLog<T>(T? oldValue, T? newValue);
}
