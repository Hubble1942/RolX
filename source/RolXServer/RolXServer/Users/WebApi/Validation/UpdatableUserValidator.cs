// -----------------------------------------------------------------------
// <copyright file="UpdatableUserValidator.cs" company="Christian Ewald">
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
/// Validator for <see cref="UpdatableUser"/> instances.
/// </summary>
public sealed class UpdatableUserValidator : AbstractValidator<UpdatableUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatableUserValidator"/> class.
    /// </summary>
    public UpdatableUserValidator()
    {
        this.RuleFor(u => u.Role).IsInEnum();

        this.RuleFor(u => u.EntryDate)
            .NotEmpty()
            .SetValidator(new IsoDateValidator<UpdatableUser>());

        this.RuleFor(u => u.LeavingDate)
            .NotEmpty()
            .SetValidator(new IsoDateValidator<UpdatableUser>())
            .GreaterThanOrEqualTo(u => u.EntryDate)
            .Unless(u => u.LeavingDate == null);

        this.RuleForEach(u => u.PartTimeSettings)
            .SetValidator(u => new PartTimeSettingValidator())
            .Must((u, s) => s.StartDate.CompareTo(u.EntryDate) >= 0 && s.StartDate.CompareTo(u.LeavingDate ?? "2999-12-31") <= 0);

        this.Transform(from: l => l.PartTimeSettings, to: l => l!.Select(s => s.StartDate))
            .Must(s => s.Distinct().Count() == s.Count())
            .WithMessage("All start dates must be unique");
    }
}
