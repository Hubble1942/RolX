// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using RolXServer.AuditLogs.Domain;
using RolXServer.Common.Errors;
using RolXServer.Users.DataAccess;
using RolXServer.Users.Domain.Mapping;
using RolXServer.Users.Domain.Model;

namespace RolXServer.Users.Domain.Detail;

/// <summary>
/// Provides access to <see cref="User"/> instances.
/// </summary>
internal sealed class UserService : IUserService
{
    private readonly RolXContext context;
    private readonly IAuditLogService auditLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="auditLogService">The audit log service.</param>
    public UserService(RolXContext context, IAuditLogService auditLogService)
    {
        this.context = context;
        this.auditLogService = auditLogService;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>
    /// The Users.
    /// </returns>
    public async Task<IEnumerable<User>> GetAll()
    {
        return await this.context.Users.AsNoTracking().Include(u => u.PartTimeSettings).ToListAsync();
    }

    /// <summary>
    /// Gets a user by the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The user or <c>null</c> if none has been found.</returns>
    public async Task<User?> GetById(Guid id)
    {
        return await this.context.Users
            .AsNoTracking()
            .Include(u => u.PartTimeSettings.OrderBy(s => s.StartDate))
            .Include(u => u.VacationDaysSettings.OrderBy(s => s.StartDate))
            .Include(u => u.BalanceCorrections.OrderBy(c => c.Date))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Updates the specified user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The async task.</returns>
    public async Task Update(UpdatableUser user)
    {
        var oldUser = await this.GetById(user.Id);
        this.auditLogService.GenerateAuditLog(oldUser?.ToUpdatableUser(), user);
        var entity = new User
        {
            Id = user.Id,
            Role = user.Role,
            EntryDate = user.EntryDate,
            LeftDate = user.LeftDate,
            IsConfirmed = true,
        };

        var entry = this.context.Users.Attach(entity);
        entry.Property(e => e.Role).IsModified = true;
        entry.Property(e => e.EntryDate).IsModified = true;
        entry.Property(e => e.LeftDate).IsModified = true;
        entry.Property(e => e.IsConfirmed).IsModified = true;

        await this.MergePartTimeSettings(user);
        await this.MergeVacationDaysSettings(user);
        await this.MergeBalanceCorrections(user);

        try
        {
            await this.context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new ItemNotFoundException($"No user with id '{user.Id}' found.", e);
        }
    }

    private async Task MergeBalanceCorrections(UpdatableUser user)
    {
        var balanceCorrectionsDates = user.BalanceCorrections.Select(s => s.Date).ToHashSet();
        var oldBalanceCorrections = await this.context.UserBalanceCorrections
            .AsNoTracking()
            .Where(c => c.UserId == user.Id)
            .ToListAsync();

        var orphanBalanceCorrections = oldBalanceCorrections.Where(c => !balanceCorrectionsDates.Contains(c.Date));

        this.context.UserBalanceCorrections.UpdateRange(
            user.BalanceCorrections.Select(s =>
            {
                s.Id = oldBalanceCorrections.FirstOrDefault(os => os.Date == s.Date)?.Id ?? 0;
                return s;
            }));

        this.context.UserBalanceCorrections.RemoveRange(orphanBalanceCorrections);
    }

    private async Task MergePartTimeSettings(UpdatableUser user)
    {
        var partTimeStartDates = user.PartTimeSettings.Select(s => s.StartDate).ToHashSet();
        var partTimeOldSettings = await this.context.UserPartTimeSettings
            .AsNoTracking()
            .Where(s => s.UserId == user.Id)
            .ToListAsync();

        var partTimeOrphanSettings = partTimeOldSettings.Where(s => !partTimeStartDates.Contains(s.StartDate));

        this.context.UserPartTimeSettings.UpdateRange(
            user.PartTimeSettings.Select(s =>
            {
                s.Id = partTimeOldSettings.FirstOrDefault(os => os.StartDate == s.StartDate)?.Id ?? 0;
                return s;
            }));

        this.context.UserPartTimeSettings.RemoveRange(partTimeOrphanSettings);
    }

    private async Task MergeVacationDaysSettings(UpdatableUser user)
    {
        var vacationDaysStartDates = user.VacationDaysSettings.Select(s => s.StartDate).ToHashSet();
        var vacationDaysOldSettings = await this.context.UserVacationDaysSettings
            .AsNoTracking()
            .Where(s => s.UserId == user.Id)
            .ToListAsync();

        var vacationDaysOrphanSettings = vacationDaysOldSettings.Where(s => !vacationDaysStartDates.Contains(s.StartDate));

        this.context.UserVacationDaysSettings.UpdateRange(
            user.VacationDaysSettings.Select(s =>
            {
                s.Id = vacationDaysOldSettings.FirstOrDefault(os => os.StartDate == s.StartDate)?.Id ?? 0;
                return s;
            }));

        this.context.UserVacationDaysSettings.RemoveRange(vacationDaysOrphanSettings);
    }
}
