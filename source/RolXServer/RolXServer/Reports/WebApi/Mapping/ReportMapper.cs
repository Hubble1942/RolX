// -----------------------------------------------------------------------
// <copyright file="ReportMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Reports.Domain.Model;
using RolXServer.Reports.WebApi.Resource;

namespace RolXServer.Reports.WebApi.Mapping;

/// <summary>
/// Maps for <see cref="Export"/> to <see cref="Report"/>.
/// </summary>
public static class ReportMapper
{
    /// <summary>
    /// Maps <paramref name="domainExport"/> to <see cref="Report"/>.
    /// </summary>
    /// <param name="domainExport">The domain export</param>
    /// <returns>A resource export</returns>
    public static Report ToResource(this Export domainExport) =>
        new(domainExport.Subproject, domainExport.Range, domainExport.Entries.ToResources());

    /// <summary>
    /// Maps <paramref name="reportFilter"/> to <see cref="ExportFilter"/>.
    /// </summary>
    /// <param name="reportFilter">The report filter.</param>
    /// <returns>An export filter.</returns>
    public static ExportFilter ToDomain(this ReportFilter reportFilter) =>
        new(
            reportFilter.DateRange,
            reportFilter.ProjectNumber,
            reportFilter.SubprojectNumber,
            reportFilter.UserIds.ToArray(),
            reportFilter.CommentFilter);

    private static IEnumerable<ReportEntry> ToResources(
        this IEnumerable<ExportEntry> domainEntries) =>
        domainEntries.Select(ToResource).ToArray();

    private static ReportEntry ToResource(ExportEntry domainEntry) =>
        new(
            domainEntry.Date,
            domainEntry.ProjectNumber,
            domainEntry.CustomerName,
            domainEntry.ProjectName,
            domainEntry.SubprojectNumber,
            domainEntry.SubprojectName,
            domainEntry.ActivityNumber,
            domainEntry.ActivityName,
            domainEntry.BillabilityName,
            domainEntry.UserName,
            domainEntry.Duration,
            domainEntry.Comment);

}
