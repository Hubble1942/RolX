// -----------------------------------------------------------------------
// <copyright file="BalanceCorrectionValidatorTest.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Validation;

public sealed class BalanceCorrectionValidatorTest
{
    private BalanceCorrectionValidator sut = null!;

    [SetUp]
    public void SetUp()
    {
        this.sut = new BalanceCorrectionValidator();
    }

    [Test]
    public void StartDate_MustNotBeEmpty()
    {
        var model = new BalanceCorrection(string.Empty, TimeSpan.Zero, TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Date);
    }

    [Test]
    public void StartDate_MustBeValidIsoDate()
    {
        var model = new BalanceCorrection("2019-12-14", TimeSpan.Zero, TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(correction => correction.Date);
    }

    [Test]
    public void StartDate_MustNotBeInvalidIsoDate()
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.Zero, TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Date);
    }

    [Test]
    [TestCase(1000001)]
    [TestCase(1000000.02)]
    [TestCase(99999999)]
    public void Vacation_MustNotBeBGreaterThanAMillionHours(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.Zero, TimeSpan.FromHours(hours));

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Vacation);
    }

    [Test]
    [TestCase(-1000001)]
    [TestCase(-1000000.02)]
    [TestCase(-99999999)]
    public void Vacation_MustNotBeBSmallerThanMinusAMillionHours(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.Zero, TimeSpan.FromHours(hours));

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Vacation);
    }

    [Test]
    [TestCase(1000000)]
    [TestCase(999999)]
    [TestCase(0.5)]
    [TestCase(0)]
    [TestCase(-0.69)]
    [TestCase(-999999)]
    [TestCase(-1000000)]
    public void Vacation_MustBeBetweenPlusAndMinusAMillionHoursInclusive(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.Zero, TimeSpan.FromHours(hours));

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(correction => correction.Vacation);
    }

    [Test]
    [TestCase(1000001)]
    [TestCase(1000000.02)]
    [TestCase(99999999)]
    public void Overtime_MustNotBeBGreaterThanAMillionHours(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.FromHours(hours), TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Overtime);
    }

    [Test]
    [TestCase(-1000001)]
    [TestCase(-1000000.02)]
    [TestCase(-99999999)]
    public void Overtime_MustNotBeBSmallerThanMinusAMillionHours(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.FromHours(hours), TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldHaveValidationErrorFor(correction => correction.Overtime);
    }

    [Test]
    [TestCase(1000000)]
    [TestCase(999999)]
    [TestCase(0.5)]
    [TestCase(0)]
    [TestCase(-0.69)]
    [TestCase(-999999)]
    [TestCase(-1000000)]
    public void Overtime_MustBeBetweenPlusAndMinusAMillionHoursInclusive(double hours)
    {
        var model = new BalanceCorrection("2019-13-14", TimeSpan.FromHours(hours), TimeSpan.Zero);

        this.sut.TestValidate(model).ShouldNotHaveValidationErrorFor(correction => correction.Overtime);
    }
}
