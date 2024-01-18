// -----------------------------------------------------------------------
// <copyright file="WorkItemGroup.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Reports.Domain.Model;

/// <summary>
/// A named group of <see cref="WorkItem"/> instances.
/// </summary>
public sealed record WorkItemGroup(
    string Name,
    IImmutableList<WorkItem> Items);
