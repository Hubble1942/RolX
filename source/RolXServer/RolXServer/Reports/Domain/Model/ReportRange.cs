// -----------------------------------------------------------------------
// <copyright file="ReportRange.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// A range for a subproject report.
/// </summary>
public sealed class ReportRange
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReportRange"/> class.
    /// </summary>
    public ReportRange()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportRange"/> class.
    /// </summary>
    /// <param name="type">The report range type.</param>
    /// <param name="customStart">The optional start date.</param>
    /// <param name="customEnd">The optional end date.</param>
    /// <exception cref="ArgumentException">Thrown when a custom type is declared but no date range is given.</exception>
    public ReportRange(ReportRangeType type, DateOnly? customStart = null, DateOnly? customEnd = null)
    {
        this.Type = type;
        switch (type)
        {
            case ReportRangeType.Custom:
                if (!(customStart.HasValue && customEnd.HasValue))
                {
                    throw new ArgumentException("A date range must be supplied for custom report ranges types.");
                }

                this.CustomStart = customStart;
                this.CustomEnd = customEnd;
                break;
            case ReportRangeType.StartToMonth:
                if (!customEnd.HasValue)
                {
                    throw new ArgumentException("An end date must be supplied for start to month report ranges types.");
                }

                this.CustomEnd = customEnd;
                break;
            default: break;
        }
    }

    /// <summary>
    /// Gets or sets the report range type.
    /// </summary>
    public ReportRangeType Type { get; set; }

    /// <summary>
    /// Gets or sets the report range type.
    /// </summary>
    public DateOnly? CustomStart { get; set; }

    /// <summary>
    /// Gets or sets the report range type.
    /// </summary>
    public DateOnly? CustomEnd { get; set; }

    /// <summary>
    /// Converts to a DateRange.
    /// </summary>
    /// <param name="now">The current date.</param>
    /// <param name="subprojectCreationDate">The subproject creation date.</param>
    /// <returns>The date range.</returns>
    public DateRange ToDateRange(DateOnly now, DateOnly? subprojectCreationDate)
    => this.Type switch
    {
        ReportRangeType.Custom => new DateRange(begin: this.CustomStart!.Value, end: this.CustomEnd!.Value.AddDays(1)),
        ReportRangeType.Today => new DateRange(begin: now, end: now.AddDays(1)),
        ReportRangeType.Yesterday => new DateRange(begin: now.AddDays(-1), end: now),
        ReportRangeType.ThisWeek => new DateRange(begin: now.StartOfWeek(), end: now.AddDays(1)),
        ReportRangeType.LastWeek => new DateRange(begin: now.StartOfWeek().AddDays(-7), end: now.StartOfWeek()),
        ReportRangeType.LastSevenDays => new DateRange(begin: now.AddDays(-7), end: now.AddDays(1)),
        ReportRangeType.LastMonth => DateRange.ForMonth(now.AddMonths(-1)),
        ReportRangeType.LastThirtyDays => new DateRange(begin: now.AddDays(-30), end: now.AddDays(1)),
        ReportRangeType.LastYear => DateRange.ForYear(now.Year - 1),
        ReportRangeType.LastThreeSixFiveDays => new DateRange(begin: now.AddDays(-365), end: now.AddDays(1)),
        ReportRangeType.StartToNow => new DateRange(begin: subprojectCreationDate ?? now, end: now.AddDays(1)),
        ReportRangeType.StartToMonth => new DateRange(begin: subprojectCreationDate ?? now, end: this.CustomEnd!.Value.StartOfMonth()),
        _ => new DateRange(now, now.AddDays(1)),
    };
}
