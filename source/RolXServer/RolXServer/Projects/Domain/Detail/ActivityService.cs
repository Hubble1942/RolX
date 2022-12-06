// -----------------------------------------------------------------------
// <copyright file="ActivityService.cs" company="Christian Ewald">
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
/// Provides access to <see cref="Activity"/> instances.
/// </summary>
internal sealed class ActivityService : IActivityService
{
    private readonly RolXContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityService" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ActivityService(RolXContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Model.Activity>> GetAll(DateOnly? unlessEndedBefore)
    {
        var query = this.dbContext.Activities
            .AsNoTracking()
            .Include(a => a.Subproject)
            .Include(a => a.Billability)
            .AsQueryable();

        if (unlessEndedBefore.HasValue)
        {
            query = query.Where(a => !a.EndedDate.HasValue || a.EndedDate.Value > unlessEndedBefore.Value);
        }

        return (await query.ToListAsync()).GroupBy(a => a.Subproject).SelectMany(group =>
        {
            var subproject = group.Key!.ToDomain();
            return group.Select(a => a.ToDomain(subproject: subproject));
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Model.Activity>> GetSuitable(Guid userId, DateOnly date)
    {
        var begin = date.AddDays(-7);
        var end = date.AddDays(7);

        return (await this.dbContext.Records
            .Where(r => r.UserId == userId && r.Date >= begin && r.Date < end)
            .Include(r => r.Entries)
                .ThenInclude(e => e.Activity)
                    .ThenInclude(a => a!.Subproject)
            .Include(r => r.Entries)
                .ThenInclude(e => e.Activity)
                    .ThenInclude(a => a!.Billability)
            .SelectMany(r => r.Entries)
            .Select(e => e.Activity!)
            .Distinct()
            .AsNoTracking()
            .ToListAsync()).Select(a => a.ToDomain());
    }

    /// <inheritdoc/>
    public async Task<IDictionary<int, TimeSpan>> GetActualSums(int subprojectId)
        => await this.dbContext.RecordEntries
            .Where(entry => entry.Activity!.SubprojectId == subprojectId)
            .GroupBy(entry => entry.ActivityId)
            .Select(group => new
            {
                ActivityId = group.Key,
                Sum = group.Sum(entry => entry.DurationSeconds),
            })
            .ToDictionaryAsync(item => item.ActivityId, item => TimeSpan.FromSeconds(item.Sum));
}
