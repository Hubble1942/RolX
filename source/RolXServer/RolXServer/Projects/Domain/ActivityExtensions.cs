// -----------------------------------------------------------------------
// <copyright file="ActivityExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.DataAccess;

namespace RolXServer.Projects.Domain;

/// <summary>
/// Extension methods for <see cref="Activity"/> instances.
/// </summary>
internal static class ActivityExtensions
{
    /// <summary>
    /// Sanitizes the specified activities.
    /// </summary>
    /// <param name="activities">The activities.</param>
    internal static void Sanitize(this IEnumerable<Model.Activity> activities)
    {
        foreach (var activity in activities)
        {
            activity.Sanitize();
        }
    }
}
