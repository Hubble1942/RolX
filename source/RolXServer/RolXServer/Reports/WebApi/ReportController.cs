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
using RolXServer.Common.Util;
using RolXServer.Projects.Domain;
using RolXServer.Reports.Domain;
using RolXServer.Reports.Domain.Model;
using RolXServer.Reports.WebApi.Mapping;
using RolXServer.Reports.WebApi.Resource;

using ReportRangeType = RolXServer.Reports.Domain.Model.ReportRangeType;

namespace RolXServer.Reports.WebApi;

/// <summary>
/// Controller for exporting data.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Administrator, Backoffice, Supervisor", Policy = "ActiveUser")]
public class ReportController : ControllerBase
{
    private readonly IExportService exportService;
    private readonly ISubprojectService subprojectService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportController" /> class.
    /// </summary>
    /// <param name="exportService">The export service.</param>
    /// <param name="subprojectService">The subproject service.</param>
    public ReportController(IExportService exportService, ISubprojectService subprojectService)
    {
        this.exportService = exportService;
        this.subprojectService = subprojectService;
    }

    /// <summary>
    /// Gets a report of a subproject.
    /// </summary>
    /// <param name="subprojectId">The optional subproject identifier.</param>
    /// <param name="reportRangeType">The report range type.</param>
    /// <param name="customStart">The optional start iso date of a custom range.</param>
    /// <param name="customEnd">The optional end iso date of a custom range.</param>
    /// <returns>
    /// The report.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<Report>> GetSubprojectReport(int subprojectId, ReportRangeType reportRangeType, string? customStart, string? customEnd)
    {
        var subproject = await this.subprojectService.GetById(subprojectId);
        if (subproject == null)
        {
            return this.NotFound($"No subproject found with id {subprojectId}");
        }

        var reportRange = new ReportRange(reportRangeType, customStart == null ? null : IsoDate.Parse(customStart), customEnd == null ? null : IsoDate.Parse(customEnd));

        var export = await this.exportService.GetFor(reportRange.ToDateRange(DateOnly.FromDateTime(DateTime.Now), subproject?.StartDate), this.User.GetUserId(), subprojectId);
        return export.ToResource();
    }

    /// <summary>
    /// Gets all possible report range types.
    /// </summary>
    /// <returns>The report range types.</returns>
    [HttpGet("rangeTypes")]
    public IEnumerable<Resource.ReportRangeType> GetReportRangeTypes()
    => Enum.GetValues<ReportRangeType>().Select(t => new Resource.ReportRangeType(
        Enum.GetName(t) ?? "ID",
        t.ToLabel(),
        t == ReportRangeType.Custom,
        t == ReportRangeType.Custom,
        t == ReportRangeType.StartToMonth));
}
