// -----------------------------------------------------------------------
// <copyright file="UserMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.DataAccess;
using RolXServer.Users.Domain.Model;

namespace RolXServer.Users.Domain.Mapping;

/// <summary>
/// Maps users to domain.
/// </summary>
internal static class UserMapper
{
    /// <summary>
    /// Maps the specified user to an updatable user.
    /// </summary>>
    /// <param name="entity">The entity.</param>
    /// <returns>The records.</returns>
    public static UpdatableUser ToUpdatableUser(this User entity)
    {
        return new UpdatableUser
        {
            Id = entity.Id,
            Role = entity.Role,
            EntryDate = entity.EntryDate,
            LeftDate = entity.LeftDate,
            PartTimeSettings = entity.PartTimeSettings.ToImmutableList(),
            VacationDaysSettings = entity.VacationDaysSettings.ToImmutableList(),
            BalanceCorrections = entity.BalanceCorrections.ToImmutableList(),
        };
    }
}
