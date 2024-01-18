// -----------------------------------------------------------------------
// <copyright file="Report.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.WebApi.Resource;

/// <summary>
/// The content of a report.
/// </summary>
public sealed record Report(
    string Subproject,
    string StartDate,
    string EndDate,
    ImmutableList<ReportEntry> ReportEntries);
