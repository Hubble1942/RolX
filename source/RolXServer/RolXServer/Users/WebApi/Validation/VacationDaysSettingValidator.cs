// -----------------------------------------------------------------------
// <copyright file="VacationDaysSettingValidator.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using FluentValidation;
using RolXServer.Common.Util;
using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Validation;

/// <summary>
/// Validator for <see cref="VacationDaysSetting"/> instances.
/// </summary>
public sealed class VacationDaysSettingValidator : AbstractValidator<VacationDaysSetting>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VacationDaysSettingValidator"/> class.
    /// </summary>
    public VacationDaysSettingValidator()
    {
        this.RuleFor(s => s.StartDate)
            .NotEmpty()
            .SetValidator(new IsoDateValidator<VacationDaysSetting>());
        this.RuleFor(s => s.VacationDays)
            .NotEmpty()
            .GreaterThanOrEqualTo(20)
            .LessThanOrEqualTo(365);
    }
}
