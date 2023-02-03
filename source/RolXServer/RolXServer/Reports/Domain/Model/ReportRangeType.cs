// -----------------------------------------------------------------------
// <copyright file="ReportRangeType.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// Enum of all possible report range types.
/// </summary>
public enum ReportRangeType
{
    /// <summary>
    /// Custom date range.
    /// </summary>
    Custom,

    /// <summary>
    /// The current day.
    /// </summary>
    Today,

    /// <summary>
    /// The previous day.
    /// </summary>
    Yesterday,

    /// <summary>
    /// The current week.
    /// </summary>
    ThisWeek,

    /// <summary>
    /// The previous week.
    /// </summary>
    LastWeek,

    /// <summary>
    /// The last seven days.
    /// </summary>
    LastSevenDays,

    /// <summary>
    /// The previous month.
    /// </summary>
    LastMonth,

    /// <summary>
    /// The last thirty days.
    /// </summary>
    LastThirtyDays,

    /// <summary>
    /// The previous year.
    /// </summary>
    LastYear,

    /// <summary>
    /// The last three hundred and sixty five days.
    /// </summary>
    LastThreeSixFiveDays,

    /// <summary>
    /// From subproject start date to now.
    /// </summary>
    StartToNow,

    /// <summary>
    /// From subproject start date to specified month.
    /// </summary>
    StartToMonth,
}
