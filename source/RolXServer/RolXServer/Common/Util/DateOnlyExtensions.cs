// -----------------------------------------------------------------------
// <copyright file="DateOnlyExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Common.Util;

/// <summary>
/// Extension methods for <see cref="DateOnly"/> instances.
/// </summary>
public static class DateOnlyExtensions
{
    /// <summary>
    /// Returns start of the current week (Monday).
    /// </summary>
    /// <param name="dateOnly">The DateTime.</param>
    /// <returns>The DateTime of Monday of this week.</returns>
    public static DateOnly StartOfWeek(this DateOnly dateOnly)
    {
        int daysFromLastMonday = dateOnly.DayOfWeek == DayOfWeek.Sunday ? 6 : dateOnly.DayOfWeek - DayOfWeek.Monday;
        return dateOnly.AddDays(-daysFromLastMonday);
    }

    /// <summary>
    /// Returns start of the current month.
    /// </summary>
    /// <param name="dateOnly">The DateTime.</param>
    /// <returns>The DateTime the first day this month.</returns>
    public static DateOnly StartOfMonth(this DateOnly dateOnly) => new DateOnly(dateOnly.Year, dateOnly.Month, 1);
}
