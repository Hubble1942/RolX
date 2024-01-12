// -----------------------------------------------------------------------
// <copyright file="AuditLog.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.DataAccess;

namespace RolXServer.AuditLogs.DataAccess;

/// <summary>
/// An entry of the audit log.
/// </summary>
public sealed class AuditLog
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the Date and time at which the change was made.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets or sets a string with the values before the change.
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// Gets or sets a string with the values after the change.
    /// </summary>
    public string? NewValues { get; set; }
}
