// -----------------------------------------------------------------------
// <copyright file="VacationDaysSettingsValidatorTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Validation;

public sealed class VacationDaysSettingsValidatorTests
{
    private VacationDaysSettingValidator sut = null!;

    [SetUp]
    public void SetUp()
    {
        this.sut = new VacationDaysSettingValidator();
    }

    [Test]
    public void StartDate_MustNotBeEmpty()
    {
        var model = new VacationDaysSetting(string.Empty, 25);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    public void StartDate_MustBeValidIsoDate()
    {
        var model = new VacationDaysSetting("2019-12-14", 25);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    public void StartDate_MustNotBeInvalidIsoDate()
    {
        var model = new VacationDaysSetting("2019-13-14", 25);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    [TestCase(365)]
    [TestCase(200)]
    [TestCase(50)]
    [TestCase(25)]
    [TestCase(20)]
    public void VacationDays_MustBeBetweenZeroAndTwentyInclusive(int value)
    {
        var model = new VacationDaysSetting("2019-12-14", value);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(setting => setting.VacationDays);
    }

    [Test]
    public void VacationDays_MustNotBeLessThanTwenty()
    {
        var model = new VacationDaysSetting("2019-12-14", 10);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.VacationDays);
    }

    [Test]
    [TestCase(730)]
    [TestCase(366)]
    public void VacationDays_MustNotBGreaterThanOneYear(int value)
    {
        var model = new VacationDaysSetting("2019-12-14", value);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.VacationDays);
    }
}
