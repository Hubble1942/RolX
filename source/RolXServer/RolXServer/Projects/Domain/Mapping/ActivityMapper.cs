// -----------------------------------------------------------------------
// <copyright file="ActivityMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Projects.Domain.Mapping;

/// <summary>
/// Maps activities from / to domain.
/// </summary>
internal static class ActivityMapper
{
    /// <summary>
    /// Converts to domain.
    /// </summary>
    /// <param name="activity">The DataAccess activity.</param>
    /// <param name="actualSums">A Dictionary of activity ids and their Actual times.</param>
    /// <param name="subproject">The Domain.Model subproject.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static Model.Activity ToDomain(this DataAccess.Activity activity, IDictionary<int, TimeSpan>? actualSums = null, Model.Subproject? subproject = null) =>
        new()
        {
            Id = activity.Id,
            Number = activity.Number,
            Name = activity.Name,
            StartDate = activity.StartDate,
            EndedDate = activity.EndedDate,
            Budget = activity.Budget,
            Actual = actualSums?.GetValueOrDefault(activity.Id),
            Subproject = subproject ?? activity.Subproject?.ToDomain(),
            Billability = activity.Billability!,
        };

    /// <summary>
    /// Converts to entity.
    /// </summary>
    /// <param name="activity">The DataAccess activity.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static DataAccess.Activity ToEntity(this Model.Activity activity) =>
    new()
    {
        Id = activity.Id,
        Number = activity.Number,
        Name = activity.Name,
        StartDate = activity.StartDate,
        EndedDate = activity.EndedDate,
        Budget = activity.Budget,
        SubprojectId = activity.Subproject?.Id ?? 0,
        BillabilityId = activity.Billability.Id,
    };
}
