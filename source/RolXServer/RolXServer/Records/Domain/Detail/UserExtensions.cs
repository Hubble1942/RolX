﻿// -----------------------------------------------------------------------
// <copyright file="UserExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using RolXServer.Common.Util;
using RolXServer.Records.Domain.Detail.Holidays;
using RolXServer.Records.Domain.Model;
using RolXServer.Users.DataAccess;

namespace RolXServer.Records.Domain.Detail
{
    /// <summary>
    /// Extensions methods for <see cref="User"/> instances.
    /// </summary>
    public static class UserExtensions
    {
        private static readonly List<RuleBase> HolidayRules = new List<RuleBase>
        {
            new RuleAtFixedDate("Neujahr", 1, 1),
            new RuleAtFixedDate("Berchtoldstag", 1, 2),
            new RuleAtFixedDate("Tag der Arbeit", 5, 1),
            new RuleAtFixedDate("Nationalfeiertag", 8, 1),
            new RuleAtFixedDate("Weihnachten", 12, 25),
            new RuleAtFixedDate("Stephanstag", 12, 26),
            new RuleEasterBased("Karfreitag", -2),
            new RuleEasterBased("Ostern", 0),
            new RuleEasterBased("Ostermontag", 1),
            new RuleEasterBased("Auffahrt", 39),
            new RuleEasterBased("Pfingsten", 49),
            new RuleEasterBased("Pfingstmontag", 50),
        };

        /// <summary>
        /// Gets the day-informations for the specified user in the specified range.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="range">The range.</param>
        /// <param name="nominalWorkTimePerDay">The nominal work time per day.</param>
        /// <returns>The day-informations.</returns>
        public static IEnumerable<DayInfo> DayInfos(this User user, DateRange range, TimeSpan nominalWorkTimePerDay)
        {
            var sortedSettings = user.Settings
                .OrderByDescending(s => s.StartDate)
                .ToList();

            var activeRange = new DateRange(
                user.EntryDate ?? range.Begin,
                user.LeavingDate?.AddDays(1) ?? range.End);

            return range.Days
                .Select(d => new DayInfo
                {
                    Date = d,
                    NominalWorkTime = activeRange.Contains(d) ? nominalWorkTimePerDay : default(TimeSpan),
                })
                .Select(d => ApplyWeekend(d))
                .Select(d => ApplyHoliday(d))
                .Select(d => ApplyPartTimeFactor(d, sortedSettings));
        }

        /// <summary>
        /// Gets the nominal work-time for the specified user in the specified range.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="range">The range.</param>
        /// <param name="nominalWorkTimePerDay">The nominal work time per day.</param>
        /// <returns>The nominal work-time.</returns>
        public static TimeSpan NominalWorkTime(this User user, DateRange range, TimeSpan nominalWorkTimePerDay)
        {
            return new TimeSpan(
                user.DayInfos(range, nominalWorkTimePerDay)
                .Sum(i => i.NominalWorkTime.Ticks));
        }

        /// <summary>
        /// Gets the nominal work-time for the specified user at the specified date.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="date">The date.</param>
        /// <param name="nominalWorkTimePerDay">The nominal work time per day.</param>
        /// <returns>
        /// The nominal work-time.
        /// </returns>
        public static TimeSpan NominalWorkTime(this User user, DateTime date, TimeSpan nominalWorkTimePerDay)
        {
            return new TimeSpan(
                user.DayInfos(new DateRange(date, date.AddDays(1)), nominalWorkTimePerDay)
                .Sum(i => i.NominalWorkTime.Ticks));
        }

        private static DayInfo ApplyWeekend(DayInfo info)
        {
            if (info.Date.DayOfWeek == DayOfWeek.Saturday || info.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                info.DayType = DayType.Weekend;
                info.NominalWorkTime = default;
            }

            return info;
        }

        private static DayInfo ApplyHoliday(DayInfo info)
        {
            var rule = HolidayRules.FirstOrDefault(r => r.IsMatching(info.Date));
            if (rule != null)
            {
                info.DayType = DayType.Holiday;
                info.DayName = rule.Name;
                info.NominalWorkTime = default;
            }

            return info;
        }

        private static DayInfo ApplyPartTimeFactor(DayInfo info, IEnumerable<UserSetting> settings)
        {
            var factor = settings
                .Where(s => s.StartDate <= info.Date)
                .Select(s => s.PartTimeFactor)
                .DefaultIfEmpty(1.0)
                .First();

            info.NominalWorkTime *= factor;

            return info;
        }
    }
}
