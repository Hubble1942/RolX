// -----------------------------------------------------------------------
// <copyright file="PaidLeaveActivities.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Projects.Domain.Detail;

/// <summary>
/// Provides the (virtual) paid leave activities.
/// </summary>
public sealed class PaidLeaveActivities : IPaidLeaveActivities
{
    private readonly Dictionary<PaidLeaveType, Model.Activity> activities;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaidLeaveActivities"/> class.
    /// </summary>
    public PaidLeaveActivities()
    {
        this.activities = Enum.GetValues<PaidLeaveType>()
            .ToDictionary(e => e, e => new Model.Activity
            {
                Id = ((int)e) + 1,
                Number = ((int)e) + 1,
                Name = e.ToPrettyString(),
                StartDate = new DateOnly(2020, 1, 1),
                Subproject = this.Subproject,
                Billability = new() // Abwesenheit. See RolXContext.SeedBillabilities
                {
                    Id = 7,
                },
            });

        this.Subproject.Activities = this.activities.Values.ToList();
    }

    /// <inheritdoc/>
    public Model.Subproject Subproject { get; } = new()
    {
        Id = 1,
        ProjectNumber = 8900,
        Number = 1,
        CustomerName = "M&F",
        ProjectName = "Allgemein",
        Name = "Bezahlte Abwesenheiten",
    };

    /// <inheritdoc/>
    public Model.Activity this[PaidLeaveType paidLeaveType]
        => this.activities[paidLeaveType];
}
