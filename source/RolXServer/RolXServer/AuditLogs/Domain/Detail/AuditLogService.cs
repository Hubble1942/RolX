// -----------------------------------------------------------------------
// <copyright file="AuditLogService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using RolXServer.AuditLogs.DataAccess;

namespace RolXServer.AuditLogs.Domain.Detail;

/// <inheritdoc/>
public class AuditLogService : IAuditLogService
{
    private readonly RolXContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public AuditLogService(RolXContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public Guid? CurrentUser { get; set; }

    /// <inheritdoc/>
    /// <remarks>The audit log is added to the context, but the context is not saved to the DB.</remarks>
    public void GenerateAuditLog<T>(T? oldValue, T? newValue)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        AuditLog auditLog = new()
        {
            UserId = this.CurrentUser,
            DateTime = DateTime.UtcNow,
            OldValues = JsonConvert.SerializeObject(oldValue, jsonSettings),
            NewValues = JsonConvert.SerializeObject(newValue, jsonSettings),
        };

        this.dbContext.AuditLogs.Add(auditLog);
    }
}
