// -----------------------------------------------------------------------
// <copyright file="UpdatableUserValidatorTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Validation;

public sealed class UpdatableUserValidatorTests
{
    private UpdatableUserValidator sut = null!;

    [SetUp]
    public void SetUp()
    {
        this.sut = new UpdatableUserValidator();
    }

    [Test]
    public void EntryDate_MustNotBeEmpty()
    {
        var model = new UpdatableUser
        {
            EntryDate = string.Empty,
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(record => record.EntryDate);
    }

    [Test]
    public void EntryDate_MustBeValidIsoDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(record => record.EntryDate);
    }

    [Test]
    public void EntryDate_MustNotBeInvalidIsoDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-13-14",
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(record => record.EntryDate);
    }

    [Test]
    public void LeavingDate_MayBeNull()
    {
        var model = new UpdatableUser
        {
            LeavingDate = null,
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void LeavingDate_MustNotBeEmpty()
    {
        var model = new UpdatableUser
        {
            LeavingDate = string.Empty,
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void LeavingDate_Valid()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-11-14",
            LeavingDate = "2019-12-14",
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void LeavingDate_MustBeValidIsoDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            LeavingDate = "2019-13-14",
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void LeavingDate_MayBeSameAsEntryDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            LeavingDate = "2019-12-14",
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void LeavingDate_MustBeAfterEntryDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            LeavingDate = "2019-12-13",
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(record => record.LeavingDate);
    }

    [Test]
    public void PartTimeSettings_MayBeEmpty()
    {
        var model = new UpdatableUser
        {
            PartTimeSettings = ImmutableList<PartTimeSetting>.Empty,
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.PartTimeSettings);
    }

    [Test]
    public void PartTimeSettings_MustNotHaveInvalidEntries()
    {
        var model = new UpdatableUser
        {
            PartTimeSettings = ImmutableList.Create(
                    new PartTimeSetting("2019-13-14", 1)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("PartTimeSettings[0].StartDate");
    }

    [Test]
    public void PartTimeSettings_MustNotHaveEntriesBeforeEntryDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            PartTimeSettings = ImmutableList.Create(
                    new PartTimeSetting("2019-11-14", 1),
                    new PartTimeSetting("2020-12-14", 1)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("PartTimeSettings[0]");
    }

    [Test]
    public void PartTimeSettings_MustNotHaveEntriesAfterLeavingDate()
    {
        var model = new UpdatableUser
        {
            LeavingDate = "2020-11-14",
            PartTimeSettings = ImmutableList.Create(
                    new PartTimeSetting("2019-11-14", 1),
                    new PartTimeSetting("2020-12-14", 1)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("PartTimeSettings[1]");
    }

    [Test]
    public void PartTimeSettings_MustNotContainMultipleEntriesWithSameStartDate()
    {
        var model = new UpdatableUser
        {
            PartTimeSettings = ImmutableList.Create(
                new PartTimeSetting("2019-12-14", 1),
                new PartTimeSetting("2019-12-14", 0.4)),
        };

        this.sut.TestValidate(model)
            .ShouldHaveValidationErrorFor(u => u.PartTimeSettings)
            .WithErrorMessage("All part time start dates must be unique");
    }

    [Test]
    public void PartTimeSettings_MustSucceedWithValidEntries()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-11-14",
            PartTimeSettings = ImmutableList.Create(
                new PartTimeSetting("2019-12-14", 1),
                new PartTimeSetting("2020-12-14", 0.4)),
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.PartTimeSettings);
    }

    [Test]
    public void VacationDaysSettings_MayBeEmpty()
    {
        var model = new UpdatableUser
        {
            VacationDaysSettings = ImmutableList<VacationDaysSetting>.Empty,
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.VacationDaysSettings);
    }

    [Test]
    public void VacationDaysSettings_MustNotHaveInvalidEntries()
    {
        var model = new UpdatableUser
        {
            VacationDaysSettings = ImmutableList.Create(
                    new VacationDaysSetting("2019-13-14", 25)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("VacationDaysSettings[0].StartDate");
    }

    [Test]
    public void VacationDaysSettings_MustNotHaveEntriesBeforeEntryDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            VacationDaysSettings = ImmutableList.Create(
                    new VacationDaysSetting("2019-11-14", 25),
                    new VacationDaysSetting("2020-12-14", 25)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("VacationDaysSettings[0]");
    }

    [Test]
    public void VacationDaysSettings_MustNotHaveEntriesAfterLeavingDate()
    {
        var model = new UpdatableUser
        {
            LeavingDate = "2020-11-14",
            VacationDaysSettings = ImmutableList.Create(
                    new VacationDaysSetting("2019-11-14", 25),
                    new VacationDaysSetting("2020-12-14", 25)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("VacationDaysSettings[1]");
    }

    [Test]
    public void VacationDaysSettings_MustNotContainMultipleEntriesWithSameStartDate()
    {
        var model = new UpdatableUser
        {
            VacationDaysSettings = ImmutableList.Create(
                new VacationDaysSetting("2019-12-14", 25),
                new VacationDaysSetting("2019-12-14", 26)),
        };

        this.sut.TestValidate(model)
            .ShouldHaveValidationErrorFor(u => u.VacationDaysSettings)
            .WithErrorMessage("All vacation days start dates must be unique");
    }

    [Test]
    public void VacationDaysSettings_MustSucceedWithValidEntries()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-11-14",
            VacationDaysSettings = ImmutableList.Create(
                new VacationDaysSetting("2019-12-14", 25),
                new VacationDaysSetting("2020-12-14", 30)),
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.VacationDaysSettings);
    }

    [Test]
    public void BalanceCorrections_MayBeEmpty()
    {
        var model = new UpdatableUser
        {
            BalanceCorrections = ImmutableList<BalanceCorrection>.Empty,
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.BalanceCorrections);
    }

    [Test]
    public void BalanceCorrections_MustNotHaveEntriesBeforeEntryDate()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-12-14",
            BalanceCorrections = ImmutableList.Create(
                    new BalanceCorrection("2019-11-14", TimeSpan.Zero, TimeSpan.Zero),
                    new BalanceCorrection("2020-12-14", TimeSpan.Zero, TimeSpan.Zero)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("BalanceCorrections[0]");
    }

    [Test]
    public void BalanceCorrections_MustNotHaveEntriesAfterLeavingDate()
    {
        var model = new UpdatableUser
        {
            LeavingDate = "2020-11-14",
            BalanceCorrections = ImmutableList.Create(
            new BalanceCorrection("2019-11-14", TimeSpan.Zero, TimeSpan.Zero),
            new BalanceCorrection("2020-12-14", TimeSpan.Zero, TimeSpan.Zero)),
        };

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor("BalanceCorrections[1]");
    }

    [Test]
    public void BalanceCorrections_MustNotContainMultipleEntriesWithSameDate()
    {
        var model = new UpdatableUser
        {
            BalanceCorrections = ImmutableList.Create(
            new BalanceCorrection("2019-11-14", TimeSpan.Zero, TimeSpan.Zero),
            new BalanceCorrection("2019-11-14", TimeSpan.Zero, TimeSpan.Zero)),
        };

        this.sut.TestValidate(model)
            .ShouldHaveValidationErrorFor(u => u.BalanceCorrections)
            .WithErrorMessage("All balance correction dates must be unique");
    }

    [Test]
    public void BalanceCorrections_MustSucceedWithValidEntries()
    {
        var model = new UpdatableUser
        {
            EntryDate = "2019-11-14",
            BalanceCorrections = ImmutableList.Create(
            new BalanceCorrection("2019-11-14", TimeSpan.Zero, TimeSpan.Zero),
            new BalanceCorrection("2020-12-14", TimeSpan.Zero, TimeSpan.Zero)),
        };

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(u => u.VacationDaysSettings);
    }
}
