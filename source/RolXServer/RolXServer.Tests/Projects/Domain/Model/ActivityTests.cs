// -----------------------------------------------------------------------
// <copyright file="ActivityTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.Domain.Model;

namespace RolXServer.Projects.Domain;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public sealed class ActivityTests
{
    private readonly Subproject subproject = new()
    {
        ProjectNumber = 4711,
        Number = 1,
        Activities = new List<Activity>
        {
            new Activity
            {
                Number = 3,
            },
        },
    };

    public ActivityTests()
    {
        foreach (var activity in this.subproject.Activities)
        {
            activity.Subproject = this.subproject;
        }
    }

    [Test]
    public void FullNumber()
    {
        this.subproject.Activities[0].FullNumber
            .Should().Be("#4711.001.03");
    }

    [Test]
    public void Sanitize_Budget()
    {
        var activity = this.subproject.Activities[0];
        activity.Budget = TimeSpan.FromMinutes(0.5);
        activity.Sanitize();

        activity.Budget.Should().BeNull();
    }

    [Test]
    public void Sanitize_Planned()
    {
        var activity = this.subproject.Activities[0];
        activity.Planned = TimeSpan.FromMinutes(0.5);
        activity.Sanitize();

        activity.Planned.Should().BeNull();
    }
}
