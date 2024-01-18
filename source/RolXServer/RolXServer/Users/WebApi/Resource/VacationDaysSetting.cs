// -----------------------------------------------------------------------
// <copyright file="VacationDaysSetting.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Users.WebApi.Resource;

/// <summary>
/// A vacation days setting of a user.
/// </summary>
public sealed record VacationDaysSetting(string StartDate, int VacationDays);
