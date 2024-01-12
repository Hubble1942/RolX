// -----------------------------------------------------------------------
// <copyright file="InjectCurrentUserFilter.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.Filters;
using RolXServer.AuditLogs.Domain;
using RolXServer.Auth.Domain;

namespace RolXServer.Common.WebApi;

/// <summary>
/// Action filter creating a database transaction per HTTP request.
/// </summary>
public sealed class InjectCurrentUserFilter : IAsyncActionFilter
{
    private readonly IAuditLogService auditLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InjectCurrentUserFilter"/> class.
    /// </summary>
    /// <param name="auditLogService">The audit log service.</param>
    public InjectCurrentUserFilter(IAuditLogService auditLogService)
    {
        this.auditLogService = auditLogService;
    }

    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        this.auditLogService.CurrentUser = context.HttpContext.User.TryGetUserId();

        await next();
    }
}
