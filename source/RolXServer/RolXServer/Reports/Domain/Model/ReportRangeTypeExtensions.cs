// -----------------------------------------------------------------------
// <copyright file="ReportRangeTypeExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// Extension methods for <see cref="ReportRange"/> values.
/// </summary>
public static class ReportRangeTypeExtensions
{
    /// <summary>
    /// Returns a label describing the report range type.
    /// </summary>
    /// <param name="type">The report range type.</param>
    /// <returns>The label.</returns>
    public static string ToLabel(this ReportRangeType type)
    => type switch
    {
        ReportRangeType.Custom => "Benutzerdefiniert",
        ReportRangeType.Today => "Heute",
        ReportRangeType.Yesterday => "Gestern",
        ReportRangeType.ThisWeek => "Diese Woche",
        ReportRangeType.LastWeek => "Letzte Woche",
        ReportRangeType.LastSevenDays => "Letzten 7 Tage",
        ReportRangeType.LastMonth => "Letzter Monat",
        ReportRangeType.LastThirtyDays => "Letzten 30 Tage",
        ReportRangeType.LastYear => "Letztes Jahr",
        ReportRangeType.LastThreeSixFiveDays => "Letzten 365 Tage",
        ReportRangeType.StartToNow => "Start bis Jetzt",
        ReportRangeType.StartToMonth => "Start bis Monat",
        _ => "Ungültig",
    };
}
