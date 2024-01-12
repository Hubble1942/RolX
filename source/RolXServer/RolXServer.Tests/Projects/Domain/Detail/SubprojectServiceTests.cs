// -----------------------------------------------------------------------
// <copyright file="SubprojectServiceTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Moq;
using RolXServer.AuditLogs.Domain;
using RolXServer.Projects.DataAccess;
using RolXServer.Projects.Domain.Mapping;

namespace RolXServer.Projects.Domain.Detail;

public sealed class SubprojectServiceTests
{
    private static Subproject SeedSubproject => new()
    {
        Id = 1,
        Number = 1,
        Name = "One",
        Activities = new List<Activity>
            {
                new Activity
                {
                    Number = 1,
                    Name = "One",
                    Billability = new() { Id = 1 },
                },
                new Activity
                {
                    Number = 2,
                    Name = "Two",
                    Billability = new() { Id = 2 },
                },
            },
    };

    [Test]
    public async Task Update_ExistingActivityChanged()
    {
        var contextFactory = InMemory.ContextFactory(SeedSubproject);

        using (var context = contextFactory())
        {
            IActivityService activityService = new ActivityService(context);
            var auditLogService = Mock.Of<IAuditLogService>();
            var sut = new SubprojectService(context, activityService, auditLogService);
            var subproject = SeedSubproject.ToDomain();
            subproject.Activities[0].Name = "Changed";
            await sut.Update(subproject);
        }

        using (var context = contextFactory())
        {
            context.Subprojects
                .Include(s => s.Activities)
                .Single(s => s.Id == 1)
                .Activities.Single(a => a.Number == 1)
                .Name.Should().Be("Changed");
        }
    }

    [Test]
    public async Task Update_ExistingActivityRemoved()
    {
        var contextFactory = InMemory.ContextFactory(SeedSubproject);

        using (var context = contextFactory())
        {
            IActivityService activityService = new ActivityService(context);
            var auditLogService = Mock.Of<IAuditLogService>();
            var sut = new SubprojectService(context, activityService, auditLogService);
            var subproject = SeedSubproject.ToDomain();
            subproject.Activities.RemoveAt(0);
            await sut.Update(subproject);
        }

        using (var context = contextFactory())
        {
            context.Subprojects
                .Include(s => s.Activities)
                .Single(s => s.Id == 1)
                .Activities.Count.Should().Be(1);
        }
    }

    [Test]
    public async Task Update_NewActivityAdded()
    {
        var contextFactory = InMemory.ContextFactory(SeedSubproject);

        using (var context = contextFactory())
        {
            IActivityService activityService = new ActivityService(context);
            var auditLogService = Mock.Of<IAuditLogService>();
            var sut = new SubprojectService(context, activityService, auditLogService);
            var subproject = SeedSubproject.ToDomain();
            subproject.Activities.Add(new Model.Activity
            {
                Number = 3,
                Name = "Three",
                Subproject = subproject,
                Billability = new() { Id = 3 },
            });
            await sut.Update(subproject);
        }

        using (var context = contextFactory())
        {
            context.Subprojects
                .Include(s => s.Activities)
                .Single(s => s.Id == 1)
                .Activities.Count.Should().Be(3);
        }
    }
}
