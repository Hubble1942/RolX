// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Users.WebApi.Resource;

/// <summary>
/// The user resource.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the avatar URL.
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets the entry date.
    /// </summary>
    public string? EntryDate { get; set; }

    /// <summary>
    /// Gets or sets the date the user leaves.
    /// </summary>
    public string? LeavingDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this user is confirmed.
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// Gets or sets the list of part time settings for the user.
    /// </summary>
    public IImmutableList<PartTimeSetting> PartTimeSettings { get; set; } = ImmutableList<PartTimeSetting>.Empty;

    /// <summary>
    /// Gets or sets the list of vacation days settings for the user.
    /// </summary>
    public IImmutableList<VacationDaysSetting> VacationDaysSettings { get; set; } = ImmutableList<VacationDaysSetting>.Empty;
}
