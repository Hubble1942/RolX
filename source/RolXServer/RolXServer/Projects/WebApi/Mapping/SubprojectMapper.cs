// -----------------------------------------------------------------------
// <copyright file="SubprojectMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;
using RolXServer.Users.Domain;

namespace RolXServer.Projects.WebApi.Mapping;

/// <summary>
/// Maps subprojects from / to resource.
/// </summary>
internal static class SubprojectMapper
{
    /// <summary>
    /// Converts to resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static Resource.Subproject ToResource(this Domain.Model.Subproject domain)
        => new(
            Id: domain.Id,
            FullNumber: domain.FullNumber,
            FullName: domain.FullName,
            StartDate: domain.StartDate?.ToIsoDate(),
            CustomerName: domain.CustomerName,
            ProjectNumber: domain.ProjectNumber,
            ProjectName: domain.ProjectName,
            Number: domain.Number,
            Name: domain.Name,
            ManagerId: domain.ManagerId,
            ManagerName: domain.Manager?.FullName() ?? "vakant",
            DeputyManagerId: domain.DeputyManagerId,
            DeputyManagerName: domain.DeputyManager?.FullName(),
            ArchitectId: domain.ArchitectId,
            ArchitectName: domain.Architect?.FullName(),
            Budget: (long)domain.Budget.TotalSeconds,
            Planned: (long)domain.Planned.TotalSeconds,
            Actual: (long)domain.Actual.TotalSeconds,
            BudgetConsumedFraction: domain.BudgetConsumedFraction ?? 0,
            PlannedConsumedFraction: domain.PlannedConsumedFraction ?? 0,
            IsOverBudget: domain.IsOverBudget,
            IsOverPlanned: domain.IsOverPlanned,
            IsClosed: domain.IsClosed,
            Activities: domain.Activities.Select(a => a.ToResource()).ToImmutableList());

    /// <summary>
    /// Converts to shallow resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static Resource.SubprojectShallow ToShallowResource(this Domain.Model.Subproject domain)
        => new(
            Id: domain.Id,
            FullNumber: domain.FullNumber,
            CustomerName: domain.CustomerName,
            ProjectName: domain.ProjectName,
            Name: domain.Name,
            ManagerName: domain.Manager?.FullName() ?? "vakant",
            DeputyManagerName: domain.DeputyManager?.FullName(),
            ArchitectName: domain.Architect?.FullName(),
            IsClosed: domain.IsClosed);

    /// <summary>
    /// Converts to domain.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static Domain.Model.Subproject ToDomain(this Resource.Subproject resource)
    {
        var domain = new Domain.Model.Subproject()
        {
            Id = resource.Id,
            Number = resource.Number,
            Name = resource.Name,
            ProjectNumber = resource.ProjectNumber,
            ProjectName = resource.ProjectName,
            CustomerName = resource.CustomerName,
            ManagerId = resource.ManagerId,
            DeputyManagerId = resource.DeputyManagerId,
            ArchitectId = resource.ArchitectId,
        };

        domain.Activities = resource.Activities
                .Select(a => a.ToDomain(domain))
                .ToList();

        return domain;
    }
}
