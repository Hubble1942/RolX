// -----------------------------------------------------------------------
// <copyright file="IExportService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Reports.Domain.Model;

namespace RolXServer.Reports.Domain;

/// <summary>
/// Provides data for exporting.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Gets the export for the specified range and subproject.
    /// </summary>
    /// <param name="creatorId">The creator of the export.</param>
    /// <param name="filter">The filter.</param>
    /// <returns>
    /// The data.
    /// </returns>
    Task<Export> GetFor(Guid creatorId, ExportFilter filter);
}
