// -----------------------------------------------------------------------
// <copyright file="FavouriteService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RolXServer.Projects.DataAccess;
using RolXServer.Projects.Domain.Mapping;

namespace RolXServer.Projects.Domain.Detail;

/// <summary>
/// Provides access to favourite <see cref="Activity"/> instances.
/// </summary>
internal sealed class FavouriteService : IFavouriteService
{
    private readonly RolXContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FavouriteService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public FavouriteService(RolXContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all favourite activities of the specified user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// The favourite activities.
    /// </returns>
    public async Task<IEnumerable<Model.Activity>> GetAll(Guid userId)
    {
        return (await this.context.FavouriteActivities
            .Where(f => f.UserId == userId)
            .Include(f => f.Activity)
                .ThenInclude(a => a!.Subproject)
            .Include(f => f.Activity)
                .ThenInclude(a => a!.Billability)
            .Select(f => f.Activity!)
            .AsNoTracking()
            .ToListAsync())
            .Select(p => p.ToDomain());
    }

    /// <summary>
    /// Adds the specified activity to the favourites of the specified user.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// The async task.
    /// </returns>
    public async Task Add(Model.Activity activity, Guid userId)
    {
        this.context.FavouriteActivities.Add(activity.ToFavouriteActivity(userId));
        await this.context.SaveChangesAsync();
    }

    /// <summary>
    /// Removes the specified activity from the favourites of the specified user.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// The async task.
    /// </returns>
    public async Task Remove(Model.Activity activity, Guid userId)
    {
        this.context.FavouriteActivities.Remove(activity.ToFavouriteActivity(userId));
        await this.context.SaveChangesAsync();
    }
}
