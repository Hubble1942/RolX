// -----------------------------------------------------------------------
// <copyright file="ExportFilter.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// Filter for creating <see cref="Export"/>.
/// </summary>
/// <param name="Range">The date range.</param>
/// <param name="ProjectNumber">The project number. Any if <see langword="null"/>.</param>
/// <param name="SubprojectNumber">The subproject number. Any if <see langword="null"/>.</param>
/// <param name="UserIds">The user ids for which to filter. Any if empty.</param>
/// <param name="CommentFilter">The comment filter. Any if <see langword="null"/>.</param>
public sealed record ExportFilter(DateRange Range,
    int? ProjectNumber,
    int? SubprojectNumber,
    IReadOnlyList<Guid> UserIds,
    string? CommentFilter);
