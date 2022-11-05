// -----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RolXServer.Auth.Domain;
using RolXServer.Reports.Domain;
using RolXServer.Reports.WebApi.Mapping;
using RolXServer.Reports.WebApi.Resource;

namespace RolXServer.Reports.WebApi;

/// <summary>
/// Controller for exporting project reports.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Administrator, Backoffice, Supervisor", Policy = "ActiveUser")]
public class ReportController : ControllerBase
{
    private readonly IExportService exportService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportController"/> class.
    /// </summary>
    /// <param name="exportService">The report service.</param>
    public ReportController(IExportService exportService)
    {
        this.exportService = exportService;
    }

    /// <summary>
    /// Returns a report for the given <paramref name="reportFilter"/>.
    /// </summary>
    /// <param name="reportFilter">The filter for the report</param>
    /// <returns>A report.</returns>
    [HttpGet]
    public async Task<ActionResult<Report>> GetReport(ReportFilter reportFilter)
    {
        var domainExport = await this.exportService.GetFor(this.User.GetUserId(), reportFilter.ToDomain());
        return domainExport.ToResource();
    }
}
