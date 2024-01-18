// -----------------------------------------------------------------------
// <copyright file="WebApplicationExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace RolXServer.Common.Startup;

/// <summary>
/// Extension methods for <see cref="WebApplication"/> instances.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Migrates the database to the current state.
    /// </summary>
    /// <param name="webApplication">The web application.</param>
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RolXContext>();
        context.Database.Migrate();
    }

    /// <summary>
    /// Adds a fallback-route to the specified file for failing non-API requests.
    /// </summary>
    /// <param name="endpointRouteBuilder">The endpoint route builder.</param>
    /// <param name="filePath">The file path.</param>
    public static void MapFallbackFileForNonApiRequests(this WebApplication endpointRouteBuilder, string filePath)
    {
        endpointRouteBuilder.Map("api/{**slug}", context =>
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        });

        endpointRouteBuilder.MapFallbackToFile(filePath, new StaticFileOptions
        {
            OnPrepareResponse = ctx => ctx.Context.Response.SetNoCacheHeaders(),
        });
    }

    /// <summary>
    /// Sets the HTTP headers to prevent caching of the specified response.
    /// </summary>
    /// <param name="response">The response.</param>
    public static void SetNoCacheHeaders(this HttpResponse response)
    {
        response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
        response.Headers[HeaderNames.Expires] = "0";
        response.Headers[HeaderNames.Pragma] = "no-cache";
    }
}
