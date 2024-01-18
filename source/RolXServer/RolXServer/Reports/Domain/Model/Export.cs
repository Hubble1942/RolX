// -----------------------------------------------------------------------
// <copyright file="Export.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// Holds the data being exported.
/// </summary>
public sealed record Export(
    string Subproject,
    DateRange Range,
    string Creator,
    DateTime CreationDate,
    IEnumerable<ExportEntry> Entries);
