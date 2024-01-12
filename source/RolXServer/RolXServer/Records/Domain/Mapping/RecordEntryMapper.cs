// -----------------------------------------------------------------------
// <copyright file="RecordEntryMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.Domain.Mapping;

namespace RolXServer.Records.Domain.Mapping;

/// <summary>
/// Maps record entries from / to resource.
/// </summary>
internal static class RecordEntryMapper
{
    /// <summary>
    /// Converts to resource.
    /// </summary>
    /// <param name="entity">The domain.</param>
    /// <returns>The resource.</returns>
    public static Model.RecordEntry ToDomain(this DataAccess.RecordEntry entity)
    {
        return new Model.RecordEntry
        {
            ActivityId = entity.ActivityId,
            Activity = entity.Activity?.ToDomain(),
            Duration = entity.Duration,
            Begin = entity.Begin,
            Pause = entity.Pause,
            Comment = entity.Comment,
        };
    }

    /// <summary>
    /// Converts to domain.
    /// </summary>
    /// <param name="domain">The resource.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static DataAccess.RecordEntry ToEntity(this Model.RecordEntry domain)
    {
        return new DataAccess.RecordEntry
        {
            ActivityId = domain.ActivityId,
            Activity = domain.Activity?.ToEntity(),
            Duration = domain.Duration,
            Begin = domain.Begin,
            Pause = domain.Pause,
            Comment = domain.Comment,
        };
    }
}
