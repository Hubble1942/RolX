// -----------------------------------------------------------------------
// <copyright file="BalanceCorrectionValidator.cs" company="Christian Ewald">
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
public sealed class BalanceCorrectionValidator : AbstractValidator<BalanceCorrection>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BalanceCorrectionValidator"/> class.
    /// </summary>
    public BalanceCorrectionValidator()
    {
        this.RuleFor(s => s.Date)
            .NotEmpty()
            .SetValidator(new IsoDateValidator<BalanceCorrection>());

        this.RuleFor(s => s.Overtime)
             .NotNull()
             .LessThanOrEqualTo(TimeSpan.FromHours(1000000))
             .GreaterThanOrEqualTo(TimeSpan.FromHours(-1000000));

        this.RuleFor(s => s.Vacation)
             .NotNull()
             .LessThanOrEqualTo(TimeSpan.FromHours(1000000))
             .GreaterThanOrEqualTo(TimeSpan.FromHours(-1000000));
    }
}
