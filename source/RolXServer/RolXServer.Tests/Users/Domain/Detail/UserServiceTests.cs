// -----------------------------------------------------------------------
// <copyright file="UserServiceTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Moq;
using RolXServer.AuditLogs.Domain;
using RolXServer.Common.Errors;
using RolXServer.Users.DataAccess;
using RolXServer.Users.Domain.Model;

namespace RolXServer.Users.Domain.Detail;

public sealed class UserServiceTests
{
    private Guid userId;
    private RolXContext context = null!;
    private UserService sut = null!;

    [SetUp]
    public void SetUp()
    {
        this.userId = Guid.NewGuid();

        var user = new User
        {
            Id = this.userId,
        };

        this.context = InMemory.ContextFactory(user)();
        var auditLogService = Mock.Of<IAuditLogService>();
        this.sut = new UserService(this.context, auditLogService);
    }

    [TearDown]
    public void TearDown()
    {
        this.context.Dispose();
    }

    [Test]
    public async Task Update_UserMustExist()
    {
        var user = new UpdatableUser
        {
            Id = Guid.NewGuid(),
        };

        Func<Task> act = async () => { await this.sut.Update(user); };
        await act.Should().ThrowAsync<ItemNotFoundException>();
    }

    [TestCase(Role.User)]
    [TestCase(Role.Supervisor)]
    [TestCase(Role.Administrator)]
    public async Task Update_Role(Role role)
    {
        var user = new UpdatableUser
        {
            Id = this.userId,
            Role = role,
        };

        await this.sut.Update(user);

        (await this.sut.GetById(this.userId))
            !.Role.Should().Be(role);
    }

    [Test]
    public async Task Update_EntryDate()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);

        var user = new UpdatableUser
        {
            Id = this.userId,
            EntryDate = date,
        };

        await this.sut.Update(user);

        (await this.sut.GetById(this.userId))
            !.EntryDate.Should().Be(date);
    }

    [Test]
    public async Task Update_LeavingDate()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);

        var user = new UpdatableUser
        {
            Id = this.userId,
            LeftDate = date,
        };

        await this.sut.Update(user);

        (await this.sut.GetById(this.userId))
            !.LeftDate.Should().Be(date);
    }

    [Test]
    public async Task Update_PartTimeSettings()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var expectedFactor = 0.5;

        var user = new UpdatableUser
        {
            Id = this.userId,
            PartTimeSettings = ImmutableList.Create<UserPartTimeSetting>(
                new UserPartTimeSetting()
                {
                    UserId = this.userId,
                    Factor = expectedFactor,
                    StartDate = date,
                }),
        };
        await this.sut.Update(user);

        (await this.sut.GetById(this.userId))
            !.PartTimeSettings
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Match<UserPartTimeSetting>(s =>
                s.Factor == expectedFactor
                && s.StartDate == date
                && s.UserId == this.userId);
    }

    [Test]
    public async Task Update_VacationDaysSettings()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var expectedVacationDays = 30;

        var user = new UpdatableUser
        {
            Id = this.userId,
            VacationDaysSettings = ImmutableList.Create(
                new UserVacationDaysSetting()
                {
                    UserId = this.userId,
                    VacationDaysPerYear = expectedVacationDays,
                    StartDate = date,
                }),
        };
        await this.sut.Update(user);

        (await this.sut.GetById(this.userId))
            !.VacationDaysSettings
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Match<UserVacationDaysSetting>(s =>
                s.VacationDaysPerYear == expectedVacationDays
                && s.StartDate == date
                && s.UserId == this.userId);
    }
}
