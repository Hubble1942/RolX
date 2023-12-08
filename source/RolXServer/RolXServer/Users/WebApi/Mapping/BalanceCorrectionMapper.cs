// -----------------------------------------------------------------------
// <copyright file="BalanceCorrectionMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;

namespace RolXServer.Users.WebApi.Mapping;

/// <summary>
/// Maps users from / to resource.
/// </summary>
internal static class BalanceCorrectionMapper
{
    /// <summary>
    /// Maps the specified domain to resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The mapped resource.</returns>
    public static Resource.BalanceCorrection ToResource(this DataAccess.UserBalanceCorrection domain)
        => new(domain.Date.ToIsoDate(), domain.Overtime, domain.Vacation);

    /// <summary>
    /// Converts the specified resource to domain.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="userId">The userid the resoucre belongs to. This property is not stored in the resource definition but required in the domain.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static DataAccess.UserBalanceCorrection ToDomain(this Resource.BalanceCorrection resource, Guid userId)
        => new() { Date = IsoDate.Parse(resource.Date), Vacation = resource.Vacation, Overtime = resource.Overtime, UserId = userId };
}
