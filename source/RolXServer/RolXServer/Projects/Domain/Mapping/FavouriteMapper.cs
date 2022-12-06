// -----------------------------------------------------------------------
// <copyright file="FavouriteMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.DataAccess;

namespace RolXServer.Projects.Domain.Mapping;

/// <summary>
/// Maps activities from / to domain.
/// </summary>
public static class FavouriteMapper
{
    /// <summary>
    /// Converts to entity.
    /// </summary>
    /// <param name="activity">The DataAccess activity.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static FavouriteActivity ToFavouriteActivity(this Model.Activity activity, Guid userId) => new()
    {
        UserId = userId,
        ActivityId = activity.Id,
    };
}
