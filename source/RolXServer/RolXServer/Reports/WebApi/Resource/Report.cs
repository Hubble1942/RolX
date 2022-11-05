// -----------------------------------------------------------------------
// <copyright file="Report.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Reports.WebApi.Resource;

/// <summary>
/// Defines a report.
/// </summary>
public sealed record Report(
    string Subproject,
    DateRange Range,
    IEnumerable<ReportEntry> Entries);
