// -----------------------------------------------------------------------
// <copyright file="PartTimeSettingsValidatorTests.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Validation;

public sealed class PartTimeSettingsValidatorTests
{
    private PartTimeSettingValidator sut = null!;

    [SetUp]
    public void SetUp()
    {
        this.sut = new PartTimeSettingValidator();
    }

    [Test]
    public void StartDate_MustNotBeEmpty()
    {
        var model = new PartTimeSetting(string.Empty, 1);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    public void StartDate_MustBeValidIsoDate()
    {
        var model = new PartTimeSetting("2019-12-14", 1);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    public void StartDate_MustNotBeInvalidIsoDate()
    {
        var model = new PartTimeSetting("2019-13-14", 1);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.StartDate);
    }

    [Test]
    public void Factor_MustNotBeNan()
    {
        var model = new PartTimeSetting("2019-12-14", double.NaN);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.Factor);
    }

    [Test]
    [TestCase(1)]
    [TestCase(0.5)]
    [TestCase(0.1234)]
    [TestCase(0.69)]
    public void Factor_MustBeBetweenZeroAndOneInclusive(double value)
    {
        var model = new PartTimeSetting("2019-12-14", value);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(setting => setting.Factor);
    }

    [Test]
    public void Factor_MustNotBeZero()
    {
        var model = new PartTimeSetting("2019-12-14", 0);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.Factor);
    }

    [Test]
    [TestCase(-42)]
    [TestCase(-1)]
    public void Factor_MustNotBeNegative(double value)
    {
        var model = new PartTimeSetting("2019-12-14", value);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.Factor);
    }

    [Test]
    [TestCase(42)]
    [TestCase(1.5)]
    public void Factor_MustNotBGreaterThanOne(double value)
    {
        var model = new PartTimeSetting("2019-12-14", value);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(setting => setting.Factor);
    }
}
