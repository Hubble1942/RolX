// -----------------------------------------------------------------------
// <copyright file="ExceptionHandlerMiddleware.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Common.Errors;

/// <summary>
/// Middleware handling common error exceptions.
/// </summary>
internal class ExceptionHandlerMiddleware
{
    private static readonly ILogger Logger = Log.ForContext<ExceptionHandlerMiddleware>();

    private readonly RequestDelegate next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next.</param>
    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Invokes the specified HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>The async task.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await this.next(httpContext);
        }
        catch (ItemNotFoundException e)
        {
            Logger.Warning(e, e.Message);

            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsync(e.Message);
        }
    }
}
