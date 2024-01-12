// -----------------------------------------------------------------------
// <copyright file="SubprojectService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RolXServer.AuditLogs.Domain;
using RolXServer.Projects.DataAccess;
using RolXServer.Projects.Domain.Mapping;

namespace RolXServer.Projects.Domain.Detail;

/// <summary>
/// Provides access to <see cref="Subproject"/> instances.
/// </summary>
internal sealed class SubprojectService : ISubprojectService
{
    private readonly RolXContext dbContext;
    private readonly IActivityService activityService;
    private readonly IAuditLogService auditLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubprojectService" /> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// /// <param name="activityService">The activity service.</param>
    /// <param name="auditLogService">The audit log service.</param>
    public SubprojectService(RolXContext dbContext, IActivityService activityService, IAuditLogService auditLogService)
    {
        this.dbContext = dbContext;
        this.activityService = activityService;
        this.auditLogService = auditLogService;
    }

    /// <summary>
    /// Gets all subprojects.
    /// </summary>
    /// <returns>
    /// The subprojects.
    /// </returns>
    public async Task<IEnumerable<Model.Subproject>> GetAll() => (await this.dbContext.Subprojects
        .AsNoTracking()
        .Include(p => p.Manager)
        .Include(p => p.DeputyManager)
        .Include(p => p.Architect)
        .Include(p => p.Activities)
        .ThenInclude(a => a.Billability)
        .ToListAsync())
        .Select(p => p.ToDomain());

    /// <summary>
    /// Gets a subproject by the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>
    /// The subproject or <c>null</c> if none has been found.
    /// </returns>
    public async Task<Model.Subproject?> GetById(int id)
    {
        var actualSums = await this.activityService.GetActualSums(id);
        return (await this.dbContext.Subprojects
            .AsNoTracking()
            .Include(p => p.Manager)
            .Include(p => p.DeputyManager)
            .Include(p => p.Architect)
            .Include(p => p.Activities)
            .ThenInclude(a => a.Billability)
            .FirstOrDefaultAsync(p => p.Id == id))?.ToDomain(actualSums);
    }

    /// <summary>
    /// Adds the specified subproject.
    /// </summary>
    /// <param name="subproject">The subproject.</param>
    /// <returns>The async task.</returns>
    public async Task<Model.Subproject> Add(Model.Subproject subproject)
    {
        subproject.Activities.Sanitize();

        this.auditLogService.GenerateAuditLog(null, subproject);

        var entity = subproject.ToEntity();
        this.dbContext.Subprojects.Add(entity);
        await this.dbContext.SaveChangesAsync();
        return entity.ToDomain();
    }

    /// <summary>
    /// Updates the specified subproject.
    /// </summary>
    /// <param name="subproject">The subproject.</param>
    /// <returns>The async task.</returns>
    public async Task Update(Model.Subproject subproject)
    {
        subproject.Activities.Sanitize();

        var oldSubproject = await this.GetById(subproject.Id);

        this.auditLogService.GenerateAuditLog(oldSubproject, subproject);

        var activityIds = subproject.Activities
            .Select(a => a.Id);

        var orphanActivities = await this.dbContext.Subprojects
            .Where(s => s.Id == subproject.Id)
            .SelectMany(s => s.Activities)
            .Where(a => !activityIds.Contains(a.Id))
            .ToListAsync();

        this.dbContext.Subprojects.Update(subproject.ToEntity());
        this.dbContext.RemoveRange(orphanActivities);

        await this.dbContext.SaveChangesAsync();
    }
}
