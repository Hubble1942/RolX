// -----------------------------------------------------------------------
// <copyright file="ExportToReport.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;
using RolXServer.Reports.Domain.Model;
using RolXServer.Reports.WebApi.Resource;

namespace RolXServer.Reports.WebApi.Mapping;

/// <summary>
/// Maps exports to report resources.
/// </summary>
internal static class ExportToReport
{
    /// <summary>
    /// Converts export to report.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static Report ToResource(this Export domain)
        => new(
            Subproject: domain.Subproject,
            StartDate: domain.Range.Begin.ToIsoDate(),
            EndDate: domain.Range.End.ToIsoDate(),
            ReportEntries: domain.Entries.Select(e => e.ToResource()).ToImmutableList());

    /// <summary>
    /// Converts export entry to resource entry.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static ReportEntry ToResource(this ExportEntry domain)
        => new(
            Date: domain.Date.ToIsoDate(),
            ProjectNumber: domain.ProjectNumber,
            CustomerName: domain.CustomerName,
            ProjectName: domain.ProjectName,
            SubprojectNumber: domain.SubprojectNumber,
            SubprojectName: domain.SubprojectName,
            ActivityNumber: domain.ActivityNumber,
            ActivityName: domain.ActivityName,
            BillabilityName: domain.BillabilityName,
            UserName: domain.UserName,
            Duration: (long)domain.Duration.TotalSeconds,
            Comment: domain.Comment);
}
