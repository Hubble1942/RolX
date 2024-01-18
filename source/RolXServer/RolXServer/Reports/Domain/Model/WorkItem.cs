// -----------------------------------------------------------------------
// <copyright file="WorkItem.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// A single work item.
/// </summary>
public sealed record WorkItem(string Name, TimeSpan Duration);
