// -----------------------------------------------------------------------
// <copyright file="RecordExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using RolXServer.Common.Util;
using RolXServer.Records;
using RolXServer.Records.Domain;
using RolXServer.Records.Domain.Model;
using RolXServer.Reports.Domain.Model;

namespace RolXServer.Reports.Domain.Detail;

/// <summary>
/// Extension methods for <see cref="Record"/> instances.
/// </summary>
public static class RecordExtensions
{
    /// <summary>
    /// Converts the specified records to a <see cref="WorkItemGroup"/> for all paid leaves.
    /// </summary>
    /// <param name="records">The records.</param>
    /// <returns>The paid-leave group.</returns>
    public static WorkItemGroup ToPaidLeaveWorkItemGroup(this IEnumerable<Record> records)
        => new WorkItemGroup(
            "Abwesenheiten",
            records
                .Where(record => record.PaidLeaveType.HasValue)
                .GroupBy(record => record.PaidLeaveType!.Value)
                .OrderBy(group => group.Key)
                .Select(group => new WorkItem(
                    group.Key.ToPrettyString(),
                    group.Sum(record => record.PaidLeaveTime())))
                .ToImmutableList());
}