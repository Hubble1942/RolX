// -----------------------------------------------------------------------
// <copyright file="SubprojectMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

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
            CustomerName: domain.CustomerName,
            ProjectNumber: domain.ProjectNumber,
            ProjectName: domain.ProjectName,
            Number: domain.Number,
            Name: domain.Name,
            ManagerId: domain.ManagerId,
            ManagerName: domain.Manager?.FullName() ?? "vakant",
            Budget: (long)domain.Budget.TotalSeconds,
            Planned: (long)domain.Planned.TotalSeconds,
            Actual: (long)domain.Actual.TotalSeconds,
            IsOverBudget: domain.IsOverBudget,
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
        };

        domain.Activities = resource.Activities
                .Select(a => a.ToDomain(domain))
                .ToList();

        return domain;
    }
}
