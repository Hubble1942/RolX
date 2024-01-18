// -----------------------------------------------------------------------
// <copyright file="EditLock.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Records.WebApi.Resource;

/// <summary>
/// Represents the locking date. Changing records before this date is prohibited.
/// </summary>
public sealed record EditLock(string Date);
