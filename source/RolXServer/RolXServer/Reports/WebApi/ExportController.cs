// -----------------------------------------------------------------------
// <copyright file="ExportController.cs" company="Christian Ewald">
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
using RolXServer.Reports.WebApi.Mapping;

namespace RolXServer.Reports.WebApi;

/// <summary>
/// Controller for exporting data.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Administrator, Backoffice, Supervisor", Policy = "ActiveUser")]
public class ExportController : ControllerBase
{
    private readonly IExportService exportService;
    private readonly ISubprojectService subprojectService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportController" /> class.
    /// </summary>
    /// <param name="exportService">The export service.</param>
    /// <param name="subprojectService">The subproject service.</param>
    public ExportController(IExportService exportService, ISubprojectService subprojectService)
    {
        this.exportService = exportService;
        this.subprojectService = subprojectService;
    }

    /// <summary>
    /// Gets an Excel workbook report containing the requested data.
    /// </summary>
    /// <param name="subprojectId">The optional subproject identifier.</param>
    /// <param name="month">The optional month.</param>
    /// <param name="begin">The optional begin date.</param>
    /// <param name="end">The optional end date.</param>
    /// <returns>
    /// The report.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetExcel(int? subprojectId, string? month, string? begin, string? end)
    {
        Projects.Domain.Model.Subproject? subproject = null;
        if (subprojectId.HasValue)
        {
            subproject = await this.subprojectService.GetById(subprojectId.Value);
            if (subproject == null)
            {
                return this.NotFound($"No subproject found with id {subprojectId}");
            }
        }

        var range = TryEvaluateRange(month, begin, end);
        if (!range.HasValue)
        {
            return this.BadRequest("A month or begin and end dates must be provided");
        }

        return this.ExcelExport(
            await this.exportService.GetFor(range.Value, this.User.GetUserId(), subprojectId),
            GetFileName(month, begin, end, subproject));
    }

    private static DateRange? TryEvaluateRange(string? month, string? begin, string? end)
    {
        if (month != null && IsoDate.TryParseMonth(month, out var monthDate))
        {
            return DateRange.ForMonth(monthDate);
        }

        if (begin != null && end != null
            && IsoDate.TryParse(begin, out var beginDate)
            && IsoDate.TryParse(end, out var endDate)
            && beginDate <= endDate)
        {
            return new DateRange(beginDate, endDate);
        }

        return null;
    }

    private static string GetFileName(string? month, string? begin, string? end, Projects.Domain.Model.Subproject? subproject)
    {
        if (end != null)
        {
            end = IsoDate.Parse(end).AddDays(-1).ToIsoDate();
        }

        var subprojectPart = subproject != null ? subproject.FullNumber : "all";
        var rangePart = month != null
            ? month
            : begin != null && end != null
                ? $"{begin}-{end}"
                : string.Empty;

        return $"rolx-{subprojectPart}-{rangePart}.xlsx";
    }
}
