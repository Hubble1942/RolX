// -----------------------------------------------------------------------
// <copyright file="VacationBudgetEvaluation.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;
using RolXServer.Users.DataAccess;

namespace RolXServer.Records.Domain.Detail.Balances;

/// <summary>
/// Extension methods for <see cref="DateRange"/> instances.
/// </summary>
internal static class VacationBudgetEvaluation
{
    /// <summary>
    /// Gets the vacation budget for the specified user up to the specified year.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="year">The year.</param>
    /// <param name="defaultVacationDaysPerYear">The default vacation days per year.</param>
    /// <param name="nominalWorkTimePerDay">The nominal work time per day.</param>
    /// <returns>The vacation budget.</returns>
    public static TimeSpan VacationBudget(this User user, int year, int defaultVacationDaysPerYear, TimeSpan nominalWorkTimePerDay)
    {
        return user.RangesWithEmploymentSettings(year, defaultVacationDaysPerYear)
            .Sum(t => t.Range.VacationDays(t.VacationDays) * t.Factor * nominalWorkTimePerDay);
    }

    /// <summary>
    /// Gets the number of vacation days for the specified range.
    /// </summary>
    /// <param name="range">The range.</param>
    /// <param name="vacationDaysPerYear">The number of vacation days per year.</param>
    /// <returns>The number of vacation days.</returns>
    public static double VacationDays(this DateRange range, int vacationDaysPerYear)
    {
        return range.TotalMonths() * vacationDaysPerYear / 12.0;
    }

    /// <summary>
    /// Gets an Enumerable of temporally sorted changes to the users part time and vacation day settings.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>An Enumerable with StartDate and the settings updated on that date.</returns>
    private static IOrderedEnumerable<(DateOnly StartDate, double? Factor, int? VacationDays)> EmploymentSettings(this User user)
    {
        var partTimeJoinVacationDays = user.PartTimeSettings
                                            .GroupJoin(
                                                user.VacationDaysSettings,
                                                pts => new { pts.UserId, pts.StartDate },
                                                vds => new { vds.UserId, vds.StartDate },
                                                (pts, vds) => new { pts, vds })
                                            .SelectMany(
                                                temp => temp.vds.DefaultIfEmpty(),
                                                (temp, uvds) => (temp.pts.StartDate, temp.pts?.Factor, uvds?.VacationDaysPerYear));

        var vacationDaysJoinPartTime = user.VacationDaysSettings
                                            .GroupJoin(
                                                user.PartTimeSettings,
                                                vds => new { vds.UserId, vds.StartDate },
                                                pts => new { pts.UserId, pts.StartDate },
                                                (vds, pts) => new { vds, pts })
                                            .SelectMany(
                                                temp => temp.pts.DefaultIfEmpty(),
                                                (temp, upts) => (temp.vds.StartDate, upts?.Factor, temp.vds?.VacationDaysPerYear));

        return partTimeJoinVacationDays.Union(vacationDaysJoinPartTime).OrderBy(x => x.StartDate);
    }

    private static IEnumerable<(DateRange Range, double Factor, int VacationDays)> RangesWithEmploymentSettings(this User user, int year, int defaultVacationDaysPerYear)
    {
        var lastStartDate = user.EntryDate;
        var lastFactor = 1.0;
        var lastVacationDays = defaultVacationDaysPerYear;

        foreach (var setting in user.EmploymentSettings())
        {
            if (setting.StartDate.Year > year)
            {
                yield return (new DateRange(lastStartDate, new DateOnly(year + 1, 1, 1)), lastFactor, lastVacationDays);
                yield break;
            }

            if (setting.StartDate > lastStartDate)
            {
                yield return (new DateRange(lastStartDate, setting.StartDate), lastFactor, lastVacationDays);

                lastStartDate = setting.StartDate;
            }

            lastFactor = setting.Factor ?? lastFactor;
            lastVacationDays = setting.VacationDays ?? lastVacationDays;
        }

        var endDate = user.LeftDate.HasValue && user.LeftDate.Value.Year == year
            ? user.LeftDate.Value
            : new DateOnly(year + 1, 1, 1);

        yield return (new DateRange(lastStartDate, endDate), lastFactor, lastVacationDays);
    }

    private static double TotalMonths(this DateRange range)
    {
        return (range.End.AbsoluteMonths() - range.Begin.AbsoluteMonths())
            - range.Begin.DayInMonthFactor()
            + range.End.DayInMonthFactor();
    }

    private static int AbsoluteMonths(this DateOnly date)
    {
        return (date.Year * 12) + date.Month - 1;
    }

    private static double DayInMonthFactor(this DateOnly date)
    {
        return (date.Day - 1.0) / DateTime.DaysInMonth(date.Year, date.Month);
    }
}
