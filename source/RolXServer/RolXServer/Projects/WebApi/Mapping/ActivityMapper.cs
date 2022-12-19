// -----------------------------------------------------------------------
// <copyright file="ActivityMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Projects.WebApi.Mapping;

/// <summary>
/// Maps activities from / to resource.
/// </summary>
internal static class ActivityMapper
{
    /// <summary>
    /// Converts to resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static Resource.Activity ToResource(this Domain.Model.Activity domain)
        => new(
            Id: domain.Id,
            Number: domain.Number,
            Name: domain.Name,
            StartDate: domain.StartDate.ToIsoDate(),
            EndDate: domain.EndedDate?.AddDays(-1).ToIsoDate(),
            BillabilityId: domain.Billability.Id,
            BillabilityName: domain.Billability.Name,
            IsBillable: domain.Billability.IsBillable,
            IsOverBudget: domain.IsOverBudget,
            IsOverPlanned: domain.IsOverPlanned,
            Budget: (long)(domain.Budget?.TotalSeconds ?? 0),
            Planned: (long)(domain.Planned?.TotalSeconds ?? 0),
            Actual: (long)(domain.Actual?.TotalSeconds ?? 0),
            BudgetConsumedFraction: domain.BudgetConsumedFraction ?? 0,
            PlannedConsumedFraction: domain.PlannedConsumedFraction ?? 0,
            ProjectName: domain.Subproject?.ProjectName ?? string.Empty,
            SubprojectName: domain.Subproject?.Name ?? string.Empty,
            CustomerName: domain.Subproject?.CustomerName ?? string.Empty,
            FullNumber: domain.FullNumber,
            FullName: domain.FullName,
            AllSubprojectNames: domain.Subproject?.AllNames ?? string.Empty);

    /// <summary>
    /// Converts to domain.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="subproject">The subproject.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static Domain.Model.Activity ToDomain(this Resource.Activity resource, Domain.Model.Subproject? subproject = null) =>
        new()
        {
            Id = resource.Id,
            Number = resource.Number,
            Name = resource.Name,
            StartDate = IsoDate.Parse(resource.StartDate),
            EndedDate = IsoDate.ParseNullable(resource.EndDate)?.AddDays(1),
            Budget = TimeSpan.FromSeconds(resource.Budget),
            Planned = TimeSpan.FromSeconds(resource.Planned),
            Actual = TimeSpan.FromSeconds(resource.Actual),
            Subproject = subproject,
            Billability = new() { Id = resource.BillabilityId },
        };
}
