// -----------------------------------------------------------------------
// <copyright file="SubprojectMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Projects.Domain.Mapping;

/// <summary>
/// Maps subprojects from / to model.
/// </summary>
public static class SubprojectMapper
{
    /// <summary>
    /// Converts to entity.
    /// </summary>
    /// <param name="domain">The Domain.Model subproject.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static DataAccess.Subproject ToEntity(this Model.Subproject domain)
    {
        var entity = new DataAccess.Subproject
        {
            Id = domain.Id,
            Number = domain.Number,
            Name = domain.Name,
            ProjectNumber = domain.ProjectNumber,
            ProjectName = domain.ProjectName,
            CustomerName = domain.CustomerName,
            ManagerId = domain.ManagerId,
            Manager = domain.Manager,
            DeputyManagerId = domain.DeputyManagerId,
            DeputyManager = domain.DeputyManager,
            Activities = domain.Activities.Select(a => a.ToEntity()).ToList(),
        };
        return entity;
    }

    /// <summary>
    /// Converts to resource.
    /// </summary>
    /// <param name="subproject">The DataAccess subproject.</param>
    /// <param name="actualSums">A Dictionary of activity ids and their Actual times.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static Model.Subproject ToDomain(this DataAccess.Subproject subproject, IDictionary<int, TimeSpan>? actualSums = null)
    {
        var domain = new Model.Subproject
        {
            Id = subproject.Id,
            Number = subproject.Number,
            Name = subproject.Name,
            ProjectNumber = subproject.ProjectNumber,
            ProjectName = subproject.ProjectName,
            CustomerName = subproject.CustomerName,
            ManagerId = subproject.ManagerId,
            Manager = subproject.Manager,
            DeputyManagerId = subproject.DeputyManagerId,
            DeputyManager = subproject.DeputyManager,
        };

        domain.Activities = subproject.Activities
                .Select(a => a.ToDomain(actualSums, domain))
                .ToList();

        return domain;
    }
}
