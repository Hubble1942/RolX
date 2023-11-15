// -----------------------------------------------------------------------
// <copyright file="VacationDaysMapper.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;
using RolXServer.Users.DataAccess;
using RolXServer.Users.WebApi.Resource;

namespace RolXServer.Users.WebApi.Mapping;

/// <summary>
/// Maps users from / to resource.
/// </summary>
internal static class VacationDaysMapper
{
    /// <summary>
    /// Maps the specified domain to resource.
    /// </summary>
    /// <param name="domain">The domain.</param>
    /// <returns>The mapped resource.</returns>
    public static VacationDaysSetting ToResource(this UserVacationDaysSetting domain)
        => new(domain.StartDate.ToIsoDate(), domain.VacationDaysPerYear);

    /// <summary>
    /// Converts the specified resource to domain.
    /// </summary>
    /// <param name="resource">The resource.</param>
    /// <param name="userId">The userid the resoucre belongs to. This property is not stored in the resource definition but required in the domain.</param>
    /// <returns>
    /// The domain.
    /// </returns>
    public static UserVacationDaysSetting ToDomain(this VacationDaysSetting resource, Guid userId)
        => new UserVacationDaysSetting { StartDate = IsoDate.Parse(resource.StartDate), VacationDaysPerYear = resource.VacationDays, UserId = userId };
}
