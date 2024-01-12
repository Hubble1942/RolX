// -----------------------------------------------------------------------
// <copyright file="RecordEntryMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Records.WebApi.Mapping;

/// <summary>
/// Maps record entries from / to resource.
/// </summary>
internal static class RecordEntryMapper
{
    /// <summary>
    /// Converts to resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The resource.</returns>
    public static Resource.RecordEntry ToResource(this Domain.Model.RecordEntry domain)
    {
        return new Resource.RecordEntry
        {
            ActivityId = domain.ActivityId,
            FullActivityNumber = domain.Activity!.FullNumber,
            Duration = (long)domain.Duration.TotalSeconds,
            Begin = (int?)domain.Begin?.ToTimeSpan().TotalSeconds,
            Pause = (int?)domain.Pause?.TotalSeconds,
            Comment = domain.Comment,
        };
    }

    /// <summary>
    /// Converts to domain.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static Domain.Model.RecordEntry ToDomain(this Resource.RecordEntry resource)
    {
        return new Domain.Model.RecordEntry
        {
            ActivityId = resource.ActivityId,
            Duration = TimeSpan.FromSeconds(resource.Duration),
            Begin = resource.Begin.HasValue ? TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(resource.Begin.Value)) : null,
            Pause = resource.Pause.HasValue && resource.Pause > 0 ? TimeSpan.FromSeconds(resource.Pause.Value) : null,
            Comment = resource.Comment ?? string.Empty,
        };
    }
}
