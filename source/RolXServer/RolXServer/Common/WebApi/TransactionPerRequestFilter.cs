// -----------------------------------------------------------------------
// <copyright file="TransactionPerRequestFilter.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Data;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace RolXServer.Common.WebApi;

/// <summary>
/// Action filter creating a database transaction per HTTP request.
/// </summary>
public sealed class TransactionPerRequestFilter : IAsyncActionFilter
{
    private static readonly HashSet<string> WriteMethods = new HashSet<string>
    {
        "POST",
        "PUT",
        "DELETE",
    };

    private readonly RolXContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionPerRequestFilter"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public TransactionPerRequestFilter(RolXContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isWriteRequest = WriteMethods.Contains(context.HttpContext.Request.Method);

        await using var transaction = await this.dbContext.Database.BeginTransactionAsync(
            isWriteRequest ? IsolationLevel.Serializable : IsolationLevel.RepeatableRead);

        var executedContext = await next();

        if (isWriteRequest && WasSuccessfulExecution(executedContext))
        {
            await transaction.CommitAsync();
        }
    }

    private static bool WasSuccessfulExecution(ActionExecutedContext executedContext)
    {
        if (executedContext.Exception != null)
        {
            return false;
        }

        var statusCode = (executedContext.Result as IStatusCodeActionResult)?.StatusCode
            ?? (executedContext.Result as ObjectResult)?.StatusCode
            ?? (int)HttpStatusCode.OK;

        var explicitlyAllowedCodes = executedContext.ActionDescriptor.EndpointMetadata
            .OfType<NonFailureHttpCodeAttribute>()
            .Select(a => a.StatusCode);

        return statusCode < (int)HttpStatusCode.BadRequest || explicitlyAllowedCodes.Contains(statusCode);
    }
}
