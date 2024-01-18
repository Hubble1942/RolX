// -----------------------------------------------------------------------
// <copyright file="ReportRangeType.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.WebApi.Resource;

/// <summary>
/// The type of a reports range.
/// </summary>
public record ReportRangeType(string Id, string Label, bool HasCustomStart, bool HasCustomEnd, bool HasCustomEndMonth);
