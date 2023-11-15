// -----------------------------------------------------------------------
// <copyright file="UpdatableUser.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Users.WebApi.Resource;

/// <summary>
/// The updatable part of a user.
/// </summary>
public class UpdatableUser
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the entry date.
    /// </summary>
    public string EntryDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the user leaves.
    /// </summary>
    public string? LeavingDate { get; set; }

    /// <summary>
    /// Gets or sets the part time settings for the user.
    /// </summary>
    public IImmutableList<PartTimeSetting> PartTimeSettings { get; set; } = ImmutableList<PartTimeSetting>.Empty;

    /// <summary>
    /// Gets or sets the vacation days settings for the user.
    /// </summary>
    public IImmutableList<VacationDaysSetting> VacationDaysSettings { get; set; } = ImmutableList<VacationDaysSetting>.Empty;
}
