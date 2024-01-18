// -----------------------------------------------------------------------
// <copyright file="ReportEntry.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.WebApi.Resource;

/// <summary>
/// A single entry of a report.
/// </summary>
public sealed record ReportEntry(
    string Date,
    int ProjectNumber,
    string CustomerName,
    string ProjectName,
    int SubprojectNumber,
    string SubprojectName,
    int ActivityNumber,
    string ActivityName,
    string BillabilityName,
    string UserName,
    long Duration,
    string Comment);
