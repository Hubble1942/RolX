// -----------------------------------------------------------------------
// <copyright file="UserVacationDaysSetting.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Users.DataAccess;

/// <summary>
/// The part-time setting of a user.
/// </summary>
public sealed class UserVacationDaysSetting
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the start date this setting is applicable.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Gets or sets the vacation days per year.
    /// </summary>
    public int VacationDaysPerYear { get; set; } = 25;
}
