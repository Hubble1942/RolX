// -----------------------------------------------------------------------
// <copyright file="PartTimeSettingValidator.cs" company="Christian Ewald">
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
/// Validator for <see cref="PartTimeSetting"/> instances.
/// </summary>
public sealed class PartTimeSettingValidator : AbstractValidator<PartTimeSetting>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartTimeSettingValidator"/> class.
    /// </summary>
    public PartTimeSettingValidator()
    {
        this.RuleFor(s => s.StartDate)
            .NotEmpty()
            .SetValidator(new IsoDateValidator<PartTimeSetting>());
        this.RuleFor(s => s.Factor)
            .GreaterThan(0)
            .LessThanOrEqualTo(1);
    }
}
