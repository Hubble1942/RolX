// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using RolXServer;
using RolXServer.AuditLogs;
using RolXServer.Auth;
using RolXServer.Common.Errors;
using RolXServer.Common.Logging;
using RolXServer.Common.Startup;
using RolXServer.Common.Util;
using RolXServer.Common.WebApi;
using RolXServer.Projects;
using RolXServer.Records;
using RolXServer.Reports;
using RolXServer.Users;

Log.Logger = new LoggerConfiguration()
            .ConfigureForRolX()
            .CreateBootstrapLogger()
            .ApplicationStart(Assembly.GetExecutingAssembly());

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) => configuration.ConfigureForRolX(context.Configuration));

    builder.Services
        .AddControllers(o =>
        {
            o.Filters.Add<NotFoundExceptionFilter>();
            o.Filters.Add<TransactionPerRequestFilter>();
            o.Filters.Add<InjectCurrentUserFilter>();
        })
        .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new TimeSpanJsonSecondsConverter()));

    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    var connectionString = builder.Configuration.GetConnectionString("RolXContext");
    builder.Services.AddDbContextPool<RolXContext>(provider =>
        provider.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

    builder.Services.AddProjects();
    builder.Services.AddAuth(builder.Configuration);
    builder.Services.AddWorkRecord(builder.Configuration);
    builder.Services.AddReports();
    builder.Services.AddUserManagement();
    builder.Services.AddAuditLogs();

    var app = builder.Build();

    app.MigrateDatabase();

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseExceptionHandlerMiddleware();
    app.UseRouting();

    // Add CORS policy for development
    app.UseCors(builder => builder
        .WithOrigins("http://localhost:4200")
        .WithExposedHeaders("content-disposition")
        .AllowAnyHeader()
        .AllowAnyMethod());

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapFallbackFileForNonApiRequests("index.html");

    app.Run();

    Log.Logger.ApplicationEnd();
}
catch (Exception exception)
{
    Log.Logger.UngracefulShutdown(exception);
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    Log.CloseAndFlush();
}
